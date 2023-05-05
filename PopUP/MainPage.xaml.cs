using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PopUP
{   // пространство имен
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {   // инициализация списков страниц и строк
        List<ContentPage> pages = new List<ContentPage>() { new PopUp_Page() };
        List<string> tekstid = new List<string> { "Ava PopUP leht" };

        public MainPage()  // конструктор класса
        {
            StackLayout st = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.Aqua // создание StackLayout с вертикальной ориентацией и цветом фона Aqua
            };
            for (int i = 0; i < pages.Count; i++) // создание кнопок и добавление их в StackLayout
            {
                Button button = new Button
                {
                    Text = tekstid[i],
                    TabIndex = i,
                    BackgroundColor = Color.Fuchsia,
                    TextColor = Color.Black
                };
                st.Children.Add(button);
                button.Clicked += Navig_funktsion;
            }
            Content = st;
        }

        private async void Navig_funktsion(object sender, EventArgs e)
        {
            Button btn = sender as Button; //(Button)sender
            await Navigation.PushAsync(pages[btn.TabIndex]);
        }
    }
}