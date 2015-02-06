using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Security;
using Eclipse.PhpaLibrary.Database;

namespace Eclipse.PhpaLibrary.Web.Providers
{
    public class Duty
    {
        /// <summary>
        /// Duties having a higher seniority include all duties of lower seniority.
        /// Seniority 0 means that this duty is presumed if no other duty has been specified.
        /// </summary>
        public int Seniority { get; set; }

        /// <summary>
        /// True if the duty can be assigned on a per module basis
        /// </summary>
        public bool ModuleSpecific { get; set; }

        /// <summary>
        /// Descriptive name of the duty
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Each user can have any number of roles. Some roles are predefined by the enum above. Executive is the
    /// least powerful role. The roles increase in power as they move up with Administrator being the most
    /// powerful role. All other roles are passed through without any interpretation.
    /// </summary>
    public class PhpaRoleProvider : RoleProvider
    {
        private readonly string[] _moduleNames;
        public string[] ModuleNames
        {
            get
            {
                return _moduleNames;
            }
        }

        private readonly List<Duty> _duties;
        public IEnumerable<Duty> Duties
        {
            get
            {
                return _duties;
            }
        }

        public PhpaRoleProvider()
        {
            _moduleNames = new string[] {"Bills", "Finance", "Payroll", "Stores", "Personnel", "MC-1", "MC-2", "MC-3", "Infra Works" };
            _duties = new List<Duty>();
            _duties.Add(new Duty { Name = "Administrator", Seniority = 100, ModuleSpecific = false });
            _duties.Add(new Duty { Name = "Manager", Seniority = 90, ModuleSpecific = true });
            _duties.Add(new Duty { Name = "Operator", Seniority = 80, ModuleSpecific = true });
            _duties.Add(new Duty { Name = "Executive", Seniority = 70, ModuleSpecific = true });
            _duties.Add(new Duty { Name = "Visitor", Seniority = 0, ModuleSpecific = true });
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
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

        public override void CreateRole(string roleName)
        {
            throw new NotSupportedException("Roles cannot be created or modified");
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotSupportedException("Roles cannot be created or modified");
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns Administrator, Operator, etc. Does not return roles concatenated with module name.
        /// </summary>
        /// <returns></returns>
        public override string[] GetAllRoles()
        {
            // Return roles for Administrator
            return DoCreateRoles(_duties.Aggregate((p, q) => p.Seniority > q.Seniority ? p : q),
                _moduleNames);
        }

        private string[] DoCreateRoles(Duty dutyPassed, IEnumerable<string> modules)
        {
            HashSet<string> list = new HashSet<string>();
            foreach (Duty duty in _duties.Where(p => p.Seniority <= dutyPassed.Seniority))
            {
                list.Add(duty.Name);
                if (duty.ModuleSpecific)
                {
                    // For safety, ignoring empty module names
                    foreach (string module in modules.Where(p => !string.IsNullOrEmpty(p)))
                    {
                        list.Add(module);
                        list.Add(module + duty.Name);
                    }
                }
            }
            return list.ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] roles;
            using (AuthenticationDataContext db = new AuthenticationDataContext(_connectString))
            {
                var roleInfo = (from user in db.PhpaUsers
                                where user.UserName == username
                                select new
                                {
                                    DutyName = user.Roles.Trim(),
                                    Modules = user.Modules ?? ""
                                }).SingleOrDefault();
                if (roleInfo == null)
                {
                    roles = new string[0];
                }
                else
                {
                    Duty duty = _duties.FirstOrDefault(p => p.Name == roleInfo.DutyName);
                    string[] modules = roleInfo.Modules.Split(',');
                    if (duty == null)
                    {
                        duty = _duties.Single(p => p.Seniority == 0);
                    }
                    else if (duty.Seniority == _duties.Max(p => p.Seniority))
                    {
                        // For administrators, use all modules
                        modules = _moduleNames;
                    }
                    roles = DoCreateRoles(duty, modules);
                }
            }
            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
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

        #region Specialized Public Functions

        /// <summary>
        /// Returns the modules user is authorized for. Van return null.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string[] GetModulesForUser(string userName)
        {
            using (AuthenticationDataContext db = new AuthenticationDataContext(_connectString))
            {
                var userInfo = (from user in db.PhpaUsers
                                where user.UserName == userName
                                select user
                                  ).SingleOrDefault();
                if (userInfo == null)
                {
                    // Garrbage user
                    return null;
                }
                Duty duty = _duties.FirstOrDefault(p => p.Name == userInfo.Roles);
                if (duty == null)
                {
                    // Garbage role assigned to user. Same as garbage user
                    return null;
                }
                if (duty.Seniority == _duties.Max(p => p.Seniority))
                {
                    // For administrators, all modules
                    return _moduleNames.ToArray();      // Cloning the array
                }
                if (string.IsNullOrEmpty(userInfo.Modules))
                {
                    return null;
                }
                return userInfo.Modules.Split(',');

            }
        }
        #endregion

    }
}
