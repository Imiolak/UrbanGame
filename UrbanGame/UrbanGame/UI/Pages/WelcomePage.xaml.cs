using System;
using UrbanGame.Database.Models;
using UrbanGame.Exceptions;
using UrbanGame.Game.Codes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace UrbanGame.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            WelcomePageBackgroundImage.Source = ImageSource.FromResource("UrbanGame.Resources.background.jpg");
        }

        private async void StartGameButton_Clicked(object sender, EventArgs e)
        {
            var playerDecision = await DisplayAlert("Rozpoczęcie gry",
                "Zamierzasz właśnie rozpocząć grę. Będziesz musiał zeskanować kod startowy. Po zeskanowaniu kodu rozpocznie się odliczanie czasu od startu gry. Jesteś pewien że chcesz już zacząć?",
                "Grajmy!",
                "Jeszcze nie");

            if (!playerDecision)
            {
                return;
            }

            var scanOptions = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                PossibleFormats = { BarcodeFormat.QR_CODE }
            };
            var qrScanPage = new ZXingScannerPage(scanOptions)
            {
                DefaultOverlayTopText = "Zeskanuj kod startowy",
                DefaultOverlayBottomText = "Po zeskanowaniu kodu rozpocznie się gra"
            };

            qrScanPage.OnScanResult += (result) =>
            {
                qrScanPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        Navigation.PopModalAsync();

                        ObjectiveCodeInterpreterFactory.CreateInterpreterEntryPoint().Interpret(result.Text);

                        App.Database.SetApplicationVariable(ApplicationVariables.GameStarted, true.ToString());
                        App.Database.SetApplicationVariable(ApplicationVariables.GameEnded, false.ToString());
                        App.Database.SetApplicationVariable(ApplicationVariables.CompletedObjectives, 0.ToString());
                        App.Database.SetApplicationVariable(ApplicationVariables.CorrectlyAnsweredQuestions, 0.ToString());
                        App.Database.SetApplicationVariable(ApplicationVariables.IncorrectlyAnsweredQustions, 0.ToString());
                        App.Database.SetApplicationVariable(ApplicationVariables.GameStartedTimestamp, DateTime.UtcNow.ToString());
                        App.Instance.SetNewMainPage(new MainGamePage());
                    }
                    catch (InvalidObjectiveCodeException)
                    {
                        DisplayAlert("Niepoprawny kod",
                            "Format kodu jest niepoprawny. Skontaktuj się z organizatorem gry i poinformuj go o tym!",
                            "OK");
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("Niepoprawny kod", ex.ToString(), "Zrozumiałem");
                    }
                });
            };

            await Navigation.PushModalAsync(qrScanPage);
        }
    }
}
