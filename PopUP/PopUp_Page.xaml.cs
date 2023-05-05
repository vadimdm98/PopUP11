using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace PopUP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUp_Page : ContentPage
    {
        private int questionIndex;
        private int score;
        private List<int> numbers;
        private Random random;

        public PopUp_Page()
        {
            InitializeComponent();
            random = new Random();
        }

        private async void OnStartTestClicked(object sender, EventArgs e)
        {
            // Создать имя для записи
            var name = NameEntry.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Viga", "Palun sisesta oma nimi.", "OK");
                return;
            }

            // Инициализировать переменные
            questionIndex = 0;
            score = 0;
            numbers = new List<int>();

            // Сгенерировать список случайных чисел для теста
            for (int i = 0; i < 10; i++)
            {
                numbers.Add(random.Next(1, 11));
            }

            // Показать первый вопрос
            await DisplayQuestion();
        }

        private async System.Threading.Tasks.Task DisplayQuestion()
        {
            // первое число для вопроса
            var number1 = numbers[questionIndex];

            // Создать случайное второе число для вопроса
            var number2 = random.Next(1, 11);

            // Вычислить правильный ответ
            var answer = number1 * number2;

            // Показать вопрос
            var response = await DisplayPromptAsync($"Küsimus {questionIndex + 1}", $"Kui palju on {number1} x {number2}?", placeholder: "Vastus");

            await SaveDataToFileAsync("test.txt", $"{number1} x {number2} = {answer}\n");

            // Проверить ответ
            if (int.TryParse(response, out int userAnswer))
            {
                if (userAnswer == answer)
                {
                    score++;
                }
            }

            // Перейти к следующему вопросу или закончить тест
            if (questionIndex < 9)
            {
                questionIndex++;
                await DisplayQuestion();
            }
            else
            {
                var percentageScore = (double)score /10 * 100;

                var letterGrade = GetLetterGrade(percentageScore);

                await DisplayAlert("Test lõpetatud", $"{NameEntry.Text}, sinu punktid {score} / 10. ({percentageScore}%)\nHinne: {letterGrade}", "OK");
            }
        }

        private string GetLetterGrade(double percentageScore)
        {
            if (percentageScore >= 90)
            {
                return "5";
            }
            else if(percentageScore >= 75)
            {
                return "4";
            }
            else if (percentageScore >= 50)
            {
                return "3";
            }
            else
            {
                return "2";
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async Task SaveDataToFileAsync(string fileName, string data)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fullPath = Path.Combine(path, fileName);

            using (StreamWriter sw = new StreamWriter(fullPath, true))
            {
                await sw.WriteAsync(data).ConfigureAwait(false);
            }
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fullPath = Path.Combine(path, "test.txt");

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                    await DisplayAlert("Korras", $"Faili kustutamine õnnestus", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Viga", $"Viga faili kustutamisel: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Viga", "Faili ei leitud", "OK");
            }
        }
    }
}