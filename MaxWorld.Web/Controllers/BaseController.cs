using Microsoft.AspNetCore.Mvc;
using MaxWorld.Web.Models;
using MaxWorld.Web.Repositories;
using System.Text.Json;

namespace MaxWorld.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public const string InvalidModelState = nameof(InvalidModelState);

        protected Repository Repository { get; set; }

        public BaseController(BaseControllerArgument baseControllerArgument)
        {
            Repository = baseControllerArgument.Repository;
        }

        protected SessionUserInfo? SessionUserInfo
        {
            get
            {
                var sessionUserInfoJson = HttpContext.Session.GetString(nameof(SessionUserInfo));
                if (string.IsNullOrEmpty(sessionUserInfoJson))
                {
                    return null;
                }

                return JsonSerializer.Deserialize<SessionUserInfo>(sessionUserInfoJson);
            }
            set
            {
                if (value == null)
                {
                    HttpContext.Session.Remove(nameof(SessionUserInfo));
                }
                else
                {
                    HttpContext.Session.SetString(nameof(SessionUserInfo), JsonSerializer.Serialize(value));
                }
            }
        }

        protected JsonResult ApiSuccess(object? payload = null)
        {
            return Json(new
            {
                Success = true,
                Payload = payload
            });
        }

        protected JsonResult ApiFailed(string? errorCode = null, object? payload = null)
        {
            return Json(new
            {
                Success = false,
                ErrorCode = errorCode,
                Payload = payload
            });
        }
    }

    public class BaseControllerArgument
    {
        public Repository Repository { get; set; }

        public BaseControllerArgument(Repository repository)
        {
            Repository = repository;
        }
    }
}
