using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Beta.Model;
using Beta.Settings;
using Beta.View;

namespace Beta.View
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();

            AddWebSearchEngine(UserSetting.Instance.WebSearchEngines);
            this.Activated += new EventHandler(Window_GetActivated);
            
        }

        public void AddWebSearchEngine(List<WebSearchEngine> webSearchEngines)
        {
            lbWebSearchEngines.Items.Clear();
            foreach (var webSearchEngine in webSearchEngines)
            {
                lbWebSearchEngines.Items.Add(webSearchEngine);
            }
        }

        void Window_GetActivated(object sender, EventArgs e)
        {
            AddWebSearchEngine(UserSetting.Instance.WebSearchEngines);
        } 

        public void ShowDetailWindow(int number)
        {
            /*
            Dispatcher.Invoke(new Action(() =>
            {
                var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.GetType() == typeof(DetailWindow))
                         ?? (DetailWindow)Activator.CreateInstance(typeof(DetailWindow));
                (window as DetailWindow).itemNumber = itemNumber;
                window.Show();
                window.Focus();
            }));
            */
            DetailWindow window = new DetailWindow(number);
            window.Show();
            window.WindowState = WindowState.Normal;
            window.Focus();
        }

        private void buttonAddMouseUp(object sender, MouseButtonEventArgs e)
        {
            ShowDetailWindow(-1);
        }

        private void buttonEditMouseUp(object sender, MouseButtonEventArgs e)
        {
            ShowDetailWindow(lbWebSearchEngines.SelectedIndex);
        }
    }
}
