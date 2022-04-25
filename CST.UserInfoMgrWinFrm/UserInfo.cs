using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST.UserInfoMgrWinFrm
{
    class UserInfo
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }

        public int UserAge { get; set; }

        public string UserMobile { get; set; }

        public string UserEmail { get; set; }

        public DateTime  CreateDate{ get; set; }

    }
}
