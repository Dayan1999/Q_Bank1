using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    public class StudentModel
    {
        public int student_id { get; set; }
        public string Student_name { get; set; }
        public string Student_password { get; set; }
        public string Student_email { get; set; }       
        public int mobile_no { get; set; }
        public int school_name { get; set; }   
        public string Confirm_Password { get; set; }
        
    }
}
