using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDBModel.EntityTypes
{
    public class User:EntityBase
    {

        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime CreateDate { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        
        public string email;

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                if (!(value.Contains("@") && value.Contains("."))) Console.WriteLine("Introduceti o adresa de email valida");
                else email = value;
            }
        }

        public IList<User> UsersList { get; set; } = new List<User>();
        
      
        
    }
}

