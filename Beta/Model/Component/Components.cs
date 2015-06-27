using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Beta.Model.Component
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

        private static void Init()
        {
            components.Clear();
            List<ComponentMetadata> componentMetadatas = ComponentLoader.LoadComponents();

            // TODO: 将依据ComponentMetadata生成Component
            foreach (ComponentMetadata metadata in componentMetadatas)
            {
                // 大致思路为根据不同的命名空间，加载组件
                throw new NotImplementedException();
            }

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
