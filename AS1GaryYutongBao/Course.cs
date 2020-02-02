using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS1GaryYutongBao
{
    /// <summary>
    /// Course
    /// </summary>
    public class Course
    {   
        public string department;
        public string level;
        public double credits;
        public Course(string department, string level, double credits)
        {
            this.department = department;
            this.level = level;
            this.credits = credits;
        }
    }
}
