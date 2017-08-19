using System;
using System.Collections.Generic;

using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ebis.Utilities;
using TuanSon.BLL;
using TuanSon.EntitiesClass;


public partial class ArticleDetails : System.Web.UI.Page
{
    int _categoryId = 0;
    public int CategoryID
    {
        get { return _categoryId; }
        set { _categoryId = value; }
    }

    int _articleId = 0;
    public int ArticleID
    {
        get { return _articleId; }
        set { _articleId = value; }
    }

    int ParentCategoryID = 0;

    int SetQuantityArticleRelated = 10;

    bool LaHuongDang = false;

    Article article = new Article();
    string aurl = string.Empty;
    string quan = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        ProccessParameter();
        if (!IsPostBack)
        {
            BindData();
            SetTitle();
            BindDataToNavigate();
            BindDataInCategory();
        }
    }

    protected void ProccessParameter()
    {
        //ArticleID = RequestHelper.GetInt("aid", 0);
        aurl = RequestHelper.GetString("aurl", string.Empty);
    }

    protected void BindData()
    {
        article = ArticleBL.GetArticleByFriendlyUrl(aurl);
        //article = ArticleBL.GetArticleByID(ArticleID);
        if (article != null)
        {
            //int viewed = 0;
            //ArticleBL.GetAndUpdateViewedArticle(ArticleID, out viewed);
            //ltrViewed.Text = ConvertUtility.ToString(viewed);

            //ltrDate.Text = "Cập nhật " + Globals.LayNgayDang(article.CreatedDate);


            ltrTitle.Text = article.Title;
            ltrContent.Text = article.DetailContent;
            ltrDescription.Text = article.Description;
            CategoryID = article.CategoryID;

            if (article.Description_Lang2.Length > 0)
            {
                pnlTag.Visible = true;
                ltrTag.Text = article.Description_Lang2;
            }

            ltrRelated.Text = article.DetailContent_Lang2;
        }
    }


    #region SetTitle
    protected void SetTitle()
    {
        string title = article.SeoTitle;
        string description = article.SeoDescription;
        string keyword = article.SeoKeyword;

        if (title.Length < 3)
        {
            title = article.Title;
        }

        if (keyword.Length < 3)
        {
            keyword = article.Title + ", " + Globals.GetValueFromRegistry("SeoKeyword");
        }

        if (description.Length < 3)
        {
            description = article.Title + ", " + Globals.GetValueFromRegistry("SeoDescription");
        }
        string chuoi = "";
        Category cat = CategoryBL.GetCategoryByID("FriendlyUrl", article.CategoryID);
        if (cat != null)
        {
            chuoi = TextChanger.GetLinkRewrite_Article(article.FriendlyUrl, cat.FriendlyUrl);
            chuoi = " <link rel='canonical' href='" + chuoi + "' />";
        }
        ltrMetaTags.Text = string.Format(@"<title>{0}</title>
<meta name=""keywords"" content=""{1}"" />
<meta name=""description"" content=""{2}"" />
{3}
", title, keyword, description, chuoi);

        
    }
    #endregion


    #region Navigate
    protected void BindDataToNavigate()
    {
        if (article == null)
        {
            return;
        }
        string nav = string.Empty;
        Category cat = CategoryBL.GetCategoryByID(article.CategoryID);
        if (cat != null)
        {

            MenuArticleControl.category = cat;
            nav += string.Format(@"<li><a href=""{0}"">{1}</a></li>",
                                  TextChanger.GetLinkRewrite_CategoryArticle(cat.FriendlyUrl),
                                  cat.CategoryName);

            Category cateParent = CategoryBL.GetCategoryByID(cat.ParentCategoryID);
            if (cateParent != null)
            {
                nav =
                    string.Format(@"<li><a href=""{0}"">{1}</a></li>",
                                  TextChanger.GetLinkRewrite_CategoryArticle(cat.FriendlyUrl),
                                  cateParent.CategoryName) + nav;
            }
        }

        ltrCrumbs.Text = nav;// +"<li>Tin chi tiết</li>";
    }
    #endregion


    #region InCategory
    protected void BindDataInCategory()
    {
        int countArticleRelated = 0;
        List<Article> articleListNew = ArticleBL.GetAllArticle("", "ArticleID DESC", string.Format("CategoryID={0} AND Hide='False' AND ArticleID>{1}", CategoryID, ArticleID), SetQuantityArticleRelated, 1);
        if (articleListNew != null && articleListNew.Count > 0)
        {
            countArticleRelated = articleListNew.Count;
        }

        List<Article> articleListOld = ArticleBL.GetAllArticle("", "ArticleID DESC", string.Format("CategoryID={0} AND Hide='False' AND ArticleID<{1}", CategoryID, ArticleID), SetQuantityArticleRelated, 1);
        if (articleListOld != null && articleListOld.Count > 0)
        {
            countArticleRelated += articleListOld.Count;
        }
        else
        {
            pnlOldRelated.Visible = false;
        }

        if (countArticleRelated <= SetQuantityArticleRelated)
        {
            List<Article> articleListAll = ArticleBL.GetAllArticle("", "ArticleID DESC", string.Format("CategoryID={0} AND Hide='False' AND ArticleID<>{1}", CategoryID, ArticleID), SetQuantityArticleRelated, 1);
            if (articleListAll != null && articleListAll.Count > 0)
            {
                countArticleRelated += articleListOld.Count;
                rptInCategory.DataSource = articleListAll;
                rptInCategory.DataBind();
                pnlOldRelated.Visible = false;
            }
        }
        else
        {
            if (articleListNew != null && articleListNew.Count > 0)
            {
                rptInCategory.DataSource = articleListNew;
                rptInCategory.DataBind();
                ltrHeadInCategory.Text = "Bài viết mới";
            }

            if (articleListOld != null && articleListOld.Count > 0)
            {
                rptArticleOld.DataSource = articleListOld;
                rptArticleOld.DataBind();
                ltrArticleOld.Text = "Bài viết đã đăng";
            }
        }

        if (countArticleRelated == 0)
        {
            pnlNewArticle.Visible = false;
        }
    }

    protected void rptInCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Article article = (Article)e.Item.DataItem;
        HyperLink hplTitle = (HyperLink)e.Item.FindControl("hplTitle");

        if (article != null)
        {
            hplTitle.Text = article.Title;


            string caturl = string.Empty;
            Category cat = CategoryBL.GetCategoryByID("FriendlyUrl", article.CategoryID);
            if (cat != null)
            {
                caturl = cat.FriendlyUrl;
            }

            hplTitle.NavigateUrl = TextChanger.GetLinkRewrite_Article(article.FriendlyUrl, caturl);

            // thanh lọc cơ thể
            //string tomtat = article.Description;
            //tomtat = tomtat.Replace(@"""", "");
            //tomtat = tomtat.Replace(@"'", "");
            //tomtat = Regex.Replace(tomtat, "\\t|\\r|\\n", "");

            //tomtat = string.Format(@"<img style=\""float:left; padding:5px\"" width=\""50\"" height=\""50\"" src=\""{0}upload/article_images/small/{1}\"">{2}", Globals.BaseUrl, article.Image, tomtat);

            //hplTitle.Attributes["onMouseover"] = string.Format("ddrivetip('{0}',400)", tomtat);
            //hplTitle.Attributes["onMouseout"] = "hideddrivetip()";
        }
    }

    #endregion

}