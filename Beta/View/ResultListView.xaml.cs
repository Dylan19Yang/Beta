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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Beta.Model;

namespace Beta.View
{
    /// <summary>
    /// ResultListView.xaml 的交互逻辑
    /// </summary>
    public partial class ResultListView : UserControl
    {
        public bool Dirty { get; set; }
        public event Action<Result> LeftMouseClickEvent;
        public event Action<Result> RightMouseClickEvent;

        protected virtual void OnRightMouseClick(Result result)
        {
            Action<Result> handler = RightMouseClickEvent;
            if (handler != null) handler(result);
        }

        protected virtual void OnLeftMouseClick(Result result)
        {
            Action<Result> handler = LeftMouseClickEvent;
            if (handler != null) handler(result);
        }

        public ResultListView()
        {
            InitializeComponent();
        }

        private void Select(int index)
        {
            if (index >= 0 && index < lbResults.Items.Count)
            {
                lbResults.SelectedItem = lbResults.Items.GetItemAt(index);
            }
        }

        private void SelectFirst()
        {
            Select(0);
        }

        public void SelectNext()
        {
            int index = lbResults.SelectedIndex;
            if (index == lbResults.Items.Count - 1)
            {
                index = -1;
            }
            Select(index + 1);
        }

        public void SelectPrev()
        {
            int index = lbResults.SelectedIndex;
            if (index == 0)
            {
                index = lbResults.Items.Count;
            }
            Select(index - 1);
        }

        public void SelectNextPage()
        {
            int index = lbResults.SelectedIndex;
            index += 5;
            if (index >= lbResults.Items.Count)
            {
                index = lbResults.Items.Count - 1;
            }
            Select(index);
        }

        public void SelectPrevPage()
        {
            int index = lbResults.SelectedIndex;
            index -= 5;
            if (index < 0)
            {
                index = 0;
            }
            Select(index);
        }

        public void Clear()
        {
            lbResults.Items.Clear();
            lbResults.Margin = new Thickness { Top = 0 };
        }

        public Result GetActiveResult()
        {
            int index = lbResults.SelectedIndex;
            if (index < 0) return null;

            return lbResults.Items[index] as Result;
        }

        public void AddResults(List<Result> results)
        {
            if (Dirty)
            {
                Dirty = false;
                lbResults.Items.Clear();
            }
            foreach (var result in results)
            {
                lbResults.Items.Add(result);
            }
            lbResults.Margin = lbResults.Items.Count > 0 ? new Thickness { Top = 8 } : new Thickness { Top = 0 };
            SelectFirst();
        }

        private void lbResults_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] != null)
            {
                lbResults.ScrollIntoView(e.AddedItems[0]);
            }
        }

        private void lbResults_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(lbResults, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null && e.ChangedButton == MouseButton.Left)
            {
                OnLeftMouseClick(item.DataContext as Result);
            }
            if (item != null && e.ChangedButton == MouseButton.Right)
            {
                OnRightMouseClick(item.DataContext as Result);
            }
        }
    }
}
