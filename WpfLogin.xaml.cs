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
using System.Net;
using System.Net.Sockets;
using Wpftest.UserinfoService;
using System.ComponentModel;

namespace Wpftest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WpfLogin : Window
    {
        private BackgroundWorker backgroundWorker;
        public WpfLogin()
        {
            InitializeComponent();
            backgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void WindowsMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton==MouseButtonState.Pressed)
            {
                try
                {
                    DragMove();
                }
                catch { }
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string user_id = txtUser.Text.Trim();
            string user_pwd = txtPwd.Password;

            progressBar.IsIndeterminate = true;

            UserinfoInput input = new UserinfoInput(user_id, user_pwd);
            
            backgroundWorker.RunWorkerAsync(input);
        }
        private static bool Login(string user_id,string user_pwd, System.ComponentModel.BackgroundWorker backgroundWorker)
        {
            
            //获取本机ip地址
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

            //加密            
            user_pwd = Commons.EncodeHelper.AES_Encrypt(user_pwd);
            //数据库验证
            UserinfoServiceSoapClient us = new UserinfoServiceSoapClient();
            string message = us.Login(user_id, user_pwd, ip_addr);
            if (string.IsNullOrEmpty(message))
            {
                WpfMain.user_id = user_id;
                return true;
                //DialogResult = Convert.ToBoolean(1);
            }
            else
            {
                MessageBox.Show(message, "信息提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            
        }

        private void Res_Click(object sender, RoutedEventArgs e)
        {
            //Hide();
            WpfRes res = new WpfRes();
            res.ShowDialog();
            
        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the input values.
            UserinfoInput input = (UserinfoInput)e.Argument;

            // Start the search for primes and wait.
            bool result = Login(input.User_id, input.User_pwd, backgroundWorker);

            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            // Return the result.
            e.Result = result;
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
                bool result = (bool)e.Result;
               if(result)
                {
                    this.DialogResult = Convert.ToBoolean(1);
                }
               else
                {
                    progressBar.IsIndeterminate = false;
                }
            }       
            
        }

       //private void ProC_Click(object sender, RoutedEventArgs e)
        //{
           //backgroundWorker.CancelAsync();
        //}
    

    }
    
}

