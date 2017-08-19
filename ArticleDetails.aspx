<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArticleDetails.aspx.cs" Inherits="ArticleDetails" MasterPageFile="~/DetailsContent.master" %>

<%@ Register Src="~/Controls/MenuContentControl.ascx" TagPrefix="uc1" TagName="MenuContentControl" %>
<%@ Register Src="~/Controls/MenuArticleControl.ascx" TagPrefix="uc1" TagName="MenuArticleControl" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphMetaTag2">
    <asp:Literal runat="server" ID="ltrMetaTags"></asp:Literal>
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <nav class="breadcrumbs hide-on-mobile">
        <ul>
            <li><a href="<%= Globals.BaseUrl %>">Trang chủ</a></li>
            
                <asp:Literal runat="server" ID="ltrCrumbs"></asp:Literal>
        </ul>
    </nav>
    <div class="clear">
    </div>
    <div id="page-news" class="units-row">
        <div class="unit-70">
            <div class="article-details">
                <h1>
                    <asp:Literal runat="server" ID="ltrTitle" /></h1>
                <div class="clear">
                </div>
                <h2>
                    <asp:Literal runat="server" ID="ltrDescription" /></h2>
                <div class="clear">
                </div>
                <asp:Literal runat="server" ID="ltrRelated" />
                <div class="clear">
                </div>
                <asp:Literal runat="server" ID="ltrContent" />
                <div class="clear">
                </div>

                <asp:Panel runat="server" ID="pnlTag" Visible="false">
                    <div class="tagLeft">
                    </div>
                    <div class="tagMid">
                        <div class="tag">
                        </div>
                        <div class="wordTag">
                            <asp:Literal runat="server" ID="ltrTag" />
                        </div>
                    </div>
                    <div class="tagRight">
                    </div>
                </asp:Panel>

            </div>

                <asp:Panel CssClass="module" runat="server" ID="pnlNewArticle">
        <div class="title">
            <h3>
                <asp:Literal runat="server" ID="ltrHeadInCategory" Text="Cùng chuyên mục" /></h3>
        </div>
        <div class="clear">
        </div>
        <asp:Repeater runat="server" ID="rptInCategory" OnItemDataBound="rptInCategory_ItemDataBound">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                    <asp:HyperLink runat="server" ID="hplTitle" /></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>
    <div class="clear">
    </div>
    <asp:Panel CssClass="module" runat="server" ID="pnlOldRelated">
        <div class="title" style="border-top: none">
            <h3>
                <asp:Literal runat="server" ID="ltrArticleOld" Text="" /></h3>
        </div>
        <div class="clear">
        </div>
        <asp:Repeater runat="server" ID="rptArticleOld" OnItemDataBound="rptInCategory_ItemDataBound">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                    <asp:HyperLink runat="server" ID="hplTitle" /></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>

        </div>

        <div class="unit-30">
            <uc1:MenuArticleControl runat="server" ID="MenuArticleControl" />
            <uc1:MenuContentControl runat="server" ID="MenuContentControl" />
        </div>
    </div>
</asp:Content>
