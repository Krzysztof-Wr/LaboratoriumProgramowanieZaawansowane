using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizData.Entities;

namespace QuizData.Db
{
    public class QuizDbContext : DbContext
    {
        public DbSet<QuizEntity> Quizzes => Set<QuizEntity>();
        public DbSet<QuestionEntity> Questions => Set<QuestionEntity>();
        public DbSet<AnswerEntity> Answers => Set<AnswerEntity>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = System.IO.Path.Combine(AppContext.BaseDirectory, "quiz.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Quiz (1) -> (many) Questions
            modelBuilder.Entity<QuestionEntity>()
                .HasOne(q => q.Quiz)
                .WithMany(quiz => quiz.Questions)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            // Question (1) -> (many) Answers
            modelBuilder.Entity<AnswerEntity>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
