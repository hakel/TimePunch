using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePunch
{
    //TODO - replace all hard coded sql with this class, look for table TimePunchEvents
    class Punch
    {
        public string UserIdentity { get => userIdentity; set => userIdentity = value; }
        public string AdminIdentity { get => adminIdentity; set => adminIdentity = value; }
        public int ManualPunch { get => manualPunch; set => manualPunch = value; }
        public int SigninUnixTime { get => signinUnixTime; set => signinUnixTime = value; }
        public int SignoutUnixTime { get => signoutUnixTime; set => signoutUnixTime = value; }
        public int SigninYear { get => signinYear; set => signinYear = value; }
        public int SigninMonth { get => signinMonth; set => signinMonth = value; }
        public int SigninDay { get => signinDay; set => signinDay = value; }
        public int SignoutYear { get => signoutYear; set => signoutYear = value; }
        public int SignoutMonth { get => signoutMonth; set => signoutMonth = value; }
        public int SignoutDay { get => signoutDay; set => signoutDay = value; }
        public string SigninType { get => signinType; set => signinType = value; }
        public string SigninComputer { get => signinComputer; set => signinComputer = value; }
        public string SignoutComputer { get => signoutComputer; set => signoutComputer = value; }
        public int CreateUnixTimeStamp { get => createUnixTimeStamp; set => createUnixTimeStamp = value; }
        public int UpdateUnixTimeStamp { get => updateUnixTimeStamp; set => updateUnixTimeStamp = value; }

        //    " userIdentity varchar(20), " +
        string userIdentity = "";
        //    " adminIdentity varchar(20), " +
        string adminIdentity = "";
        //    " manualPunch int, " +
        int manualPunch = 0;
        //    " signinUnixTime int, " +
        int signinUnixTime = 0;
        //    " signoutUnixTime int, " +
        int signoutUnixTime = 0;
        //    " signinYear int, " +
        int signinYear = 0;
        //    " signinMonth int, " +
        int signinMonth = 0;
        //    " signinDay int, " +
        int signinDay = 0;
        //    " signoutYear int, " +
        int signoutYear = 0;
        //    " signoutMonth int, " +
        int signoutMonth = 0;
        //    " signoutDay int, " +
        int signoutDay = 0;
        //    " signinType varchar(20), " +
        string signinType = "";
        //    " signinComputer varchar(20), " +
        string signinComputer = "";
        //    " signoutComputer varchar(20), " +
        string signoutComputer = "";
        //    " createUnixTimeStamp int, " +
        int createUnixTimeStamp = 0;
        //    " updateUnixTimeStamp int " +
        int updateUnixTimeStamp = 0;


    }
}
