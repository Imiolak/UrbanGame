using System;
using UrbanGame.Database.Models;
using UrbanGame.Game;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace UrbanGame.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
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

                        var startingGameStructire = new ClueReader().ReadStartingCode(result.Text);

                        App.Database.SetApplicationVariable(ApplicationVariables.GameStarted, 
                            true.ToString());
                        App.Database.SetApplicationVariable(ApplicationVariables.GameEnded, 
                            false.ToString());
                        App.Database.SetApplicationVariable(ApplicationVariables.GameStartedTimestamp,
                            DateTime.UtcNow.ToString());
                        App.Database.SetApplicationVariable(ApplicationVariables.CurrentObjective,
                            1.ToString());
                        App.Database.SetApplicationVariable(ApplicationVariables.NumberOfObjectives,
                            startingGameStructire.NumberOfObjectives.ToString());
                        App.Database.SetApplicationVariable(ApplicationVariables.PointsPerMainObjective,
                            startingGameStructire.PointsPerMainObjective.ToString());
                        App.Database.SetApplicationVariable(ApplicationVariables.PointsPerExtraObjective,
                            startingGameStructire.PointPerExtraObjective.ToString());

                        foreach (var objective in startingGameStructire.Objectives)
                        {
                            App.Database.AddObjective(objective);
                        }

                        App.Instance.SetNewMainPage(new MainGamePage());
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("Niepoprawny kod", ex.ToString(), "Zrozumiałem");
                    }
                });
            };

            await Navigation.PushModalAsync(qrScanPage);
        }

        private void HowToPlayButton_Clicked(object sender, EventArgs e)
        {
            
        }

        private void ResetDbButton_Clicked(object sender, EventArgs e)
        {
            App.Database.ClearDb();
        }
    }
}
