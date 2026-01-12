using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizData.Db;
using QuizData.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace QuizWeb.Pages.Quizzes
{
    public class IndexModel : PageModel
    {
        private readonly QuizDbContext _db;

        public IndexModel(QuizDbContext db)
        {
            _db = db;
        }

        public List<QuizEntity> Quizzes { get; set; } = new();

        public async Task OnGetAsync()
        {
            Quizzes = await _db.Quizzes.ToListAsync();
        }
    }
}
