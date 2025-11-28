using QuizCore;

namespace ProgramowanieZaawansowane_Projekt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Quiz quiz = new Quiz("Test", "Quiz testowy do sprawdzenia działania.");

            Answer jed = new Answer("1", false);
            Answer dwa = new Answer("2", false);
            Answer czt= new Answer("4", true);

            Question q_pier = new Question("Ile to jest 2 + 2?");
            q_pier.AddAnswer(jed);
            q_pier.AddAnswer(dwa);
            q_pier.AddAnswer(czt);

            quiz.AddQuestion(q_pier);

            foreach (var question in quiz.Questions)
            {
                Console.WriteLine(question.Text);

                for (int i = 0; i < question.Answers.Count; i++)
                {
                    Console.WriteLine($"{i}: {question.Answers[i].Text}");
                }
            }

            // 2. Zasymulowane odpowiedzi użytkownika (wybrał odpowiedź 2 -> "4")
            List<int> userAnswers = new List<int> { 2 };

            // 3. Obliczamy wynik
            int score = quiz.CalculateScore(userAnswers);
            Console.WriteLine($"\nTwój wynik: {score}/{quiz.GetTotalQuestions()}");

        }
    }
}
