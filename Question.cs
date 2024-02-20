using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace memory

{

    public class Answer
    {
        public bool correctAnswer { get; set; }
        public string text { get; set; }
    }

    public class Data
    {
        public string text { get; set; }
        public List<Question> questions { get; set; }
    }

    public class Question
    {
        public string question { get; set; }
        public List<Answer> answers { get; set; }
    }

    public class Level
    {
        public int level { get; set; }
        public int score { get; set; }
        public List<Data> data { get; set; }
    }
}
