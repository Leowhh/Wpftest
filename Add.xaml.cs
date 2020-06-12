using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpftest.SalesService;
using System.ComponentModel;
using Commons;

namespace Wpftest
{
    /// <summary>
    /// Add.xaml 的交互逻辑
    /// </summary>
    public partial class Add : Page
    {

        private BackgroundWorker backgroundWorker;
        private BackgroundWorker backgroundWorker1;
        public Add()
        {
            InitializeComponent();
            backgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));
            backgroundWorker1 = ((BackgroundWorker)this.FindResource("backgroundWorker1"));
        }          
           
            private static void Added(Sale sale, System.ComponentModel.BackgroundWorker backgroundWorker)
        {
            sale.sal_pwd = Commons.EncodeHelper.AES_Encrypt(sale.sal_pwd);

            SalesServiceSoapClient ss = new SalesServiceSoapClient();//实例化web服务对象
            
            //调用web服务中的添加
            string message = ss.AddSales(sale);
            MessageBox.Show(message, "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
            

        }
        private static void MountAdded(SaleGrade salegrade, System.ComponentModel.BackgroundWorker backgroundWorker)
        {
            SalesServiceSoapClient ss = new SalesServiceSoapClient();//实例化web服务对象

            //调用web服务中的添加
            string message = ss.AddSalesGrade(salegrade);
            MessageBox.Show(message, "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the input values.
            Sale sale = (Sale)e.Argument;

            // Start the search for primes and wait.
            Added(sale, backgroundWorker);

            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the input values.
            SaleGrade salegrade = (SaleGrade)e.Argument;

            // Start the search for primes and wait.
            MountAdded(salegrade, backgroundWorker);

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
                progressbar.IsIndeterminate = false;
                AddBtn.IsEnabled = true;
            }
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                Mountprogressbar.IsIndeterminate = false;
                GradeAdd.IsEnabled = true;
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            progressbar.IsIndeterminate = true;
            AddBtn.IsEnabled = false;
            SalesService.Sale sale = new Sale();//实例化对应表的实体（model）对象
            string number = sal_number.Text;
            if (sal_pwd.Password == "")
            {
                sal_pwd.Password = number.Substring(5, 6);
            }
 
            sale.sal_no = sal_no.Text.Trim();//控件中的值赋给实体对象的属性
            sale.sal_name = sal_name.Text.Trim();
            sale.sal_class = sal_class.Text.Trim();
            sale.sal_age = sal_age.Text.Trim();
            sale.sal_sex = sal_sex.Text.Trim();
            sale.sal_number = sal_number.Text.Trim();
            sale.sal_pwd = sal_pwd.Password;
           
            backgroundWorker.RunWorkerAsync(sale);
        }

        private void GradeAdd_Click(object sender, RoutedEventArgs e)
        {
            
            Mountprogressbar.IsIndeterminate = true;
            GradeAdd.IsEnabled = false;
            SalesService.SaleGrade salegrade = new SaleGrade();//实例化对应表的实体（model）对象

            salegrade.sal_no = No.Text.Trim();//控件中的值赋给实体对象的属性
            salegrade.sal_date = Date.Text.Trim();
            salegrade.sal_mount = Mount.Text.Trim();

            backgroundWorker1.RunWorkerAsync(salegrade);
           
        }
    }
}
