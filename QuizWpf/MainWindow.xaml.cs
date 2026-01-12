using System.Collections.ObjectModel;
using System.Windows;
using QuizData.Entities;
using QuizData.Services;
using System.Linq;

namespace QuizWpf
{
    public partial class MainWindow : Window
    {
        private readonly QuizCrudService _crud = new QuizCrudService();
        private readonly ObservableCollection<QuizEntity> _quizzes = new ObservableCollection<QuizEntity>();
        private readonly ObservableCollection<QuestionEntity> _questions = new();
        private readonly ObservableCollection<AnswerEntity> _answers = new();

        public MainWindow()
        {
            InitializeComponent();

            QuizListBox.ItemsSource = _quizzes;
            QuizListBox.DisplayMemberPath = "Title";
            QuestionListBox.ItemsSource = _questions;
            AnswerListBox.ItemsSource = _answers;

            // Wczytaj quizy przy starcie okna
            Loaded += async (_, __) => await LoadQuizzesAsync();
        }

        private async System.Threading.Tasks.Task LoadQuizzesAsync()
        {
            _quizzes.Clear();
            var all = await _crud.GetAllQuizzesAsync();
            foreach (var q in all)
                _quizzes.Add(q);
        }

        private async void AddQuizButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string description = DescriptionTextBox.Text;


            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Podaj tytuł quizu.");
                return;
            }

            var quiz = new QuizEntity
            {
                Title = title,
                Description = description
            };

            await _crud.CreateQuizAsync(quiz);

            TitleTextBox.Clear();
            DescriptionTextBox.Clear();

            await LoadQuizzesAsync();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadQuizzesAsync();
        }

        private async void QuizListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (QuizListBox.SelectedItem is not QuizEntity selectedQuiz)
                return;

            _questions.Clear();
            _answers.Clear();

            var qs = await _crud.GetQuestionsForQuizAsync(selectedQuiz.Id);
            foreach (var q in qs)
                _questions.Add(q);
        }
        private async void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (QuizListBox.SelectedItem is not QuizEntity selectedQuiz)
            {
                MessageBox.Show("Najpierw wybierz quiz.");
                return;
            }

            var text = QuestionTextBox.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Podaj treść pytania.");
                return;
            }

            await _crud.AddQuestionAsync(selectedQuiz.Id, text);
            QuestionTextBox.Clear();

            // odśwież pytania
            var qs = await _crud.GetQuestionsForQuizAsync(selectedQuiz.Id);
            _questions.Clear();
            foreach (var q in qs) _questions.Add(q);

            _answers.Clear();
        }
        private void QuestionListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (QuestionListBox.SelectedItem is not QuestionEntity q)
                return;

            _answers.Clear();
            foreach (var a in q.Answers)
                _answers.Add(a);
        }
        private async void AddAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (QuestionListBox.SelectedItem is not QuestionEntity selectedQuestion)
            {
                MessageBox.Show("Najpierw wybierz pytanie.");
                return;
            }

            var text = AnswerTextBox.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Podaj treść odpowiedzi.");
                return;
            }

            bool isCorrect = IsCorrectCheckBox.IsChecked == true;

            await _crud.AddAnswerAsync(selectedQuestion.Id, text, isCorrect);

            AnswerTextBox.Clear();
            IsCorrectCheckBox.IsChecked = false;

            // odśwież pytania (żeby wczytać nowe Answers z bazy)
            var qs = await _crud.GetQuestionsForQuizAsync(selectedQuestion.QuizId);
            _questions.Clear();
            foreach (var q in qs) _questions.Add(q);

            // ustaw ponownie zaznaczone pytanie po odświeżeniu (po Id)
            var refreshed = qs.FirstOrDefault(x => x.Id == selectedQuestion.Id);
            if (refreshed != null)
            {
                QuestionListBox.SelectedItem = _questions.First(x => x.Id == refreshed.Id);

                _answers.Clear();
                foreach (var a in refreshed.Answers)
                    _answers.Add(a);
            }
        }

    }
}
