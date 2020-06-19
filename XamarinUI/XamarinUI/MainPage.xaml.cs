using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinUI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        string[] months = {"January", "February", "March", "April", "May",
                    "June", "July", "September", "October", "November", "December"};
        private int currentHour = 0;
        private int currentMinute = 0;
        private int currentSecond = 0;
        private DayOfWeek currentDayOfweek;
        private int currentDay = 0;
        private int currentMonth = 0;
        private int currentYear = 0;

        Random random = new Random();

        public MainPage()
        {
            InitializeComponent();
            imageBackground.Source = "Wallpaper" + random.Next(1, 14) + ".jpg";
            SetDateTime();
            SetTimer();
        }
        private void SetDateTime()
        {
            DateTime localDate = DateTime.Now;
            currentHour = localDate.Hour;
            currentMinute = localDate.Minute;
            currentSecond = localDate.Second;
            currentDayOfweek = localDate.DayOfWeek;
            currentDay = localDate.Day;
            currentMonth = localDate.Month;
            currentYear = localDate.Year;
            labelHour.Text = "" + currentHour;               // int
            labelMinute.Text = "" + currentMinute;           // int
            labelSecond.Text = "" + currentSecond;           // int
            labelDayOfWeek.Text = "" + currentDayOfweek;     // DayOfWeek
            labelDay.Text = ", " + currentDay;                 // int
            labelMonth.Text = " " + months[currentMonth - 1]; // int
            labelYear.Text = " " + currentYear;               // int
        }
        private void SetTimer()
        {
            System.Timers.Timer secondTimer = new System.Timers.Timer();
            secondTimer.Interval = 1000;  // 1 second
            secondTimer.Enabled = true;
            secondTimer.Elapsed += SecondTimerElapsedEvent;
            secondTimer.Start();
        }
        private void SecondTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
            SecondChanged();
        }
        private async void SecondChanged()
        {
            await Task.WhenAll<bool>(
                labelColon.FadeTo(0, 0),
                labelSecond.FadeTo(0, 500),
                labelSecond.TranslateTo(0, -50, 500)
            );
            labelSecond.TranslationY = 50;
            await Device.InvokeOnMainThreadAsync(() =>
            {
                currentSecond++;
                if (currentSecond % 10 == 7)
                {
                    tenSecondEvent(1200, 3000, Easing.Linear);
                }
                if (currentSecond >= 60)
                {
                    oneMinuteEvent(3000);
                    currentSecond = 0;
                    currentMinute++;
                    if (currentMinute >= 60)
                    {
                        currentMinute = 0;
                        currentHour++;
                        if (currentHour >= 24)
                        {
                            SetDateTime();
                        }
                    }
                }
                labelSecond.Text = "" + currentSecond;
                labelMinute.Text = "" + currentMinute;
                labelHour.Text = "" + currentHour;
            });
            await Task.WhenAll<bool>(
                labelColon.FadeTo(1, 0),
                labelSecond.FadeTo(1, 500),
                labelSecond.TranslateTo(0, 0, 500)
            );
            
        }
        private async void tenSecondEvent(double translatePosition, uint length, Easing easing)
        {
            await Task.WhenAny<bool>(
                labelDayOfWeek.TranslateTo(-translatePosition, 0, length, easing),
                labelDay.TranslateTo(-translatePosition, 0, length, easing),
                labelMonth.TranslateTo(-translatePosition, 0, length, easing),
                labelYear.TranslateTo(-translatePosition, 0, length, easing)
            );
            labelDayOfWeek.TranslationX = translatePosition;
            labelDay.TranslationX = translatePosition;
            labelMonth.TranslationX = translatePosition;
            labelYear.TranslationX = translatePosition;

            await Task.WhenAny<bool>(
                labelDayOfWeek.TranslateTo(0, 0, length, easing),
                labelDay.TranslateTo(0, 0, length, easing),
                labelMonth.TranslateTo(0, 0, length, easing),
                labelYear.TranslateTo(0, 0, length, easing)
            );
        }
        private async void oneMinuteEvent(uint length)
        {
            imageBackground.Opacity = 1;
            await imageBackground.FadeTo(0, length);
            await Device.InvokeOnMainThreadAsync(() =>
            {
                imageBackground.Source = "Wallpaper" + random.Next(1, 14) + ".jpg";
            });
            await imageBackground.FadeTo(1, length);
        }
    }
}