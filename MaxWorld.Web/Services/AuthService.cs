using MaxWorld.Data.Users;
using MaxWorld.Web.Utilities;

namespace MaxWorld.Web.Services
{
    public class AuthService : BaseService
    {
        public AuthService(BaseServiceArgument baseServiceArgument) : base(baseServiceArgument) { }

        protected string HashPassword(string password)
        {
            return Crypto.Sha512Encode(password);
        }

        public Task<bool> IsAccountRegisteredAsync(string email)
        {
            return Repository.QueryFirstAsync<bool>(@$"
                SELECT CASE WHEN EXISTS (
                    SELECT 1 FROM [User] WHERE [Email] = @email
                ) THEN 1 ELSE 0 END
                ", new { email });
        }

        public async Task<Guid> RegisterAsync(string email, string password, string name)
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = email,
                Name = name,
            };

            await Repository.ExecuteAsync(@"
                INSERT INTO [User] ([UserId], [Email], [Name])
                VALUES (@UserId, @Email, @Name)
                ", user);

            var userPassword = new UserPassword
            {
                UserId = user.UserId,
                PasswordHash = HashPassword(password)
            };

            await Repository.ExecuteAsync(@"
                INSERT INTO [UserPassword] ([UserId], [PasswordHash])
                VALUES (@UserId, @PasswordHash)
                ", userPassword);

            return user.UserId;
        }

        public Task<User?> LoginAsync(string email, string password)
        {
            return Repository.QueryFirstOrDefaultAsync<User?>(@"
                SELECT [User].* FROM [User]
                JOIN [UserPassword] WITH (NOLOCK)
                    ON [User].[UserId] = [UserPassword].[UserId]
                WHERE [User].[Email] = @Email
                    AND [PasswordHash] = @PasswordHash
                ", new
            {
                Email = email,
                PasswordHash = HashPassword(password)
            });
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var resetToken = Guid.NewGuid().ToString().Replace("-", "");
            await Repository.ExecuteAsync(@"
                UPDATE [UserPassword]
                SET [ResetToken] = @resetToken,
                    [ResetTokenExpiration] = @ResetTokenExpiration
                WHERE [UserId] = (
                    SELECT TOP(1) [UserId] FROM [User]
                    WHERE [Email] = @email
                )
                ", new
            {
                resetToken,
                ResetTokenExpiration = DateTime.Now.AddMinutes(30),
                email,
            });

            return resetToken;
        }

        public Task<Guid?> GetUserIdByPasswordResetTokenAsync(string token)
        {
            return Repository.QueryFirstOrDefaultAsync<Guid?>(@"
                SELECT [UserId] FROM [UserPassword]
                WHERE [ResetToken] = @token
                    AND [ResetTokenExpiration] >= @Now
                ", new
            {
                token,
                DateTime.Now
            });
        }

        public async Task<User> ResetPasswordAsync(Guid userId, string password)
        {
            await Repository.ExecuteAsync(@"
                UPDATE [UserPassword]
                SET [PasswordHash] = @passwordHash,
                    [ResetToken] = NULL,
                    [ResetTokenExpiration] = NULL
                WHERE [UserId] = @userId
                ", new
            {
                passwordHash = HashPassword(password),
                userId
            });

            return await Repository.QueryFirstOrDefaultAsync<User>(@"
                SELECT * FROM [User]
                WHERE [UserId] = @userId
                ", new { userId });
        }
    }
}
