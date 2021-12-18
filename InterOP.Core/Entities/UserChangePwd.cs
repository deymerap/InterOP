using System;
using System.Collections.Generic;
using System.Text;

namespace InterOP.Core.Entities
{
    public class UserChangePwd
    {
        public string NitProvider { get; set; }
        public string PwdOld { get; set; }
        public string PwdNew { get; set; }
    }
}
