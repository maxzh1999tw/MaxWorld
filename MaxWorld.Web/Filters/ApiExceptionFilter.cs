using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MaxWorld.Web.Filters
{
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
