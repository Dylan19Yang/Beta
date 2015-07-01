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
        }

        public void AddWebSearchEngine(List<WebSearchEngine> webSearchEngines)
        {
            foreach (var webSearchEngine in webSearchEngines)
            {
                lbWebSearchEngines.Items.Add(webSearchEngine);
            }
        }

        private void buttonAddMouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void buttonEditMouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
