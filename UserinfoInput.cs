using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpftest
{
    public class UserinfoInput
    {
            public string User_id
            { get; set; }

            public string User_pwd
            { get; set; }

            public int IDtype
            { get; set; }
            public UserinfoInput(string user_id, string user_pwd, int idtype)
            {
                User_id = user_id;
                User_pwd = user_pwd;
                IDtype = idtype;
            }
    }
    public class UserResinfoInput
    {
        public string User_id
        { get; set; }

        public string User_pwd
        { get; set; }

        public string Ip_addr
        { get; set; }

        public UserResinfoInput(string user_id, string user_pwd, string ip_addr)
        {
            User_id = user_id;
            User_pwd = user_pwd;
            Ip_addr = ip_addr;
        }
    }
    public class UpdateMonth
    {
        public string Year
        { get; set; }
        public UpdateMonth(string year)
        {
            Year = year;
        }
    }


}
