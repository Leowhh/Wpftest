using LiveCharts;
using LiveCharts.Wpf;
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
using Wpftest.SalesService;

namespace Wpftest
{
    /// <summary>
    /// Dashboard.xaml 的交互逻辑
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            
            Month.IsChecked = true;
            
        }
        private void Year_Checked(object sender, RoutedEventArgs e)
        {
            UpdateYear();
            
            
        }


        private void Month_Checked(object sender, RoutedEventArgs e)
        {
            string y = year.Text;
            UpdateMonth(y);
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
        public void UpdateMonth(string year)
        {
            if (year == "")
            {
                //x轴
                string[] Labels = new[] { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };
                s1x.Labels = Labels;
            }
            else
            {
                SalesServiceSoapClient ss = new SalesServiceSoapClient();//实例化web服务对象
                Month month = new Month();//实例化对应表的实体（model）对象
                month = ss.SelectGrade(year);

                SeriesCollection series = new SeriesCollection
                {

                new LineSeries
                    {
                        //Title = "未分组",
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
            }
        }
        private void year_btn_Click(object sender, RoutedEventArgs e)
        {
            string y = year.Text;
            UpdateMonth(y);
        }

        private void Day_Checked(object sender, RoutedEventArgs e)
        {
            month.Visibility = Visibility;
            
        }
    }
}
