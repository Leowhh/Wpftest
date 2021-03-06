﻿using System;
using System.Collections.Generic;
using System.Data;
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
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class EditSal : Page
    {
        public static string sal_no;
        public EditSal()
        {
            InitializeComponent();

            SalesServiceSoapClient ss = new SalesServiceSoapClient();//实例化web服务对象
            DataTable dt = ss.GetSalesInfo(sal_no, "","","","");
            dt.Columns[0].ColumnName = "代号";
            dt.Columns[1].ColumnName = "姓名";
            dt.Columns[2].ColumnName = "组";
            dt.Columns[3].ColumnName = "性别";
            dt.Columns[4].ColumnName = "年龄";
            dt.Columns[5].ColumnName = "联系方式";
            data.DataContext = dt.DefaultView;

        }

        private void ok_btn_Click(object sender, RoutedEventArgs e)
        {
            SalesServiceSoapClient ss = new SalesServiceSoapClient();
            DataTable dt = new DataTable();
            dt = ((DataView)data.ItemsSource).Table;
            dt.Columns[0].ColumnName = "sal_no";
            dt.Columns[1].ColumnName = "sal_name";
            dt.Columns[2].ColumnName = "sal_class";
            dt.Columns[3].ColumnName = "sal_sex";
            dt.Columns[4].ColumnName = "sal_age";
            dt.Columns[5].ColumnName = "sal_number";
            string message = ss.UpdataSalesInfo(dt);
            MessageBox.Show(message);
            dt = ss.GetSalesInfo(sal_no, "", "", "", "");
            dt.Columns[0].ColumnName = "代号";
            dt.Columns[1].ColumnName = "姓名";
            dt.Columns[2].ColumnName = "组";
            dt.Columns[3].ColumnName = "性别";
            dt.Columns[4].ColumnName = "年龄";
            dt.Columns[5].ColumnName = "联系方式";
            data.DataContext = dt.DefaultView;
        }
    }
}
