using MaxWorld.Data.Users;

namespace MaxWorld.Web.Services
{
    public class NotificationService : BaseService
    {
        public NotificationService(BaseServiceArgument baseServiceArgument)
            : base(baseServiceArgument) { }

        public Task<IEnumerable<Notification>> GetNotificationsAsync(Guid userId, int offset = 0, int count = 5)
        {
            return Repository.QueryAsync<Notification>(@$"
                SELECT * FROM [{nameof(Notification)}]
                WHERE [{nameof(User.UserId)}] = @userId
                ORDER BY [{nameof(Notification.CreateTime)}] DESC
                OFFSET @offset ROWS
                FETCH NEXT @count ROWS ONLY
                ", new
            {
                userId,
                offset,
                count
            });
        }
    }
}
