using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Beta.Model.ComponentSystem
{
    public class ComponentContext
    {
        public IGlobalAPI GlobalAPI { get; set; }

        #region Try
        public void InvokeMethodInMainWindow(string methodName,object[] args)
        {
            Assembly ass = Assembly.Load("Beta");
            Type type = ass.GetType("Beta.IGlobalAPI");
            MethodInfo method = type.GetMethod(methodName);           
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                method.Invoke(App.Current.MainWindow as MainWindow, args);
            }));

        }
        #endregion
    }
}
