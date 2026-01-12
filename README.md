# System Quizów – Programowanie Zaawansowane

Projekt przedstawia kompletny system quizów zrealizowany w technologii .NET, składający się z kilku warstw:
- logiki biznesowej
- bazy danych
- aplikacji desktopowej
- aplikacji webowej

## Struktura rozwiązania

| Projekt | Opis |
|-------|------|
| QuizCore | Logika domenowa (Quiz, Question, Answer), interfejsy i generyki |
| QuizData | Entity Framework Core, SQLite, CRUD, LINQ |
| QuizWpf | Panel administracyjny (tworzenie quizów, pytań i odpowiedzi) |
| QuizWeb | Strona internetowa do rozwiązywania quizów (Razor Pages + Blazor) |
| ProgramowanieZaawansowane_Projekt | Aplikacja konsolowa (CLI) do testów |

---

## Funkcjonalności

- Tworzenie quizów, pytań i odpowiedzi (WPF)
- Przechowywanie danych w SQLite (EF Core)
- Wyświetlanie i rozwiązywanie quizów w przeglądarce (Razor Pages)
- Komponent Blazor do wyświetlania listy quizów
- Program konsolowy do testów i uruchamiania LINQ
- Obsługa asynchroniczna (async/await)

---

## Jak uruchomić

1. Otwórz rozwiązanie w Visual Studio
2. Ustaw projekt startowy:
   - **QuizWpf** – do zarządzania quizami
   - **QuizWeb** – do rozwiązywania quizów w przeglądarce
3. Uruchom (`F5`)

Baza danych `quiz.db` znajduje się w folderze projektu:
