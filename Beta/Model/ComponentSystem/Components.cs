using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace Beta.Model.ComponentSystem
{
    public static class Components
    {
        private static List<Component> components = new List<Component>();
        public static List<Component> AllComponents
        {
            get
            {
                return components;
            }
        }

        public static void Init()
        {
            components.Clear();
            List<ComponentMetadata> componentMetadatas = ComponentLoader.LoadComponentMetadatas();

            // 将依据ComponentMetadata生成Component
            foreach (ComponentMetadata metadata in componentMetadatas)
            {
                try
                {
                    List<Type> componentTypes = Assembly.GetExecutingAssembly().GetTypes().Where(o => o.IsClass && !o.IsAbstract && (o.BaseType == typeof(Component) || o.GetInterfaces().Contains(typeof(IComponent)))).ToList();
                    if (componentTypes.Count == 0)
                    {
                        Console.WriteLine("Couldn't load component {0}. (Namespace: {1})", metadata.Name, metadata.Namespace);
                        continue;
                    }

                    foreach (Type type in componentTypes)
                    {
                        Component component = Activator.CreateInstance(type) as Component;
                        component.Metadata = metadata;
                        components.Add(component);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Couldn't load component {0}. (Namespace: {1}) : {2}", metadata.Name, metadata.Namespace, e.Message);
#if (DEBUG)
					{
						throw;
					}
#endif
                }
            }

            // 初始化各组件
            // TODO: 子线程无法访问 主窗体 所在线程，所以报错
            foreach (Component component in components)
            {
                Component tempComponent = component;
                ThreadPool.QueueUserWorkItem(o => component.Init(new ComponentContext()
                {
                    GlobalAPI = App.Current.MainWindow as MainWindow
                }));
            }
        }

        public static bool MatchComponentActionName(Query query)
        {
            if (string.IsNullOrEmpty(query.ActionName)) return false;

            return components.Any(o => o.Metadata.ActionKeywork == query.ActionName);
        }
    }
}
