using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Enrollment
    {

        public int IdEnrollment { get; set; }
        public int IdStudy { get; set; }
        public int Semester { get; set; }
        public DateTime StartDate { get; set; }



        public override string ToString()
        { 
            return IdEnrollment + ", id study:" + IdStudy + ", Semester: " +Semester + "Start date: "+ StartDate ;
        }
    }
}
