using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Forms;

using NHotkey;
using NHotkey.Wpf;

using Beta.Model;
using Beta.Utils;
using Beta.Settings;
using Beta.Model.ComponentSystem;

using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;
using TextBox = System.Windows.Controls.TextBox;
using ToolTip = System.Windows.Controls.ToolTip;
using Rectangle = System.Drawing.Rectangle;
using MessageBox = System.Windows.MessageBox;

namespace Beta
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, IGlobalAPI
    {
        #region Properties

        private NotifyIcon notifyIcon;
        private bool queryHasReturn;
        private string lastQuery;
        private ToolTip toolTip = new ToolTip();

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            // 初始化系统托盘
            InitTray();

            // 关联点击事件
            resultListBox.LeftMouseClickEvent += SelectResult;
            resultListBox.RightMouseClickEvent += ResultListBox_RightMouseClickEvent;

            // 关联快捷键 alt+space
            SetHotKey(UserSetting.Instance.HotKey, OnHotKey);

            // 设置线程池上限
            ThreadPool.SetMaxThreads(30, 10);

            // 建立索引
            ThreadPool.QueueUserWorkItem(o =>
            {
                Thread.Sleep(50);
                Components.Init();
                Console.WriteLine("组件初始化完毕！！！");
            });
        }

        #region UI Methods

        private void InitTray()
        {
            notifyIcon = new NotifyIcon { 
                Text = "Beta", 
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath), 
                Visible = true 
            };

            notifyIcon.Click += (o, e) => ShowBeta();

            // 打开菜单项
            var open = new MenuItem("Open");
            open.Click += (o, e) => ShowBeta();

            // 退出菜单项
            var exit = new MenuItem("Exit");
            exit.Click += (o, e) => CloseApp();

            // 添加到托盘
            MenuItem[] childen = { open, exit };
            notifyIcon.ContextMenu = new ContextMenu(childen);
        }

        #endregion

        #region Global API

        public void CloseApp()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                notifyIcon.Visible = false;
                Close();
                Environment.Exit(0);
            }));
        }

        public void HideApp()
        {
            Dispatcher.Invoke(new Action(HideBeta));
        }

        public void ShowApp()
        {
            Dispatcher.Invoke(new Action(() => ShowBeta()));
        }

        public void PushResult(List<Result> results)
        {
            OnUpdateResultView(results);
        }

        public bool ShellRun(string cmd, bool runAsAdministrator = false)
        {
            try
            {
                if (string.IsNullOrEmpty(cmd))
                    throw new ArgumentNullException();

                WindowsShellRun.Start(cmd, runAsAdministrator);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not start " + cmd + "\n" + ex.Message);
            }
            return false;
        }

        //void ShowMsg(string title, string subTitle, string iconPath)
        //{
        //    Dispatcher.Invoke(new Action(() =>
        //    {
        //        var m = new Msg { Owner = GetWindow(this) };
        //        m.Show(title, subTitle, iconPath);
        //    }));
        //}

        #endregion

        public void SetHotKey(string hotKeyStr, EventHandler<HotkeyEventArgs> action)
        {
            var hotkey = new HotKeyModel(hotKeyStr);
            try
            {
                HotkeyManager.Current.AddOrReplace(hotKeyStr, hotkey.CharKey, hotkey.ModifierKeys, action);
            }
            catch (Exception)
            {
                MessageBox.Show("Register hotkey: " + hotKeyStr + " failed.");
            }
        }

        public void RemoveHotKey(string hotKeyStr)
        {
            if (!string.IsNullOrEmpty(hotKeyStr))
            {
                HotkeyManager.Current.Remove(hotKeyStr);
            }
        }

        #region Actions

        private void ShowBeta(bool selectAllText = true)
        {
            if (!double.IsNaN(Left) && !double.IsNaN(Top))
            {
                var origScreen = Screen.FromRectangle(new Rectangle((int)Left, (int)Top, (int)ActualWidth, (int)ActualHeight));
                var screen = Screen.FromPoint(System.Windows.Forms.Cursor.Position);
                var coordX = (Left - origScreen.WorkingArea.Left) / (origScreen.WorkingArea.Width - ActualWidth);
                var coordY = (Top - origScreen.WorkingArea.Top) / (origScreen.WorkingArea.Height - ActualHeight);
                Left = (screen.WorkingArea.Width - ActualWidth) * coordX + screen.WorkingArea.Left;
                Top = (screen.WorkingArea.Height - ActualHeight) * coordY + screen.WorkingArea.Top;
            }

            Show();
            Activate();
            Focus();
            inputTextBox.Focus(); // 聚焦输入框

            // 默认自动全选
            if (selectAllText) inputTextBox.SelectAll();
        }

        private void HideBeta()
        {
            Hide();
        }

        private void SelectResult(Result result)
        {
            if (result != null)
            {
                result.Action();
            }
        }

        private Result GetActiveResult()
        {
            return resultListBox.GetActiveResult();
        }

        private void SelectPrevItem()
        {
            resultListBox.SelectPrev();
        }

        private void SelectNextItem()
        {
            resultListBox.SelectNext();
        }

        /// <summary>
        /// 处理alt+space快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHotKey(object sender, HotkeyEventArgs e)
        {
            if (!IsVisible)
            {
                ShowBeta();
            }
            else
            {
                HideBeta();
            }
            e.Handled = true;
        }

        public void OnUpdateResultView(List<Result> list)
        {
            if (list == null || list.Count == 0) return;

            if (list.Count > 0)
            {
                Dispatcher.Invoke(new Action(() =>
                   resultListBox.AddResults(list))
                );
            }
        }

        private void Border_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void InputTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            lastQuery = inputTextBox.Text;
            toolTip.IsOpen = false;
            resultListBox.Dirty = true;
            Dispatcher.DelayInvoke("UpdateSearch",
                o =>
                {
                    Dispatcher.DelayInvoke("ClearResults", i =>
                    {
                        // first try to use clear method inside pnlResult, which is more closer to the add new results
                        // and this will not bring splash issues.After waiting 30ms, if there still no results added, we
                        // must clear the result. otherwise, it will be confused why the query changed, but the results
                        // didn't.
                        if (resultListBox.Dirty) resultListBox.Clear();
                    }, TimeSpan.FromMilliseconds(100), null);
                    queryHasReturn = false;
                    var q = new Query(lastQuery);
                    CommandDispatcher.Dispatch(q);
                    if (Components.MatchComponentActionName(q))
                    {
                        Console.WriteLine("Match Component Action Name '{0}'", q);
                    }
                }, TimeSpan.FromMilliseconds(150));
        }

        void ResultListBox_RightMouseClickEvent(Result result)
        {
            
        }

        private void InputTextBox_OnPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //when alt is pressed, the real key should be e.SystemKey
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
            switch (key)
            {
                case Key.Escape:
                    HideBeta();
                    e.Handled = true;
                    break;

                case Key.Tab:
                    //if (globalHotkey.CheckModifiers().ShiftPressed)
                    //{
                    //    SelectPrevItem();
                    //}
                    //else
                    //{
                    //    SelectNextItem();
                    //}
                    e.Handled = true;
                    break;

                case Key.Down:
                    SelectNextItem();
                    e.Handled = true;
                    break;

                case Key.Up:
                    SelectPrevItem();
                    e.Handled = true;
                    break;

                case Key.PageDown:
                    resultListBox.SelectNextPage();
                    e.Handled = true;
                    break;

                case Key.PageUp:
                    resultListBox.SelectPrevPage();
                    e.Handled = true;
                    break;

                case Key.Back:
                    
                    break;

                case Key.F1:
                    
                    break;

                case Key.Enter:
                    Result activeResult = GetActiveResult();
                    SelectResult(activeResult);
                    e.Handled = true;
                    break;
            }
        }
        
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Left = (SystemParameters.PrimaryScreenWidth - ActualWidth) / 2;
            Top = (SystemParameters.PrimaryScreenHeight - ActualHeight) / 3;
        }

        private void MainWindow_OnDeactivated(object sender, EventArgs e)
        {
            HideBeta();
        }

        private void MainWindow_OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UserSetting.Instance.Save();
            HideBeta();
            e.Cancel = true;
        }

        #endregion
    }
}