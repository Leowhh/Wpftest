using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpftest.SalesService;

namespace Wpftest
{
    class compare
    {
        public float[] Amonth = new float[12];
        public int maxMonth;
        public int minMonth;
        public float max;
        public float min;
        public float total;
        public void setYear(Month month)
        {
            Amonth[0] = Convert.ToSingle(month.Jan);
            Amonth[1] = Convert.ToSingle(month.Feb);
            Amonth[2] = Convert.ToSingle(month.Mar);
            Amonth[3] = Convert.ToSingle(month.Apr);
            Amonth[4] = Convert.ToSingle(month.May);
            Amonth[5] = Convert.ToSingle(month.Jun);
            Amonth[6] = Convert.ToSingle(month.Jul);
            Amonth[7] = Convert.ToSingle(month.Aug);
            Amonth[8] = Convert.ToSingle(month.Sep);
            Amonth[9] = Convert.ToSingle(month.Oct);
            Amonth[10] = Convert.ToSingle(month.Nov);
            Amonth[11] = Convert.ToSingle(month.Dec);
            int i;
            max = 0;   
            for(i=0;i<12;i++)
            {               
                if (max < Amonth[i])
                { 
                 max = Amonth[i]; 
                 maxMonth = i + 1;
                }              
            }
            min = max;
            for(i=0;i<12;i++)
            {
                if (Amonth[i] == 0);

                else if (min > Amonth[i])
                {
                    min = Amonth[i];
                    minMonth = i + 1;
                }
            }
            total =
              Convert.ToSingle(month.Jan)
              + Convert.ToSingle(month.Feb)
              + Convert.ToSingle(month.Mar)
              + Convert.ToSingle(month.Apr)
              + Convert.ToSingle(month.May)
              + Convert.ToSingle(month.Jun)
              + Convert.ToSingle(month.Jul)
              + Convert.ToSingle(month.Aug)
              + Convert.ToSingle(month.Sep)
              + Convert.ToSingle(month.Oct)
              + Convert.ToSingle(month.Nov)
              + Convert.ToSingle(month.Dec);

        }
   

    }
}