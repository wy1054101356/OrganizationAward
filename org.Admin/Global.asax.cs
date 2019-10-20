using CZ.Admin.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace org.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
			log4net.Config.XmlConfigurator.Configure();
			//500 筛选器

			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
		}


        
	}
}
