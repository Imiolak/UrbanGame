using SQLite.Net;
using System;
using System.Collections.ObjectModel;
using UrbanGame.Database.Models;
using UrbanGame.Exceptions;
using UrbanGame.Game;
using UrbanGame.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;
using Result = SQLite.Net.Interop.Result;

namespace UrbanGame.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainGamePage : ContentPage
    {
        public MainGamePage()
        {
            InitializeComponent();
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
                DefaultOverlayBottomText = "Pls"
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
                        string objectiveName, imageUrl, detailsPageUrl;
                        
                        var clue = new ClueReader().ReadClue(result.Text, out numberOfExtraObjectives, out objectiveName, out imageUrl, out detailsPageUrl);

                        ((MainGameViewModel) BindingContext).AddClue(clue, numberOfExtraObjectives, objectiveName, imageUrl, detailsPageUrl);
                    }
                    catch (InvalidClueCodeException)
                    {
                        DisplayAlert("Niepoprawny kod",
                            "Format kodu jest niepoprawny. Skontaktuj się z organizatorem gry i poinformuj go o tym!",
                            "OK");
                    }
                    catch (ClueNotInCurrentGameScopeException)
                    {
                        DisplayAlert("Nieobsługiwany kod",
                            "Kod, który zeskanowałeś może być użyty jedynie w późniejszej fazie gry. Znajdź poprzednie kody i wróć aby odblokować ten!",
                            "OK");
                    }
                    catch (SQLiteException ex)
                    {
                        if (ex.Result == Result.Constraint)
                        {
                            DisplayAlert("Powtórzony kod",
                                "Zeskanowałeś już wcześniej ten kod!",
                                "OK");
                        }
                        else
                        {
                            DisplayAlert("Błąd bazy",
                                $"Coś poszło nie tak przy zapisywaniu kodu do bazy: {ex}",
                                "OK");
                        }

                        
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("Niepoprawny kod", ex.ToString(), "OK");
                    }
                });
            };

            Navigation.PushModalAsync(qrScanPage);
        }

        private void CarouselView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var objective = ((CarouselView) sender).Item as Objective;

            ((MainGameViewModel)BindingContext).MainClue = objective.MainClue;
            ((MainGameViewModel)BindingContext).ExtraClues = new ObservableCollection<Clue>(objective.ExtraClues);
            ((MainGameViewModel)BindingContext).ImageSource = objective.ImageSource;
            ((MainGameViewModel)BindingContext).DetailsPageUrl = objective.DetailsPageUrl;
            ((MainGameViewModel)BindingContext).ObjectiveCompleted = objective.IsCompleted;
            
            //remove when biding repaired
            ExtraCluesView.Children.Clear();
            foreach (var extraClue in ((MainGameViewModel)BindingContext).ExtraClues)
            {
                ExtraCluesView.Children.Add(new Label
                {
                    Text = $"{extraClue.Minor}. {extraClue.Content}"
                });    
            }
        }
    }
}
