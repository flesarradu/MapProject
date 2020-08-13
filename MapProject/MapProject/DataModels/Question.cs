using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.DataModels
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public int Answer1Risk { get; set; }
        public int Answer2Risk { get; set; }
        public int Answer3Risk { get; set; }
        public int Answer4Risk { get; set; }

    }
}
