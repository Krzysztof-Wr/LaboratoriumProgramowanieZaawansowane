using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizData.Db;
using QuizData.Entities;

namespace QuizData.Services
{
    public class QuizCrudService
    {
        public async Task<int> CreateQuizAsync(QuizEntity quiz)
        {
            using var db = new QuizDbContext();
            db.Quizzes.Add(quiz);
            await db.SaveChangesAsync();
            return quiz.Id;
        }

        public async Task<List<QuizEntity>> GetAllQuizzesAsync()
        {
            using var db = new QuizDbContext();
            return await db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(qq => qq.Answers)
                .ToListAsync();
        }

        public async Task<QuizEntity?> GetQuizByIdAsync(int id)
        {
            using var db = new QuizDbContext();
            return await db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(qq => qq.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<bool> UpdateQuizTitleAsync(int id, string newTitle)
        {
            using var db = new QuizDbContext();
            var quiz = await db.Quizzes.FirstOrDefaultAsync(q => q.Id == id);
            if (quiz == null) return false;

            quiz.Title = newTitle;
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteQuizAsync(int id)
        {
            using var db = new QuizDbContext();
            var quiz = await db.Quizzes.FirstOrDefaultAsync(q => q.Id == id);
            if (quiz == null) return false;

            db.Quizzes.Remove(quiz);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<List<QuizEntity>> SearchQuizzesByTitleAsync(string term)
        {
            using var db = new QuizDbContext();

            return await db.Quizzes
                .Where(q => q.Title.Contains(term))
                .ToListAsync();
        }

        public async Task<List<QuizEntity>> GetQuizzesWithMinQuestionsAsync(int minQuestions)
        {
            using var db = new QuizDbContext();

            return await db.Quizzes
                .Include(q => q.Questions)
                .Where(q => q.Questions.Count >= minQuestions)
                .ToListAsync();
        }

        public async Task<List<QuizEntity>> GetQuizzesOrderedByQuestionCountAsync()
        {
            using var db = new QuizDbContext();

            return await db.Quizzes
                .Include(q => q.Questions)
                .OrderByDescending(q => q.Questions.Count)
                .ToListAsync();
        }
        public async Task<List<QuestionEntity>> GetQuestionsForQuizAsync(int quizId)
        {
            using var db = new QuizDbContext();
            return await db.Questions
                .Where(q => q.QuizId == quizId)
                .Include(q => q.Answers)
                .ToListAsync();
        }

        public async Task<int> AddQuestionAsync(int quizId, string text)
        {
            using var db = new QuizDbContext();

            var question = new QuestionEntity
            {
                QuizId = quizId,
                Text = text
            };

            db.Questions.Add(question);
            await db.SaveChangesAsync();
            return question.Id;
        }

        public async Task<int> AddAnswerAsync(int questionId, string text, bool isCorrect)
        {
            using var db = new QuizDbContext();

            var answer = new AnswerEntity
            {
                QuestionId = questionId,
                Text = text,
                IsCorrect = isCorrect
            };

            db.Answers.Add(answer);
            await db.SaveChangesAsync();
            return answer.Id;
        }

    }

}
