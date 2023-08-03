using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    internal class AnswerModel
    {
        public int question_id {  get; set; }
        public int Answer_id { get; set; }

        public string Answer_description { get; set; }
        public bool IsCorrectAnswer { get; set; }
    }
}
