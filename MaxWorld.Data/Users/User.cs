﻿namespace MaxWorld.Data.Users
{
    public class User
    {
        public Guid UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public virtual UserPassword? Password { get; set; }
    }
}