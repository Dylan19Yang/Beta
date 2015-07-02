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
    /// DetailWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DetailWindow : Window
    {
        private List<WebSearchEngine> webSearchEngines = new List<WebSearchEngine>();
        public int itemNumber{get;set;}
        private WebSearchEngine newWebSearchEngine = new WebSearchEngine();
        public DetailWindow()
        {
            InitializeComponent();
            webSearchEngines = UserSetting.Instance.WebSearchEngines;
            newWebSearchEngine= textInit();
        }
        public DetailWindow(int number)
        {
            InitializeComponent();
            itemNumber = number;
            webSearchEngines = UserSetting.Instance.WebSearchEngines;
            newWebSearchEngine = textInit();
        }

        public WebSearchEngine textInit()
        {
            if (itemNumber < 0) return new WebSearchEngine();
            WebSearchEngine webSerachEngine = webSearchEngines[itemNumber];
            textTitle.Text = webSerachEngine.Title;
            textActionWord.Text = webSerachEngine.ActionWord;
            textURL.Text = webSerachEngine.URL;
            return webSerachEngine;
        }

        private void buttonCancelMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void buttonOkMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (textTitle.Text != null && textActionWord.Text != null && textURL.Text != null)
            {
                newWebSearchEngine.Title = textTitle.Text;
                newWebSearchEngine.ActionWord = textActionWord.Text;
                newWebSearchEngine.URL = textURL.Text;
            }
            UserSetting.Instance.Save();
            this.Close();
        }
    }
}
