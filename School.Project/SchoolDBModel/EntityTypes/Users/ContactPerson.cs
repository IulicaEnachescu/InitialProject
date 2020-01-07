using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDBModel.EntityTypes
{
    public class ContactPerson:EntityBase
    {
        
        public string Name { get; set; }
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


        public List<Student> Students { get; set; } = new List<Student>();
    }
}

