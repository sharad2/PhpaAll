﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PhpaAll.Bills
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="PHPA2151114")]
	public partial class PhpaBillsDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertBill(Bill instance);
    partial void UpdateBill(Bill instance);
    partial void DeleteBill(Bill instance);
    #endregion
		
		public PhpaBillsDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["PHPA2151114ConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public PhpaBillsDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PhpaBillsDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PhpaBillsDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PhpaBillsDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Bill> Bills
		{
			get
			{
				return this.GetTable<Bill>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Bill")]
	public partial class Bill : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _BillNumber;
		
		private System.Nullable<System.DateTime> _BillDate;
		
		private System.Nullable<System.DateTime> _DueDate;
		
		private System.Nullable<System.DateTime> _SubmittedToDivision;
		
		private System.Nullable<System.DateTime> _PaidOn;
		
		private System.Nullable<int> _DivisionId;
		
		private System.Nullable<int> _ContractorId;
		
		private System.Nullable<decimal> _Amount;
		
		private string _ApprovedBy;
		
		private System.Nullable<System.DateTime> _ApprovedOn;
		
		private string _Remarks;
		
		private System.Data.Linq.Binary _BillImage;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnBillNumberChanging(string value);
    partial void OnBillNumberChanged();
    partial void OnBillDateChanging(System.Nullable<System.DateTime> value);
    partial void OnBillDateChanged();
    partial void OnDueDateChanging(System.Nullable<System.DateTime> value);
    partial void OnDueDateChanged();
    partial void OnSubmittedOnDateChanging(System.Nullable<System.DateTime> value);
    partial void OnSubmittedOnDateChanged();
    partial void OnPaidDateChanging(System.Nullable<System.DateTime> value);
    partial void OnPaidDateChanged();
    partial void OnSubmitedToDivisionIdChanging(System.Nullable<int> value);
    partial void OnSubmitedToDivisionIdChanged();
    partial void OnContractorIdChanging(System.Nullable<int> value);
    partial void OnContractorIdChanged();
    partial void OnAmountChanging(System.Nullable<decimal> value);
    partial void OnAmountChanged();
    partial void OnApprovedByChanging(string value);
    partial void OnApprovedByChanged();
    partial void OnApprovedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnApprovedOnChanged();
    partial void OnRemarksChanging(string value);
    partial void OnRemarksChanged();
    partial void OnBillImageChanging(System.Data.Linq.Binary value);
    partial void OnBillImageChanged();
    #endregion
		
		public Bill()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BillNumber", DbType="NVarChar(20)")]
		public string BillNumber
		{
			get
			{
				return this._BillNumber;
			}
			set
			{
				if ((this._BillNumber != value))
				{
					this.OnBillNumberChanging(value);
					this.SendPropertyChanging();
					this._BillNumber = value;
					this.SendPropertyChanged("BillNumber");
					this.OnBillNumberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BillDate", DbType="SmallDateTime")]
		public System.Nullable<System.DateTime> BillDate
		{
			get
			{
				return this._BillDate;
			}
			set
			{
				if ((this._BillDate != value))
				{
					this.OnBillDateChanging(value);
					this.SendPropertyChanging();
					this._BillDate = value;
					this.SendPropertyChanged("BillDate");
					this.OnBillDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DueDate", DbType="SmallDateTime")]
		public System.Nullable<System.DateTime> DueDate
		{
			get
			{
				return this._DueDate;
			}
			set
			{
				if ((this._DueDate != value))
				{
					this.OnDueDateChanging(value);
					this.SendPropertyChanging();
					this._DueDate = value;
					this.SendPropertyChanged("DueDate");
					this.OnDueDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="SubmittedToDivision", Storage="_SubmittedToDivision", DbType="SmallDateTime")]
		public System.Nullable<System.DateTime> SubmittedOnDate
		{
			get
			{
				return this._SubmittedToDivision;
			}
			set
			{
				if ((this._SubmittedToDivision != value))
				{
					this.OnSubmittedOnDateChanging(value);
					this.SendPropertyChanging();
					this._SubmittedToDivision = value;
					this.SendPropertyChanged("SubmittedOnDate");
					this.OnSubmittedOnDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="PaidOn", Storage="_PaidOn", DbType="SmallDateTime")]
		public System.Nullable<System.DateTime> PaidDate
		{
			get
			{
				return this._PaidOn;
			}
			set
			{
				if ((this._PaidOn != value))
				{
					this.OnPaidDateChanging(value);
					this.SendPropertyChanging();
					this._PaidOn = value;
					this.SendPropertyChanged("PaidDate");
					this.OnPaidDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="DivisionId", Storage="_DivisionId", DbType="Int")]
		public System.Nullable<int> SubmitedToDivisionId
		{
			get
			{
				return this._DivisionId;
			}
			set
			{
				if ((this._DivisionId != value))
				{
					this.OnSubmitedToDivisionIdChanging(value);
					this.SendPropertyChanging();
					this._DivisionId = value;
					this.SendPropertyChanged("SubmitedToDivisionId");
					this.OnSubmitedToDivisionIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ContractorId", DbType="Int")]
		public System.Nullable<int> ContractorId
		{
			get
			{
				return this._ContractorId;
			}
			set
			{
				if ((this._ContractorId != value))
				{
					this.OnContractorIdChanging(value);
					this.SendPropertyChanging();
					this._ContractorId = value;
					this.SendPropertyChanged("ContractorId");
					this.OnContractorIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Amount", DbType="Money")]
		public System.Nullable<decimal> Amount
		{
			get
			{
				return this._Amount;
			}
			set
			{
				if ((this._Amount != value))
				{
					this.OnAmountChanging(value);
					this.SendPropertyChanging();
					this._Amount = value;
					this.SendPropertyChanged("Amount");
					this.OnAmountChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ApprovedBy", DbType="NVarChar(50)")]
		public string ApprovedBy
		{
			get
			{
				return this._ApprovedBy;
			}
			set
			{
				if ((this._ApprovedBy != value))
				{
					this.OnApprovedByChanging(value);
					this.SendPropertyChanging();
					this._ApprovedBy = value;
					this.SendPropertyChanged("ApprovedBy");
					this.OnApprovedByChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ApprovedOn", DbType="SmallDateTime")]
		public System.Nullable<System.DateTime> ApprovedOn
		{
			get
			{
				return this._ApprovedOn;
			}
			set
			{
				if ((this._ApprovedOn != value))
				{
					this.OnApprovedOnChanging(value);
					this.SendPropertyChanging();
					this._ApprovedOn = value;
					this.SendPropertyChanged("ApprovedOn");
					this.OnApprovedOnChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Remarks", DbType="NVarChar(255)")]
		public string Remarks
		{
			get
			{
				return this._Remarks;
			}
			set
			{
				if ((this._Remarks != value))
				{
					this.OnRemarksChanging(value);
					this.SendPropertyChanging();
					this._Remarks = value;
					this.SendPropertyChanged("Remarks");
					this.OnRemarksChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BillImage", DbType="VarBinary(MAX)", UpdateCheck=UpdateCheck.Never)]
		public System.Data.Linq.Binary BillImage
		{
			get
			{
				return this._BillImage;
			}
			set
			{
				if ((this._BillImage != value))
				{
					this.OnBillImageChanging(value);
					this.SendPropertyChanging();
					this._BillImage = value;
					this.SendPropertyChanged("BillImage");
					this.OnBillImageChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
