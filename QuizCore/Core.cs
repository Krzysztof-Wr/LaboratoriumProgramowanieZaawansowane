using System;
using System.Collections.Generic;

namespace QuizCore
{
    // ===== INTERFEJSY =====

    public interface IAnswer
    {
        string Text { get; }
        bool IsCorrect { get; }
    }

    public interface IQuestion
    {
        string Text { get; }
        IReadOnlyList<IAnswer> Answers { get; }

        void AddAnswer(IAnswer answer);
        bool IsAnswerCorrect(int answerIndex);
    }

    public interface IQuiz
    {
        string Title { get; }
        string Description { get; }
        IReadOnlyList<IQuestion> Questions { get; }

        void AddQuestion(IQuestion question);
        int GetTotalQuestions();
        int CalculateScore(IList<int> userAnswers);
    }

    // ===== KLASY =====

    public class Answer : IAnswer
    {
        public string Text { get; }
        public bool IsCorrect { get; }

        public Answer(string text, bool isCorrect)
        {
            Text = text;
            IsCorrect = isCorrect;
        }
    }

    public class Question : IQuestion
    {
        public string Text { get; }

        private readonly List<IAnswer> _answers;
        public IReadOnlyList<IAnswer> Answers => _answers;

        public Question(string text)
        {
            Text = text;
            _answers = new List<IAnswer>();
        }

        public void AddAnswer(IAnswer answer)
        {
            _answers.Add(answer);
        }

        public bool IsAnswerCorrect(int answerIndex)
        {
            if (answerIndex < 0 || answerIndex >= _answers.Count)
                return false;

            return _answers[answerIndex].IsCorrect;
        }
    }

    public class Quiz : IQuiz
    {
        public string Title { get; }
        public string Description { get; }

        private readonly List<IQuestion> _questions;
        public IReadOnlyList<IQuestion> Questions => _questions;

        public Quiz(string title, string description)
        {
            Title = title;
            Description = description;
            _questions = new List<IQuestion>();
        }

        public void AddQuestion(IQuestion question)
        {
            _questions.Add(question);
        }

        public int GetTotalQuestions()
        {
            return _questions.Count;
        }

        public int CalculateScore(IList<int> userAnswers)
        {
            int score = 0;

            int count = Math.Min(_questions.Count, userAnswers.Count);

            for (int i = 0; i < count; i++)
            {
                if (_questions[i].IsAnswerCorrect(userAnswers[i]))
                    score++;
            }

            return score;
        }
    }

    public class QuizRepository<TQuiz> where TQuiz : IQuiz
    {
        private readonly List<TQuiz> _quizzes = new List<TQuiz>();

        public void AddQuiz(TQuiz quiz)
        {
            _quizzes.Add(quiz);
        }

        public IReadOnlyList<TQuiz> GetAll()
        {
            return _quizzes;
        }

        public TQuiz? FindByTitle(string title)
        {
            foreach (var quiz in _quizzes)
            {
                if (string.Equals(quiz.Title, title, StringComparison.OrdinalIgnoreCase))
                    return quiz;
            }

            return default;
        }
    }

}