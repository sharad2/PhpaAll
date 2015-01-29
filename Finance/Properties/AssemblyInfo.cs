using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Finance")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Eclipse Systems Pvt. Ltd.")]
[assembly: AssemblyProduct("Finance")]
[assembly: AssemblyCopyright("Copyright © Eclipse Systems Pvt. Ltd. 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("3d5900ae-111a-45be-96b3-d9e4606ca793")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
// Change log from version 6.2.5.0 to 6.2.6.0 (Branched on 23 August 2012)
// provide web config setting to read office name
// Personal: Showing correct bank under Finance tab on home page
// New property added in web.config for Office

//Change log from 6.2.6.0 to 6.2.7.0
// Fixed bug in salary period edit

//Change log from 6.2.7.0 to 6.2.7.1
// Rounded off values in TDS certificate UI

// Change log from 6.2.7.1 to 6.2.7.2
// 1. Fixed issue of mismatch between net Pay of Paybill and that of TDS certificate.

// Change log from 6.2.7.2 to 6.2.7.4 (Branched by Hemant K. Singh on july 2nd 2013)
// Meregd changes of branch version 6.2.7.3
// Change log for version 6.2.7.3
// 1. Fixed time out expired issue in STHC report.
// Changes of this version
// 1. Provided consolidated contarctor report.
// 2. Fund postion report is available.
// 3. Showing Diviosn wise salary break up in paybill.

// Change log from 6.2.7.4 to 6.2.7.5 (Branched by Hemant K. Singh on July 9th 2013)
// 1. Fixed the green tax issue in the receipt and payment report.
// 2. Fixed the green tax issue in the balance sheet report.

// Change log from 6.2.7.5 to 6.2.7.6 (Branched by Binay Bhushan on July 17th 2013)
// 1. Fixed the issue for Grand total at footer doesnot matches with the column total in Paybill.

// Change log from 6.2.7.6 to 6.2.7.7 (Branched by Hemant K. Singh on September 11th 2013)
// 1. Updated the Fund position report for PHPA1.

// Change log from 6.2.7.7 to 6.2.7.8 (Branched by Hemant K. Singh on Sep 21st 2013)
// 1. Updated Fund position report for PHPA1.

// Change log from 6.2.7.8 to 6.3.0.0 (Tagged by Hemant K. Singh on 25th April 2014)
// 1. Upgrade jquery from 1.5.1 to 1.8.2
// 2. Upgrade jquery UI to 1.8.12 to 1.10.0
// 3. Fixed the Internet Explorer compatibility issues.

// Change log from 6.3.0.0 to 6.3.0.1 (Tagged by Hemant K. Singh on May 13th 2014.)
// 1. Now we are showing BOB Thimphu (A/c No.20700220295270028) in the balance fund section of the balance fund position report.

// Change log from 6.3.0.1 to 6.3.0.2 (Tagged by Hemant K. Singh on May 26th 2014.)
// 1. Bug Fixed: We can now update job/employee information for any given voucher in the create voucher form.

//Change log from 6.3.0.2 to 7.0.0.0(Tagged by MBisht 29 Jan 2015)

/* Introducing following new Features :-
 * 1. User can now change his/her password.
 * 2. Health Contribution can be given on the basis % of Gross salary.
 * 3. Assigning MR Number feature is now avalible.
 * 4. Keeping bank name, account number and designation with employee period so that if anything of these get change then old
 * information should display correctly with old data

*/
[assembly: AssemblyVersion("7.0.0.0")]
[assembly: AssemblyFileVersion("7.0.0.0")]