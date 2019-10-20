using Newtonsoft.Json;
using org.Admin.Common;
using org.Admin.Controllers;
using org.Common;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CZ.Admin.App_Start
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
            filters.Add(new CustomErrorAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
	public class CustomErrorAttribute : HandleErrorAttribute
	{
		public override void OnException(ExceptionContext filterContext)
		{
			base.OnException(filterContext);
			ErrorMessage msg = new ErrorMessage(filterContext.Exception, "页面");
			msg.ShowException = MvcException.IsExceptionEnabled();
			//错误记录
			LogHelper.WriteLog(JsonConvert.SerializeObject(msg, Formatting.Indented),null);

			//设置为true阻止golbal里面的错误执行
			filterContext.ExceptionHandled = true;
			filterContext.Result = new ViewResult() { ViewName = "/Views/Error/Error500.cshtml", ViewData = new ViewDataDictionary<ErrorMessage>(msg) };
		}
	}
	
	/// <summary>
	/// 异常信息显示
	/// </summary>
	public class MvcException
	{
		/// <summary>
		/// 是否已经获取的允许显示异常
		/// </summary>
		private static bool HasGetExceptionEnabled = false;

		private static bool isExceptionEnabled;

		/// <summary>
		/// 是否显示异常信息
		/// </summary>
		/// <returns>是否显示异常信息</returns>
		public static bool IsExceptionEnabled()
		{
			if (!HasGetExceptionEnabled)
			{
				isExceptionEnabled = GetExceptionEnabled();
				HasGetExceptionEnabled = true;
			}
			return isExceptionEnabled;
		}

		/// <summary>
		/// 根据Web.config AppSettings节点下的ExceptionEnabled值来决定是否显示异常信息
		/// </summary>
		/// <returns></returns>
		private static bool GetExceptionEnabled()
		{
			bool result;
			if (!Boolean.TryParse(ConfigurationManager.AppSettings["ExceptionEnabled"], out result))
			{
				return false;
			}
			return result;
		}
	}
}