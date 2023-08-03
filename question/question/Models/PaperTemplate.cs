using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    internal class PaperTemplate
    {
        public int module_id{ get; set;}
        public int exam_id { get; set;}
        public string exam_name { get; set;}
        public string exam_subject { get; set;}
        public int No_of_Questions { get; set;}

    }
}
