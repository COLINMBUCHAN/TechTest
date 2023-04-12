using MyApp.Models.Base;
using System;

namespace MyApp.Models
{
    public class User : ModelBase
    {
        public string Forename { get; set; }

        public string Surname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}