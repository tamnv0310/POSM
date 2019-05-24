using System;
using System.Collections.Generic;
using System.Text;

namespace POSM.Core.Constants
{
    public static class CommonConstant
    {
        #region Default
        public const string TOKENSCHEMA = "bearer";
        public const string USERID = "userId";
        public const string SUCCESS = "success.";
        public const int ACCESSTOKENMINUTEEXPIRED = 30;
        public const int REFRESHTOKENDAYEXPIRED = 30;
        #endregion

        #region Message text
        public const string USERHASBEENDELETED = "Your account has been deleted. Please contact Facilities Management for assistance.";
        public const string REFRESHTOKENISINVALID = "Your session has expired. Please login again.";
        #endregion
    }
}
