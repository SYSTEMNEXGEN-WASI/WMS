using System;
interface IPageBase
{
    string ConcatDataKeys(global::System.Web.UI.WebControls.DataKey k);
    global::System.Web.UI.Control FindCtrl(global::System.Web.UI.Control rootCtrl, string controlID_Tob_Find);        
}
