using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizData.Entities
{
    public class QuizEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";

        public List<QuestionEntity> Questions { get; set; } = new();
    }
}

