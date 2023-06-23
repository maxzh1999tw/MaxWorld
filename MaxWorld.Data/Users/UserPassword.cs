using System.ComponentModel.DataAnnotations;

namespace MaxWorld.Data.Users
{
    public class UserPassword
    {
        [Key]
        public Guid UserId { get; set; }

        public string PasswordHash { get; set; } = null!;

        public string? ResetToken { get; set; } = null;
        public DateTime? ResetTokenExpiration { get; set; } = null;

        public virtual User User { get; set; } = null!;
    }
}
