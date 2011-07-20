namespace Admin.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class Menu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Indicate that menu item selected
        /// </summary>
        /// <param name="pg">Page address</param>
        /// <returns>CSS class to append for current menu item</returns>
        protected string Current(string pg)
        {
            if (Request.Path.ToLower().Contains(pg.ToLower()))
            {
                return "class=\"content-box-selected\"";
            }
            return "";
        }
    }
}