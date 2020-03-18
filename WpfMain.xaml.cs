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
using Wpftest.UserinfoService;

namespace Wpftest
{
    /// <summary>
    /// WpfMain.xaml 的交互逻辑
    /// </summary>
    public partial class WpfMain : Window
    {
        public static string user_id, last_date;

        

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
            Page.Navigate(new Dashboard());
        }

        private void Add_Checked(object sender, RoutedEventArgs e)
        {
            Page.Navigate(new Add());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            last_date = GetLastDate(user_id);
            info.ToolTip = "上次登录时间："+ last_date +"";
            UpdateDate(user_id);
            user.Text = user_id;
        }
    }
}
