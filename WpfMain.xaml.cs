using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpftest.UserinfoService;

namespace Wpftest
{
    /// <summary>
    /// WpfMain.xaml 的交互逻辑
    /// </summary>
    public partial class WpfMain : Window
    {
        public static string user_id, last_date, user_type;

        

        public WpfMain()
        {
            InitializeComponent();
            //显示登陆界面，验证后返回。
            WpfLogin login = new WpfLogin();
            login.ShowDialog();
            if (login.DialogResult != Convert.ToBoolean(1))
            {
                this.Close();
            }
            //显示登陆界面 结束
            Dashboard.IsChecked = true;
            if (user_type == "员工")
            {
                Delete.Visibility = Visibility.Hidden;
                Saerch.Visibility = Visibility.Hidden;
            }
        }
        public void UpdateDate(string user_id)
        {
            //DateTime DT = System.DateTime.Now;
            string dt = System.DateTime.Now.ToString();
            UserinfoServiceSoapClient us = new UserinfoServiceSoapClient();
            us.UpdateDate(dt, user_id);

        }
        public string GetLastDate(string user_id)
        {
            UserinfoServiceSoapClient us = new UserinfoServiceSoapClient();
            string dt = us.GetLastDate(user_id);
            if (dt == "")
                return "此次为首次登录";
            return dt;
        }
        private void Clo_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Max_Click(object sender, RoutedEventArgs e)
        {       
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
            
        }

        private void Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Rect r = new System.Windows.Rect(e.NewSize);
            RectangleGeometry gm = new RectangleGeometry(r, 5, 5);
            ((UIElement)sender).Clip = gm; 
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            WpfRes res = new WpfRes();
            res.ShowDialog();
        }

        private void Dashboard_Checked(object sender, RoutedEventArgs e)
        {
            if (user_type == "管理员")
                Page.Navigate(new Dashboard());
            else
            {
                DashboardSal.user_id = user_id;
                Page.Navigate(new DashboardSal());
            }
                
        }

        private void Add_Checked(object sender, RoutedEventArgs e)
        {
            if (user_type == "管理员")
                Page.Navigate(new Add());
            else
            {
                AddSal.sal_no = user_id;
                Page.Navigate(new AddSal());
            }
        }

        private void Delete_Checked(object sender, RoutedEventArgs e)
        {
            Page.Navigate(new Delete());
        }

        private void btn_openchart_Click(object sender, RoutedEventArgs e)
        {
             popusBottom2.IsOpen = false;
            popusBottom2.IsOpen = true;
        }

        private void Edit_Checked(object sender, RoutedEventArgs e)
        {
            if (user_type == "管理员")
                Page.Navigate(new Edit());
            else
            {
                EditSal.sal_no = user_id;
                Page.Navigate(new EditSal());
            }
                
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            last_date = GetLastDate(user_id);
            info.ToolTip = "上次登录时间：" + last_date + "";
            UpdateDate(user_id);
            user.Text = user_id;
            Type.Text = user_type;
        }
    }
    public class PopopHelper : Popup
    {
        
        /// <summary>  
        /// 是否窗口随动，默认为随动（true）  
        /// </summary>  
        public bool IsPositionUpdate
        {
            get { return (bool)GetValue(IsPositionUpdateProperty); }
            set { SetValue(IsPositionUpdateProperty, value); }
        }

        public static readonly DependencyProperty IsPositionUpdateProperty =
            DependencyProperty.Register("IsPositionUpdate", typeof(bool), typeof(PopopHelper), new PropertyMetadata(true, new PropertyChangedCallback(IsPositionUpdateChanged)));

        private static void IsPositionUpdateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PopopHelper).pup_Loaded(d as PopopHelper, null);
        }

        /// <summary>  
        /// 加载窗口随动事件  
        /// </summary>  
        public PopopHelper()
        {
            this.Loaded += pup_Loaded;
        }

        /// <summary>  
        /// 加载窗口随动事件  
        /// </summary>  
        private void pup_Loaded(object sender, RoutedEventArgs e)
        {
            Popup pup = sender as Popup;
            var win = VisualTreeHelper.GetParent(pup);
            while (win != null && (win as Window) == null)
            {
                win = VisualTreeHelper.GetParent(win);
            }
            if ((win as Window) != null)
            {
                (win as Window).LocationChanged -= PositionChanged;
                (win as Window).SizeChanged -= PositionChanged;
                if (IsPositionUpdate)
                {
                    (win as Window).LocationChanged += PositionChanged;
                    (win as Window).SizeChanged += PositionChanged;
                }
            }
        }

        /// <summary>  
        /// 刷新位置  
        /// </summary>  
        private void PositionChanged(object sender, EventArgs e)
        {
            try
            {
                var method = typeof(Popup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (this.IsOpen)
                {
                    method.Invoke(this, null);
                }
            }
            catch
            {
                return;
            }
        }

        //是否最前默认为非最前（false）  
        public static DependencyProperty TopmostProperty = Window.TopmostProperty.AddOwner(typeof(Popup), new FrameworkPropertyMetadata(false, OnTopmostChanged));
        public bool Topmost
        {
            get { return (bool)GetValue(TopmostProperty); }
            set { SetValue(TopmostProperty, value); }
        }
        private static void OnTopmostChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as PopopHelper).UpdateWindow();
        }

        /// <summary>  
        /// 重写拉开方法，置于非最前  
        /// </summary>  
        /// <param name="e"></param>  
        protected override void OnOpened(EventArgs e)
        {
            UpdateWindow();
        }

        /// <summary>  
        /// 刷新Popup层级  
        /// </summary>  
        private void UpdateWindow()
        {
            var hwnd = ((HwndSource)PresentationSource.FromVisual(this.Child)).Handle;
            RECT rect;
            if (NativeMethods.GetWindowRect(hwnd, out rect))
            {
                NativeMethods.SetWindowPos(hwnd, Topmost ? -1 : -2, rect.Left, rect.Top, (int)this.Width, (int)this.Height, 0);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        #region P/Invoke imports & definitions  
        public static class NativeMethods
        {


            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
            [DllImport("user32", EntryPoint = "SetWindowPos")]
            internal static extern int SetWindowPos(IntPtr hWnd, int hwndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        }
        #endregion
    
    }
}
