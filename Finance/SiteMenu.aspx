<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="SiteMenu.aspx.cs"
    Inherits="PhpaAll.SiteMenu" Title="Site Map" EnableViewState="false" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <asp:SiteMapDataSource ID="smds" runat="server" StartFromCurrentNode="false"
        ShowStartingNode="false" StartingNodeUrl="~/MIS/Default.aspx" />
    <asp:Menu ID="menu" runat="server" DataSourceID="smds" StaticDisplayLevels="5"
        DynamicHorizontalOffset="2" StaticSubMenuIndent="10px" CssClass="ui-widget" OnMenuItemDataBound="menuStore_MenuItemDataBound">
        <StaticSelectedStyle CssClass="ui-selected" />
        <LevelMenuItemStyles>
            <asp:MenuItemStyle VerticalPadding="2mm" CssClass="ui-priority-primary" />
            <asp:MenuItemStyle HorizontalPadding="2em" VerticalPadding="0.5mm"  />
            <asp:MenuItemStyle HorizontalPadding="2em" VerticalPadding="0.5mm" Font-Size="Larger" />
        </LevelMenuItemStyles>
        <DataBindings>
            <asp:MenuItemBinding DataMember="SiteMapNode" NavigateUrlField="Url" TextField="Title"
                ValueField="Description" />
        </DataBindings>
        <StaticHoverStyle CssClass="ui-selecting" />
        <StaticItemTemplate>
            <%# Eval("Text") %>.<span >
            <%# Eval("Value") %></span>
        </StaticItemTemplate>
        
    </asp:Menu>
</asp:Content>
