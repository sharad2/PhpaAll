
using System;
using System.Data.Linq;
using Eclipse.PhpaLibrary.Reporting;
using System.Linq;

namespace Eclipse.PhpaLibrary.Database.Payroll
{
    partial class PayrollDataContext
    {
        partial void UpdateEmployee(Employee instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertEmployee(Employee instance)
        {

            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void InsertAdjustment(Adjustment instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateAdjustment(Adjustment instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void UpdateEmployeeAdjustment(EmployeeAdjustment instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertEmployeeAdjustment(EmployeeAdjustment instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateEmployeePeriod(EmployeePeriod instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertEmployeePeriod(EmployeePeriod instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateSalaryPeriod(SalaryPeriod instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertSalaryPeriod(SalaryPeriod instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void DeleteDivision(Division instance)
        {
            throw new NotImplementedException("Use FinanceDataContext to manage divisions");
        }

        partial void InsertDivision(Division instance)
        {
            throw new NotImplementedException("Use FinanceDataContext to manage divisions");
        }

        partial void UpdateDivision(Division instance)
        {
            throw new NotImplementedException("Use FinanceDataContext to manage divisions");
        }

        partial void InsertPeriodEmployeeAdjustment(PeriodEmployeeAdjustment instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void InsertEmployeeType(EmployeeType instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void InsertHeadOfAccount(HeadOfAccount instance)
        {
            throw new NotImplementedException("Use FinanceDataContext to manage head of accounts");
        }

        partial void UpdateEmployeeType(EmployeeType instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateHeadOfAccount(HeadOfAccount instance)
        {
            throw new NotImplementedException("Use FinanceDataContext to manage head of accounts");
        }

        partial void UpdatePeriodEmployeeAdjustment(PeriodEmployeeAdjustment instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

    }

    partial class Adjustment : IAuditable
    {
        /// <summary>
        /// Added by Sharad 22 Jul 2010
        /// </summary>
        public string AdjustmentTypeDescription
        {
            get
            {
                return this.IsDeduction ? "Deduction" : "Earning";
            }

        }
    }

    partial class Employee : IAuditable, IEmployee
    {
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

    }

    partial class ServicePeriod
    {
        public string PayScale
        {
            get
            {
                string str = string.Format("{0:N2}-{1:N2}-{2:N2}", this.MinPayScaleAmount,
                    this.IncrementAmount, this.MaxPayScaleAmount);
                return str;
            }

        }
    }

    partial class EmployeeType : IAuditable
    {

    }

    partial class SalaryPeriod : IAuditable
    {
    }


    partial class EmployeeAdjustment : IAuditable
    {
        public EmployeeAdjustment(Adjustment adj)
        {
            this.AdjustmentId = adj.AdjustmentId;
            this.FlatAmount = adj.FlatAmount;
            this.FractionOfBasic = adj.FractionOfBasic;
            this.IsFlatAmountOverridden = false;
            this.IsFractionBasicOverridden = false;
            this.FractionOfGross = adj.FractionOfGross;
            this.IsFractionGrossOverridden = false;
        }
    }


    partial class EmployeePeriod : IAuditable
    {
        /// <summary>
        /// Creates a new employee period for a specific employee. Default employee adjustments are added
        /// to the created salary employee period
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        public void AddDefaultAdjustments(Employee emp)
        {
            //EmployeePeriod ep = new EmployeePeriod();
            this.BasicPay = emp.BasicSalary;
            this.EmployeeId = emp.EmployeeId;

            decimal grossSalary = emp.BasicSalary ?? 0;

            foreach (EmployeeAdjustment empadj in emp.EmployeeAdjustments.Where(p => !p.Adjustment.IsDeduction))
            {
                // First insert allowances
                PeriodEmployeeAdjustment pea = new PeriodEmployeeAdjustment();
                this.PeriodEmployeeAdjustments.Add(pea);

                pea.AdjustmentId = empadj.AdjustmentId;
                pea.Amount = (empadj.FlatAmount ?? 0) + (Convert.ToDecimal((empadj.FractionOfBasic ?? 0))) * ((empadj.Employee.BasicSalary ?? 0));
                grossSalary += pea.Amount ?? 0;

                pea.Comment = empadj.Comment;
            }

            // Now insert deductions
            foreach (EmployeeAdjustment empadj in emp.EmployeeAdjustments.Where(p => p.Adjustment.IsDeduction))
            {
                PeriodEmployeeAdjustment pea = new PeriodEmployeeAdjustment();
                this.PeriodEmployeeAdjustments.Add(pea);

                pea.AdjustmentId = empadj.AdjustmentId;
                pea.Amount = (empadj.FlatAmount ?? 0) + (Convert.ToDecimal((empadj.FractionOfBasic ?? 0))) * ((empadj.Employee.BasicSalary ?? 0)) +
                    (Convert.ToDecimal((empadj.FractionOfGross ?? 0))) * grossSalary;

                pea.Comment = empadj.Comment;
            }
            return;
        }
    }

    partial class PeriodEmployeeAdjustment : IAuditable
    {
        public decimal? AmountRounded
        {
            get
            {
                return Math.Round(this.Amount ?? 0, 0, MidpointRounding.AwayFromZero);
            }
        }
    }

    partial class AdjustmentCategory
    {
        public string CategoryTypeDescription
        {
            get
            {
                return this.IsDeduction ? "Deduction" : "Allowance";
            }

        }
    }

}
