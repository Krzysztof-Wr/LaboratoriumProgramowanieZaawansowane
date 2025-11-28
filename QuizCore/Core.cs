using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizCore
{
    public class Answer
    {
        public string Text { get; }
        public bool IsCorrect { get; }

        public Answer(string text, bool isCorrect)
        {
            Text = text;
            IsCorrect = isCorrect;
        }

        public void PrintText()
        {
            Console.WriteLine(Text);
        }
    }

    public class Question
    {
        public string Text { get; }
        public List<Answer> Answers { get; }

        public Question(string text)
        {
            Text = text;
            Answers = new List<Answer>();
        }

        public void AddAnswer(Answer answer)
        {
            Answers.Add(answer);
        }

        public bool IsAnswerCorrect(int answerIndex)
        {
            if (answerIndex < 0 || answerIndex >= Answers.Count)
                return false;

            return Answers[answerIndex].IsCorrect;
        }
    }

    public class Quiz
    {
        public string Title { get; }
        public string Description { get; }
        public List<Question> Questions { get; }

        public Quiz(string title, string description)
        {
            Title = title;
            Description = description;
            Questions = new List<Question>();
        }

        public void AddQuestion(Question question)
        {
            Questions.Add(question);
        }

        public int GetTotalQuestions()
        {
            return Questions.Count;
        }

        public int CalculateScore(List<int> userAnswers)
        {
            int score = 0;

            int count = Math.Min(Questions.Count, userAnswers.Count);

            for (int i = 0; i < count; i++)
            {
                if (Questions[i].IsAnswerCorrect(userAnswers[i]))
                {
                    score++;
                }
            }

            return score;
        }

    }
}
