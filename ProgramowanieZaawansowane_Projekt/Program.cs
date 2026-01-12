using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuizCore;
using QuizData.Entities;
using QuizData.Services;

namespace ProgramowanieZaawansowane_Projekt
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            RunQuiz();

            TestRepo();

            await TestEfAsync();

            Console.WriteLine("\nNaciśnij dowolny klawisz, aby zakończyć.");
            Console.ReadKey();
        }

        private static void RunQuiz()
        {
            Quiz quiz = new Quiz("Test", "Quiz testowy do sprawdzenia działania.");

            Answer jed = new Answer("1", false);
            Answer dwa = new Answer("2", false);
            Answer czt = new Answer("4", true);

            Question q_pier = new Question("Ile to jest 2 + 2?");
            q_pier.AddAnswer(jed);
            q_pier.AddAnswer(dwa);
            q_pier.AddAnswer(czt);

            quiz.AddQuestion(q_pier);

            List<int> odpUzytkownika = new List<int>();

            Console.WriteLine($"Quiz: {quiz.Title}");
            Console.WriteLine(quiz.Description);
            Console.WriteLine();

            foreach (var question in quiz.Questions)
            {
                Console.WriteLine(question.Text);

                for (int i = 0; i < question.Answers.Count; i++)
                {
                    Console.WriteLine($"{i}: {question.Answers[i].Text}");
                }

                int wybranyIndex;

                while (true)
                {
                    Console.Write("Wybierz numer odpowiedzi: ");
                    string? input = Console.ReadLine();

                    if (int.TryParse(input, out wybranyIndex) &&
                        wybranyIndex >= 0 &&
                        wybranyIndex < question.Answers.Count)
                    {
                        break;
                    }

                    Console.WriteLine("Nieprawidłowy numer odpowiedzi, spróbuj jeszcze raz.");
                }

                odpUzytkownika.Add(wybranyIndex);
                Console.WriteLine();
            }

            int wynik = quiz.CalculateScore(odpUzytkownika);

            Console.WriteLine($"Twój wynik: {wynik}/{quiz.GetTotalQuestions()}");
            Console.WriteLine();
        }

        private static void TestRepo()
        {
            Quiz quiz = new Quiz("Test", "Quiz testowy do sprawdzenia działania.");

            var repo = new QuizRepository<IQuiz>();
            repo.AddQuiz(quiz);

            Console.WriteLine("Repo ma quizów: " + repo.GetAll().Count);

            var znaleziony = repo.FindByTitle("Test");
            Console.WriteLine(znaleziony != null
                ? "Znaleziono quiz: " + znaleziony.Title
                : "Nie znaleziono quizu");

            Console.WriteLine();
        }

        private static async Task TestEfAsync()
        {
            var crud = new QuizCrudService();

            // CREATE
            var quizEntity = new QuizEntity
            {
                Title = "Quiz z bazy",
                Description = "Pierwszy zapis do SQLite",
                Questions =
                {
                    new QuestionEntity
                    {
                        Text = "Ile to 2 + 2?",
                        Answers =
                        {
                            new AnswerEntity { Text = "3", IsCorrect = false },
                            new AnswerEntity { Text = "4", IsCorrect = true },
                            new AnswerEntity { Text = "5", IsCorrect = false }
                        }
                    }
                }
            };

            int newId = await crud.CreateQuizAsync(quizEntity);
            Console.WriteLine($"Dodano quiz do bazy. ID={newId}");

            // READ (all)
            var all = await crud.GetAllQuizzesAsync();
            Console.WriteLine($"W bazie jest quizów: {all.Count}");

            // READ (one)
            var one = await crud.GetQuizByIdAsync(newId);
            Console.WriteLine(one != null ? $"Pobrano: {one.Title}" : "Nie znaleziono quizu");

            // UPDATE
            await crud.UpdateQuizTitleAsync(newId, "Quiz z bazy (zmieniony tytuł)");
            Console.WriteLine("Zmieniono tytuł.");

            // LINQ: wyszukiwanie po tytule
            var found = await crud.SearchQuizzesByTitleAsync("Quiz");
            Console.WriteLine($"LINQ - znalezione (tytuł zawiera 'Quiz'): {found.Count}");

            // LINQ: quizy z min. 1 pytaniem
            var withMinQ = await crud.GetQuizzesWithMinQuestionsAsync(1);
            Console.WriteLine($"LINQ - quizy z min. 1 pytaniem: {withMinQ.Count}");

            // LINQ: sortowanie po liczbie pytań
            var ordered = await crud.GetQuizzesOrderedByQuestionCountAsync();
            Console.WriteLine("LINQ - posortowane po liczbie pytań (malejąco):");
            foreach (var q in ordered)
            {
                int countQ = q.Questions?.Count ?? 0;
                Console.WriteLine($"- {q.Title} (pytań: {countQ})");
            }


            // DELETE (testujemy raz, żeby CRUD był pełny)
            //bool deleted = await crud.DeleteQuizAsync(newId);
            //Console.WriteLine(deleted ? "Usunięto quiz (DELETE OK)." : "Nie udało się usunąć quizu.");

            Console.WriteLine();
        }
    }
}
