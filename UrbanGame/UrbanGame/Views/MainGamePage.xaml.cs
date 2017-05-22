﻿using SQLite.Net;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
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

            var assembly = typeof(EmbeddedImages).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
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
                        string objectiveName;
                        string imageUrl;
                        var clue = new ClueReader().ReadClue(result.Text, out numberOfExtraObjectives, out objectiveName, out imageUrl);

                        ((MainGameViewModel) BindingContext).AddClue(clue, numberOfExtraObjectives, objectiveName, imageUrl);
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

    public class EmbeddedImages : ContentPage
    {
        public EmbeddedImages()
        {
            var embeddedImage = new Image { Aspect = Aspect.AspectFit };

            // resource identifiers start with assembly-name DOT filename
            embeddedImage.Source = ImageSource.FromResource("UrbanGame.Resources.emptyImage.png");

            Content = new StackLayout
            {
                Children = {
                new Label {
                    Text = "ImageSource.FromResource",
                    FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
                    FontAttributes = FontAttributes.Bold
                },
                embeddedImage,
                new Label { Text = "WorkingWithImages.beach.jpg embedded resource" }
            },
                Padding = new Thickness(0, 20, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            // NOTE: use for debugging, not in released app code!
            //var assembly = typeof(EmbeddedImages).GetTypeInfo().Assembly;
            //foreach (var res in assembly.GetManifestResourceNames()) 
            //	System.Diagnostics.Debug.WriteLine("found resource: " + res);
        }
    }
}
