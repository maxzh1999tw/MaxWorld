using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MaxWorld.Web.Filters
{
    /// <summary>
    /// API 專用的 ExceptionFilter，會以標準格式回傳錯誤訊息
    /// </summary>
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            context.Result = new JsonResult(new
            {
                Success = false,

#if DEBUG
                Payload = context.Exception.ToString(),
#endif
            });
        }
    }
}
