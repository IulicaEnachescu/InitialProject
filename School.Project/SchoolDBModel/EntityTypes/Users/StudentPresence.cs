using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDBModel.EntityTypes
{
    public class StudentPresence : EntityBase
    {
        public ClassTimeTable ClassTimetable { get; set; } = new ClassTimeTable();
        public Student Student { get; set; } = new Student();
        public bool Presence { get; set; }

        public IList<StudentPresence> StudentPresences { get; set; } = new List<StudentPresence>();
    }
}
