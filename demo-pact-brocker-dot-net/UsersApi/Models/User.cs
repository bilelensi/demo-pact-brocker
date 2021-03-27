using System;

namespace UsersApi.Models
{
    public class User
    {
        public int ID { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime LastUpdate { get; set; }

        public GenderEnum Gender { get; set; }

        public PositionEnum Position { get; set; }
    }
}
