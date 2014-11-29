using System;
namespace Eclipse.PhpaLibrary.Reporting
{
    /// <summary>
    /// This is a minimalist interface representing employee information. It is implemented by Employee class
    /// as well as RoEmployee class. This allows EmployeeSelector to work with the employee, regardless of which
    /// database is used to access it.
    /// </summary>
    public interface IEmployee
    {
        string EmployeeCode { get; }
        int EmployeeId { get; }
        string FullName { get; }
    }
}
