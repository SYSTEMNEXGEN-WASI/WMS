using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WebApplication2;
/// <summary>
/// Summary description for PageBase
/// </summary>
public class PageBase : System.Web.UI.Page, IPageBase
{
    public PageBase()
        : base()
    {
    }
    public string ConcatDataKeys(DataKey k)
    {
        string s = string.Empty;
        foreach (object eachItem in k.Values)
        {
            s += "|" + eachItem.ToString();
        }
        return s;
    }

    public Control FindCtrl(Control rootCtrl, string controlID_Tob_Find)
    {
        if (rootCtrl.ID == controlID_Tob_Find)
        {
            return rootCtrl;
        }
        else
        {
            foreach (Control eachItem in rootCtrl.Controls)
            {
                Control c = FindCtrl(eachItem, controlID_Tob_Find);
                if (c != null)
                {
                    return c;
                }
            }
            return null;
        }
    }


    private SessionForPage _sessionPage;
    public SessionForPage SessionPage
        {
            get
            {
                if (_sessionPage == null)
                {
                    _sessionPage = new SessionForPage(this.GetType().Name, Session);
                }
                return _sessionPage;
            }
        }

    public ClsSessionInfo SessionInformation
    {
        get
        {
            if (Session["clsSessionInfo"] != null)
            {
                ClsSessionInfo obj = new ClsSessionInfo();
                obj = (ClsSessionInfo)Session["clsSessionInfo"];
                return obj;
            }
            else
            {
                return null;
            }
        }
    }

    public static void SendAlert(string sMessage) 
    {
        sMessage = "alert('" + sMessage.Replace("'", @"\'").Replace("\n", @"\n") + "');";

        if (HttpContext.Current.CurrentHandler is Page)
        {
            Page p = (Page)HttpContext.Current.CurrentHandler;

            if (ScriptManager.GetCurrent(p) != null)
            {
                ScriptManager.RegisterStartupScript(p, typeof(Page), "Message", sMessage, true);
            }
            else
            {
                p.ClientScript.RegisterStartupScript(typeof(Page), "Message", sMessage, true);
            }
        }
    }

    //public static void SendConfirm(string sMessage)
    //{
    //    //sMessage = "confirm('" + sMessage.Replace("'", @"\'").Replace("\n", @"\n") + "');";
    //    sMessage = "confirm();";
    //    if (HttpContext.Current.CurrentHandler is Page)
    //    {
    //        Page p = (Page)HttpContext.Current.CurrentHandler;

    //        if (ScriptManager.GetCurrent(p) != null)
    //        {
    //            ScriptManager.RegisterStartupScript(p, typeof(Page), "Message", sMessage, true);

    //        }
    //        else
    //        {
    //            p.ClientScript.RegisterStartupScript(typeof(Page), "Message", sMessage, true);
    //        }
    //    }
    //}

}
public class SessionForPage
{
    private System.Web.SessionState.HttpSessionState _session;
    private string _PageName;
    public SessionForPage(string pPageName, System.Web.SessionState.HttpSessionState pSession)
    {
        _session = pSession;
        _PageName = pPageName;
    }
    public object this[string name]
    {
        get { return _session[_PageName + name]; }
        set { _session[_PageName + name] = value; }
    }

    public object this[int index]
    {
        get { return _session[index]; }
        set { _session[index] = value; }
    }

    
}

