using MultipleImageApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MultipleImageApp
{
    public partial class App : Application
    {
        public App(IMultiMediaPickerService multiMediaPickerService)
        {
            InitializeComponent();

            MainPage = new MainPage( multiMediaPickerService);
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
