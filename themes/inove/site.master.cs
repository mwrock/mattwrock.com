using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class site : System.Web.UI.MasterPage
{
  protected void Page_Load(object sender, EventArgs e)
  {
		if (Page.User.Identity.IsAuthenticated)
		{
			aLogin.InnerText = Resources.labels.logoff;
			aLogin.HRef = BlogEngine.Core.Utils.RelativeWebRoot + "login.aspx?logoff";
		}
		else
		{
			aLogin.HRef = BlogEngine.Core.Utils.RelativeWebRoot + "login.aspx";
			aLogin.InnerText = Resources.labels.login;
		}
  }

  public string MenuClass(string menuName)
  {
      if (Request.RawUrl.Contains(menuName))
          return "class=\"current_page_item\"";
      else
          return "class=\"page_item\"";
  }

}
