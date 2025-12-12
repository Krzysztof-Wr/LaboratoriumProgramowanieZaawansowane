using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuizCore;
using System.Collections.ObjectModel;

namespace QuizWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Quiz> _quizzes = new ObservableCollection<Quiz>();
        public MainWindow()
        {
            InitializeComponent();
            QuizListBox.ItemsSource = _quizzes;
            QuizListBox.DisplayMemberPath = "Title";
        }
            

        private void AddQuizButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string description = DescriptionTextBox.Text;

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Podaj tytuł quizu.");
                return;
            }

            Quiz quiz = new Quiz(title, description);
            _quizzes.Add(quiz);

            TitleTextBox.Clear();
            DescriptionTextBox.Clear();
        }
    }
}
