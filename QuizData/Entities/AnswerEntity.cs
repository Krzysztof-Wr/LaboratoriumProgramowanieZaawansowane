using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizData.Entities
{
    public class AnswerEntity
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public QuestionEntity? Question { get; set; }
    }
}

