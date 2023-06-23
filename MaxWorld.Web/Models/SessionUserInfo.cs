namespace MaxWorld.Web.Models
{
    public class SessionUserInfo
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }

        public SessionUserInfo(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}
