using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Security;
using Eclipse.PhpaLibrary.Database;
using System.Web.SessionState;

namespace Eclipse.PhpaLibrary.Web.Providers
{
    public class PhpaMembershipProvider : MembershipProvider
    {
        public PhpaMembershipProvider()
        {

        }

        private string _applicationName;
        public override string ApplicationName
        {
            get
            {
                return _applicationName;
            }
            set
            {
                _applicationName = value;
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            using (AuthenticationDataContext ctx = new AuthenticationDataContext(_connectString))
            {
                var query = (from user in ctx.PhpaUsers
                             where user.UserName == username &&
                             user.Password == oldPassword
                             select user).Single();
                query.Password = newPassword;
                ctx.SubmitChanges();
                return true;
            }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException("ChangePasswordQuestionAndAnswer");
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException("CreateUser");
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException("DeleteUser");
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException("EnablePasswordReset"); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException("EnablePasswordRetrieval"); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException("FindUsersByEmail");
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException("FindUsersByName");
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException("GetAllUsers");
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException("GetNumberOfUsersOnline");
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException("GetPassword");
        }

        /// <summary>
        /// Implemented this for Change password feature
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            using (AuthenticationDataContext ctx = new AuthenticationDataContext(_connectString))
            {
                var query = (from user in ctx.PhpaUsers
                             where user.UserName == username
                             select new MembershipUser(this.Name, username, user.UserId, null, null, user.AdminComment, true, false,
                                 user.Created ?? DateTime.Today, DateTime.Today, DateTime.Today, DateTime.Today, DateTime.Today)).SingleOrDefault();
                return query;
            }


            throw new NotImplementedException("GetUser");
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException("GetUser2");
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException("GetUserNameByEmail");
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException("MaxInvalidPasswordAttempts"); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException("MinRequiredNonAlphanumericCharacters"); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException("PasswordAttemptWindow"); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException("PasswordFormat"); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException("PasswordStrengthRegularExpression"); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException("RequiresQuestionAndAnswer"); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException("RequiresUniqueEmail"); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException("ResetPassword");
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException("UnlockUser");
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException("UpdateUser");
        }

        private string _connectString;
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            string str = config["connectionStringName"];
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("connectionStringName must be provided");
            }
            else
            {
                _connectString = ConfigurationManager.ConnectionStrings[str].ConnectionString;
            }

            str = config["applicationName"];
            if (string.IsNullOrEmpty(str))
            {
                _applicationName = "default";
            }
            else
            {
                _applicationName = str;
            }
            base.Initialize(name, config);
        }

        public override bool ValidateUser(string username, string password)
        {
            using (AuthenticationDataContext ctx = new AuthenticationDataContext(_connectString))
            {
                try
                {
                    var query = (from user in ctx.PhpaUsers
                                 where user.UserName == username &&
                                 user.Password == password
                                 select user).Single();
                    return true;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }

        }
    }
}
