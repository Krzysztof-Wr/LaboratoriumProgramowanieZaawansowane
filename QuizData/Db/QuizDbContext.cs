using System;
using System.Collections.Generic;
using System.IO;
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
            // Szukamy folderu rozwiązania: idziemy w górę od bin/... do katalogu z .sln
            var baseDir = AppContext.BaseDirectory;
            var dir = new DirectoryInfo(baseDir);

            while (dir != null && !dir.GetFiles("*.sln").Any())
                dir = dir.Parent;

            if (dir == null)
                throw new Exception("Nie znaleziono pliku .sln - nie mogę ustalić ścieżki do bazy.");

            var dbPath = Path.Combine(dir.FullName, "quiz.db");
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
