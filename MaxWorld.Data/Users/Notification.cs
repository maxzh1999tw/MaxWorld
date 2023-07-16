namespace MaxWorld.Data.Users
{
    public class Notification
    {
        public Guid NotificationId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreateTime { get; set; }

        public string IconClass { get; set; } = null!;

        public string? Href { get; set; }

        public string Content { get; set; } = null!;

        public bool Read { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
