<%@ Page Title="PIS Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="PISDefault.doc.aspx.cs" Inherits="PhpaAll.Doc.PISDefault" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
<br />
<p>
        Welcome to PIS, this section give all the possible options to manage all the employees.
        The highlights of the features provided to as follows -
    </p>
    <dl>
        <dt style="font-weight: bold">The Home Page</dt>
        <dd>
            <ol>
                <li>This page gives an overall view of the <strong>Personnel Information System</strong>.
                    You may see the employees of a particular category by clicking on the links available
                    in the table showing the employee strength categorically. </li>
                <li>It basically alerts you about various upcoming events like increment due, promotion
                    due and lists major events happened in the past.</li>
                <li>In the <strong>'Staff Strength By Division and Type'</strong> report, the
                    <asp:Label ID="Label2" runat="server" CssClass="ui-state-active" Width="5em" Height="1em" />
                    represents Foreigners and
                    <asp:Label ID="Label3" runat="server" CssClass="ui-state-highlight" Width="5em" Height="1em" />
                    represents Bhutanese</li>
            </ol>
        </dd>
        <dt style="font-weight: bold">Set up the environment</dt>
        <dd>
            In order to set up the environment to make PIS functional, you need to specify some
            fixed set of information. You can do this from the given links under <strong>Masters</strong>
            menu. You can enter some fixed set of information's like "Leave Type","Training Types",
            "Countries", "Promotion Types", etc.
        </dd>
        <dt style="font-weight: bold">New Recruitment</dt>
        <dd>
            To enter details of the new employee you need to navigate to the <a href="../PIS/Employees.aspx">
                Employee's Page</a> by clicking on the "Employees" menu. And from there you
            can enter all his details.</dd>
        <dt style="font-weight: bold">Search any existing employee</dt>
        <dd>
            To straight a way look up or view any specific employee's details, enter full name
            or first few alphabets of the employee's name in the search textbox given in the
            side navigation panel and click on search button. This will take you to the employee
            page and from there you can view his details like joining dates, employee type,
            designations, etc.</dd>
        <dt style="font-weight: bold">Promoting, incrementing or updating other details of any
            employee.</dt>
        <dd>
            Follow the following steps to update any employee's details like, employee type,
            salary, designations, family-details ,etc. -
            <ol>
                <li>Search the employee in the <a href="../PIS/Employees.aspx">Employee's Page</a>.( Use the
                    filter section given on the page to fine-tune your search.) </li>
                <li>Click on the employee's name appearing as a link, this will take you to the employee-details
                    page.</li>
                <li>From Employee-Details page you can update any employee's salary, designation, etc.</li>
            </ol>
        </dd>
        <dt style="font-weight: bold">Viewing Personal Information of all or specific employees</dt>
        <dd>
            Lists the employee with their personal details on the basis of following -
            <ol>
                <li>Employee Type</li>
                <li>Termination Status</li>
                <li>Grade</li>
                <li>Division</li>
            </ol>
        </dd>
    </dl>
    <h4>
        There are several reports related to PIS, they are explained here in below -</h4>
    <dl>
        <dt>Staff Strength:</dt>
        <dd>
            This report shows only active employees category-wise ("Contract","Deputation","Not
            Set","Regular","Secondment", "Work Charged", "etc").</dd>
        <dt>Personnel Information:</dt>
        <dd>
            Lists employees with their details.</dd>
        <dt>Termination Status:</dt>
        <dd>
            This report lists those employees who have been terminated or resigned.</dd>
        <dt>Promotion:</dt>
        <dd>
            Displays list of employees being promoted and their service since last promotion
            or appointment</dd>
        <dt>Trainings:</dt>
        <dd>
            This report list the employees who have been nominated for training in the past.</dd>
        <dt>Leaves:</dt>
        <dd>
            Lists the leave details of all the employees for all or specified "Leave Type" and
            "Period Range".
        </dd>
        <dt>Medical Allowance:</dt>
        <dd>
            Lists details of medical allowances granted for any or specified period range and
            hospitals.</dd>
        <dt>Staff Division:</dt>
        <dd>
            It shows the list of employees per "Division", "Sub-DIvision" and Office.</dd>
        <dt>Probation Completion:</dt>
        <dd>
            It show the list of employees who have completed the probation during the specified
            range of dates.</dd>
        <dt>Contract Completion:</dt>
        <dd>
            Shows list of employees who have completed the specified contract period belonging
            to all or specific employee-type.</dd>
        <dt>Next Increment Due:</dt>
        <dd>
            Lists the employees whose increment is due in the specified range of dates.
        </dd>
        <dt>Next Promotion Due:</dt>
        <dd>
            Lists the employees whose promotion is due in the specified range of dates.
        </dd>
        <dt>ACR</dt>
        <dd>
            It's report which shows the ACR status (Pending or completed) of employee for current
            year.</dd>
    </dl>
</asp:Content>
