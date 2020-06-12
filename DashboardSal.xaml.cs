using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Wpftest.SalesService;

namespace Wpftest
{
    /// <summary>
    /// Dashboard.xaml 的交互逻辑
    /// </summary>
    public partial class DashboardSal : Page
    {
        public static string user_id;
        private BackgroundWorker backgroundWorkerUpdateMonth;
        public DashboardSal()
        {
            InitializeComponent();
            backgroundWorkerUpdateMonth = ((BackgroundWorker)this.FindResource("backgroundWorkerUpdateMonth"));
            Month.IsChecked = true;

        }
        private void Year_Checked(object sender, RoutedEventArgs e)
        {
            UpdateYear();
        }


        private void Month_Checked(object sender, RoutedEventArgs e)
        {
            string y = year.Text;
            TotalTip.Text = "年度销售总额";
            MaxTip.Text = "年度销售额最大月";
            MinTip.Text = "年度销售额最小月";
            month.Visibility = Visibility.Collapsed;
            if (y == "")
                ;
            else
            {
                UpdateMonth input = new UpdateMonth(y);

                backgroundWorkerUpdateMonth.RunWorkerAsync(input);
                //UpdateMonth(y);
            }
        }
        public void UpdateYear()
        {
            SeriesCollection series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "深圳",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 }
                },
                new LineSeries
                {
                    Title = "广州",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 }
                }
            };

            //x轴
            string[] Labels = new[] { "2011", "2012", "2013", "2014", "2015" };
            s1.Series = series;
            s1x.Labels = Labels;
            s1y.LabelFormatter = new Func<double, string>((value) =>
            {
                return value.ToString();
            });
        }
        public void UpdateMonth(Month month)
        {
            compare com = new compare();
            com.setYear(month);
            int maxMonth = com.maxMonth;
            int minMonth = com.minMonth;
            float max = com.max;
            float min = com.min;
            float total = com.total;
            this.Dispatcher.BeginInvoke((Action)delegate ()
            {                 //括号里些想要使用的控件属性    

                Max.Text = "当月共计￥" + max;
                Min.Text = "当月共计￥" + min;

                Total.Text = "全年共计￥" + total;
                MaxMonth.Text = maxMonth.ToString();
                MinMonth.Text = minMonth.ToString();

                SeriesCollection series = new SeriesCollection
            {

            new LineSeries
                {
                   
                    //Title = year,
                    Values = new ChartValues<double> { Convert.ToDouble(month.Jan) , Convert.ToDouble(month.Feb) ,Convert.ToDouble(month.Mar) ,Convert.ToDouble(month.Apr) ,Convert.ToDouble(month.May) ,Convert.ToDouble(month.Jun) ,Convert.ToDouble(month.Jul) ,Convert.ToDouble(month.Aug) ,Convert.ToDouble(month.Sep) ,Convert.ToDouble(month.Oct) ,Convert.ToDouble(month.Nov) ,Convert.ToDouble(month.Dec) }
                }

            };

                //x轴
                string[] Labels = new[] { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };
                s1.Series = series;
                s1x.Labels = Labels;
                s1y.LabelFormatter = new Func<double, string>((value) =>
                {
                    return value.ToString();
                });


            });
        }
        private void year_btn_Click(object sender, RoutedEventArgs e)
        {
            string y = year.Text;
            iYear.Text = y;
            progressBar1.IsIndeterminate = true;
            progressBar2.IsIndeterminate = true;
            year_btn.IsEnabled = false;
            UpdateMonth input = new UpdateMonth(y);

            backgroundWorkerUpdateMonth.RunWorkerAsync(input);
            //UpdateMonth(y);
        }

        private void Day_Checked(object sender, RoutedEventArgs e)
        {
            month.Visibility = Visibility;

        }
        private static Month AsyncUpdateMonth(string year, System.ComponentModel.BackgroundWorker backgroundWorker)
        {
            SalesServiceSoapClient ss = new SalesServiceSoapClient();//实例化web服务对象
            Month month = new Month();//实例化对应表的实体（model）对象

            month = ss.SelectSalGrade(year,user_id);
            return month;

        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the input values.
            UpdateMonth input = (UpdateMonth)e.Argument;

            // Start the search for primes and wait.
            Month month = AsyncUpdateMonth(input.Year, backgroundWorkerUpdateMonth);

            UpdateMonth(month);
            if (backgroundWorkerUpdateMonth.CancellationPending)
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
                progressBar1.IsIndeterminate = false;
                progressBar2.IsIndeterminate = false;
                year_btn.IsEnabled = true;

            }

        }

    }
}
