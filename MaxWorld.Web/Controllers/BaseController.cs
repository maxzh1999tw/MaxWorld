using MaxWorld.Web.Models;
using MaxWorld.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MaxWorld.Web.Controllers
{
    /// <summary>
    /// 基礎的 Controller 類別，提供資料庫存取的能力與常用的方法
    /// </summary>
    public abstract class BaseController : Controller
    {
        public const string InvalidModelState = nameof(InvalidModelState);

        /// <summary>
        /// 資料庫操控物件
        /// </summary>
        protected Repository Repository { get; set; }

        public BaseController(BaseControllerArgument baseControllerArgument)
        {
            Repository = baseControllerArgument.Repository;
        }

        /// <summary>
        /// 使用者登入資訊
        /// </summary>
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

        /// <summary>
        /// 回傳成功 API 回覆格式
        /// </summary>
        protected JsonResult ApiSuccess(object? payload = null)
        {
            return Json(new
            {
                Success = true,
                Payload = payload
            });
        }

        /// <summary>
        /// 回傳失敗 API 回覆格式
        /// </summary>
        /// <param name="errorCode">錯誤代碼</param>
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
