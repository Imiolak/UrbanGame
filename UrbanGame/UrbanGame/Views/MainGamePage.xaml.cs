using System;
using UrbanGame.Database.Models;
using UrbanGame.Exceptions;
using UrbanGame.Game;
using UrbanGame.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace UrbanGame.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainGamePage : ContentPage
    {
        private readonly MainGameViewModel _viewModel;

        public MainGamePage(MainGameViewModel viewModel = null)
        {
            InitializeComponent();

            _viewModel = viewModel ?? new MainGameViewModel();
            BindingContext = _viewModel;
        }
        
        private void ResetProgressButton_Clicked(object sender, EventArgs e)
        {
            App.Database.ClearDb();
            App.Instance.SetNewMainPage(new WelcomePage());
        }

        private void ScanCodeButton_Clicked(object sender, EventArgs e)
        {
            var scanOptions = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                PossibleFormats = { BarcodeFormat.QR_CODE }
            };
            var qrScanPage = new ZXingScannerPage(scanOptions)
            {
                DefaultOverlayTopText = "Zeskanuj kod",
                DefaultOverlayBottomText = "sdrgdsb"
            };

            qrScanPage.OnScanResult += (result) =>
            {
                qrScanPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        Navigation.PopModalAsync();

                        int numberOfExtraObjectives;
                        var clue = new ClueReader().ReadClue(result.Text, out numberOfExtraObjectives);

                        _viewModel.AddClue(clue, numberOfExtraObjectives);
                    }
                    catch (ClueNotInCurrentGameScopeException)
                    {
                        DisplayAlert("Nieobsługiwany kod",
                               "Kod, który zeskanowałeś może być użyty jedynie w późniejszej fazie gry. Znajdź poprzednie kody i wróć aby odblokować ten!",
                               "OK");
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("Niepoprawny kod", ex.ToString(), "Zrozumiałem");
                    }
                });
            };

            Navigation.PushModalAsync(qrScanPage);
        }

        private void CarouselView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ObjectiveDetailsView.BindingContext = ((CarouselView) sender).Item as Objective;
        }
    }
}
