using MaxWorld.Data.Users;
using MaxWorld.Web.Filters;
using MaxWorld.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaxWorld.Web.Controllers
{
    [CustomAuthorize]
    public class NotificationController : BaseController
    {
        private readonly NotificationService _notificationService;

        public NotificationController(BaseControllerArgument baseControllerArgument,
            NotificationService notificationService)
            : base(baseControllerArgument)
        {
            _notificationService = notificationService;
        }

        [ApiExceptionFilter]
        [CustomAuthorize(isApi: true)]
        public async Task<IActionResult> GetNotifications()
        {
            return ApiSuccess(new Notification[]
            {
                new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    UserId = SessionUserInfo.UserId,
                    CreateTime = DateTime.Now.AddDays(-2),
                    IconClass = "fas fa-file-alt text-white",
                    Content = "測試通知",
                    Read = false,
                    Href = Url.Action("Index")
                }
            });

            var notifications = await _notificationService.GetNotificationsAsync(SessionUserInfo.UserId);
            return ApiSuccess(notifications);
        }
    }
}
