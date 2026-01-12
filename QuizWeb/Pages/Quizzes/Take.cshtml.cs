using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuizData.Db;
using QuizData.Entities;

namespace QuizWeb.Pages.Quizzes
{
    public class TakeModel : PageModel
    {
        private readonly QuizDbContext _db;

        public TakeModel(QuizDbContext db)
        {
            _db = db;
        }

        public QuizEntity? Quiz { get; set; }

        // Klucz: QuestionId, Wartoœæ: AnswerId
        [BindProperty]
        public Dictionary<int, int> SelectedAnswerByQuestionId { get; set; } = new();

        public int? Score { get; set; }
        public int TotalQuestions { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Quiz = await _db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(qq => qq.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (Quiz == null)
                return NotFound();

            TotalQuestions = Quiz.Questions.Count;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Quiz = await _db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(qq => qq.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (Quiz == null)
                return NotFound();

            int score = 0;

            foreach (var question in Quiz.Questions)
            {
                if (SelectedAnswerByQuestionId.TryGetValue(question.Id, out int selectedAnswerId))
                {
                    var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == selectedAnswerId);
                    if (selectedAnswer != null && selectedAnswer.IsCorrect)
                        score++;
                }
            }

            Score = score;
            TotalQuestions = Quiz.Questions.Count;

            return Page();
        }
    }
}
