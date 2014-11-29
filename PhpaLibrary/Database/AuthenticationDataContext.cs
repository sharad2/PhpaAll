using System.Data.Linq;
using System.Data.SqlClient;

namespace Eclipse.PhpaLibrary.Database
{
    public partial class AuthenticationDataContext
    {
        partial void UpdatePhpaUser(PhpaUser instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertPhpaUser(PhpaUser instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        public override void SubmitChanges(ConflictMode failureMode)
        {
            try
            {
                base.SubmitChanges(failureMode);
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 2627:
                        // Unique key violation. Assume that user is being inserted/updated
                        throw new ValidationException("User with the specified Login Id already exists. Cannot create duplicate user. Choose another name for Login Id.");

                    default:
                        throw;
                }

            }
        }
    }

    public partial class PhpaUser : IAuditable
    {
    }
}
