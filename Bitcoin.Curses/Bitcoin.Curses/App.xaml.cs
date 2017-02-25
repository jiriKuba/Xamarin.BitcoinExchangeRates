using Bitcoin.Curses.Messages;
using Bitcoin.Curses.Services;
using Bitcoin.Curses.Services.Interfaces;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Bitcoin.Curses
{
    public partial class App : Application
    {
        public static App Instance { get; private set; }

        public App()
        {
            Instance = this;

            this.InitializeComponent();

            //register exception handler
            Messenger.Default.Register<ExceptionMessage>(this,(action) => ReceiveExceptionMessage(action));

            this.MainPage = new MainPage();
        }

        private void ReceiveExceptionMessage(ExceptionMessage message)
        {
            DisplayAlert("Alert", message.ThrowedException.Message);
        }

        public async void DisplayAlert(String header, String message)
        {
            Page page = this.MainPage;
            if (page != null)
            {
                await page.DisplayAlert(header, message, "Ok");
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
