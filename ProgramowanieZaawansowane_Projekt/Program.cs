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

            List<int> odpUzytkownika = new List<int>();

            Console.WriteLine($"Quiz: {quiz.Title}");
            Console.WriteLine(quiz.Description);
            Console.WriteLine();

            //      --- Przechodzimy po wszystkich pytaniach w quizie ---
            foreach (var question in quiz.Questions)
            {
                Console.WriteLine(question.Text);

                for (int i = 0; i < question.Answers.Count; i++) 
                {
                    Console.WriteLine($"{i}: { question.Answers[i].Text}");
                }


                int wybranyIndex;

                while (true)
                {
                    Console.WriteLine("Wybierz numer odpowiedzi: ");
                    string? input = Console.ReadLine();

                    if(int.TryParse(input, out wybranyIndex) && wybranyIndex >= 0 && wybranyIndex < question.Answers.Count)
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
            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć.");
            Console.ReadKey();

        }
    }
}
