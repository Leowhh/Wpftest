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
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;

namespace Wpftest
{
    /// <summary>
    /// WpfRes.xaml 的交互逻辑
    /// </summary>
    public partial class WpfRes : Window
    {
        private BackgroundWorker backgroundWorker;
        public WpfRes()
        {
            InitializeComponent();
            backgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));
            string ip_addr = string.Empty;
            string HostName = Dns.GetHostName();//得到主机名；
            IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
            for (int i = 0; i < IpEntry.AddressList.Length; i++)
            {
                if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    ip_addr = IpEntry.AddressList[i].ToString();
                }
            }
            txtIp_addr.Text = ip_addr;

        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            string user_id = txtUser.Text.Trim();
            string user_pwd = txtPwd.Password;
            string ip_addr = txtIp_addr.Text.Trim();
            progressBar.IsIndeterminate = true;
            btn_OK.IsEnabled = false;
            UserResinfoInput input = new UserResinfoInput(user_id, user_pwd, ip_addr);

            backgroundWorker.RunWorkerAsync(input);
            
            
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            //WpfLogin log = new WpfLogin();
            //log.Show();
            Close();
        }

        private void Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void WindowsMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    DragMove();
                }
                catch { }
            }
        }
        private static void Res(string user_id,string user_pwd,string ip_addr, System.ComponentModel.BackgroundWorker backgroundWorker)
        {
            //加密
            user_pwd = Commons.EncodeHelper.AES_Encrypt(user_pwd);

            //实例化
            UserinfoServiceSoapClient us = new UserinfoServiceSoapClient();
            string message = us.Register(user_id, user_pwd, ip_addr);
            MessageBox.Show(message, "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);          
        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the input values.
            UserResinfoInput input = (UserResinfoInput)e.Argument;

            // Start the search for primes and wait.
            Res(input.User_id, input.User_pwd, input.Ip_addr, backgroundWorker);

            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Search cancelled.");
            }
            else if (e.Error != null)
            {
                // An error was thrown by the DoWork event handler.
                MessageBox.Show(e.Error.Message, "An Error Occurred");
            }
            else
            {
                    Close();
                    progressBar.IsIndeterminate = false;
                    
            }

        }

    }
}
