using MaxWorld.Web.Controllers;
using MaxWorld.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MaxWorld.Web.Filters
{
    /// <summary>
    /// 自訂 Authorize Filter，
    /// 當未登入時根據 Action 是否為 API 來決定回傳 401 或重導向至登入頁。
    /// Action 上的設定會覆寫 Controller 的設定。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly bool _isApi;

        public CustomAuthorizeAttribute(bool isApi = false)
        {
            _isApi = isApi;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor is not ControllerActionDescriptor 
                || IsOverwrittenByAction(context)
                || IsAllowAnonymous(context))
            {
                return;
            }

            var session = context.HttpContext.Session;
            var sessionUserInfoJson = session.GetString(nameof(SessionUserInfo));

            if (string.IsNullOrEmpty(sessionUserInfoJson))
            {
                context.Result = _isApi ?  
                    new UnauthorizedResult() : 
                    new RedirectToActionResult(nameof(HomeController.Login), "Home", null);
            }
        }

        private static bool IsOverwrittenByAction(AuthorizationFilterContext context)
        {
            var actionAttributes = context.ActionDescriptor.EndpointMetadata;
            return actionAttributes.OfType<CustomAuthorizeAttribute>().Count() > 1;
        }

        private static bool IsAllowAnonymous(AuthorizationFilterContext context)
        {
            var actionAttributes = context.ActionDescriptor.EndpointMetadata;
            return actionAttributes.OfType<AllowAnonymousAttribute>().Any();
        }
    }
}
