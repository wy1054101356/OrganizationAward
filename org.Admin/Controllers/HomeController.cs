using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace org.Admin.Controllers
{
    public class HomeController : BaseController
	{
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.LoginUser = _UserInfo;
            return View();
        }
    }
}