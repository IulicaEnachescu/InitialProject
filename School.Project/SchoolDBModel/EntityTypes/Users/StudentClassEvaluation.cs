using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDBModel.EntityTypes
{
   public class StudentClassEvaluation:EntityBase
    {

        public Class Class { get; set; } = new Class();
        public Student Student { get; set; } = new Student();
        public string Description { get; set; }
        public int Grade { get; set; }

        
    }
}
