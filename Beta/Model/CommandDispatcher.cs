using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Beta.Model.ComponentSystem;

namespace Beta.Model
{
    public static class CommandDispatcher
    {
        private static IEnumerable<Component> allComponents = Components.AllComponents;

        public static void Dispatch(Query query)
        {
            foreach (Component component in allComponents)
            {
                Component tempComponent = component;
                ThreadPool.QueueUserWorkItem(state =>
                {
                    List<Result> results = tempComponent.Query(query);
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        (App.Current.MainWindow as MainWindow).PushResult(results);
                    }));
                });
            }
        }
    }
}
