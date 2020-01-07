using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDBModel.EntityTypes
{
    public class StudentPayment:EntityBase
    {
        public Student Student{ get; set; }
        public Class Class { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Ammount { get; set; }

        public IList<StudentPayment> StudentPayments { get; set; } = new List<StudentPayment>();

    }
}
