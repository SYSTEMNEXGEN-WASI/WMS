using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using DevExpress.Web.ASPxGridView;

public class MyTemplate : ITemplate
{

    #region ITemplate Members

    public void InstantiateIn(Control container)
    {
        LiteralControl lc = new LiteralControl();
        lc.ID = "label";
        GridViewEditItemTemplateContainer templateContainer = container as GridViewEditItemTemplateContainer;
        lc.Text = templateContainer.Text;
        container.Controls.Add(lc);
    }
    #endregion
}

