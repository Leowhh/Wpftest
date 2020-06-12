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
    public partial class AddSal : Page
    {
        public static string sal_no;
        private BackgroundWorker backgroundWorker1;
        public AddSal()
        {
            InitializeComponent();
            backgroundWorker1 = ((BackgroundWorker)this.FindResource("backgroundWorker1"));
        }

        private static void MountAdded(SaleGrade salegrade, System.ComponentModel.BackgroundWorker backgroundWorker)
        {
            SalesServiceSoapClient ss = new SalesServiceSoapClient();//实例化web服务对象

            //调用web服务中的添加
            string message = ss.AddSalesGrade(salegrade);
            MessageBox.Show(message, "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the input values.
            SaleGrade salegrade = (SaleGrade)e.Argument;

            // Start the search for primes and wait.
            MountAdded(salegrade, backgroundWorker1);

            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
                return;
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

        private void GradeAdd_Click(object sender, RoutedEventArgs e)
        {

            Mountprogressbar.IsIndeterminate = true;
            GradeAdd.IsEnabled = false;
            SalesService.SaleGrade salegrade = new SaleGrade();//实例化对应表的实体（model）对象

            salegrade.sal_no = sal_no.Trim();//控件中的值赋给实体对象的属性
            salegrade.sal_date = Date.Text.Trim();
            salegrade.sal_mount = Mount.Text.Trim();

            backgroundWorker1.RunWorkerAsync(salegrade);

        }
    }
}
