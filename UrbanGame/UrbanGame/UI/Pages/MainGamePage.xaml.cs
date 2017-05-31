using Plugin.Geolocator;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UrbanGame.Annotations;
using UrbanGame.Database.Models;
using UrbanGame.Game.Codes;
using UrbanGame.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;
using Result = SQLite.Net.Interop.Result;

namespace UrbanGame.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainGamePage : ContentPage
    {
        private MainGamePageViewModel ViewModel => (MainGamePageViewModel) BindingContext;

        public MainGamePage()
        {
            InitializeComponent();
            PopulatePageContent();
        }
        
        private void PopulatePageContent()
        {
            var activeObjectiveStep = App.Database.GetActiveObjectiveStep();
            CreateViewBasedOnObjecriveStepType(activeObjectiveStep);
        }

        private void CreateViewBasedOnObjecriveStepType(ObjectiveStep activeObjectiveStep)
        {
            if (activeObjectiveStep.ObjectiveStepType == typeof(EndGameObjectiveStep).FullName)
            {
                if (!ViewModel.GameEnded)
                {
                    ViewModel.GameEnded = true;
                    ViewModel.GameEndedTimeStamp = DateTime.UtcNow;

                    var notCompletedObjetives = App.Database.GetAllObjectives()
                        .Where(obj => !obj.IsCompleted);
                    foreach (var objective in notCompletedObjetives)
                    {
                        objective.IsCompleted = true;
                        App.Database.UpdateObjective(objective);
                        ViewModel.CompletedObjectives = ViewModel.NumberOfObjectives.ToString();
                    }
                }
                CreateViewForEndGameObjectiveStep(activeObjectiveStep as EndGameObjectiveStep);
            }
            if (activeObjectiveStep.ObjectiveStepType == typeof(GoToLocationObjectiveStep).FullName)
            {
                CreateViewForGoToLocationObjectiveStep(activeObjectiveStep as GoToLocationObjectiveStep);
            }
            else if (activeObjectiveStep.ObjectiveStepType == typeof(TextObjectiveStep).FullName)
            {
                CreateViewForTextObjectiveStep(activeObjectiveStep as TextObjectiveStep);
            }
            else if (activeObjectiveStep.ObjectiveStepType == typeof(QuestionObjectiveStep).FullName)
            {
                CreateViewForQuestionObjectiveStep(activeObjectiveStep as QuestionObjectiveStep);
            }
        }

        private void CreateViewForEndGameObjectiveStep(EndGameObjectiveStep endGameObjectiveStep)
        {
            var endGameTextLabel = new Label
            {
                Text = endGameObjectiveStep.EndGameMessageText,
                LineBreakMode = LineBreakMode.WordWrap,
                HorizontalOptions = LayoutOptions.Fill
            };

            PageContentFrame.Content = endGameTextLabel;
        }

        private void CreateViewForGoToLocationObjectiveStep(GoToLocationObjectiveStep goToLocationObjectiveStep)
        {
            var textLabel = new Label
            {
                Text = goToLocationObjectiveStep.Text,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Fill
            };

            var geoLocator = CrossGeolocator.Current;
            var map = new Map
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                IsShowingUser = true,
                MapType = MapType.Street
            };
            map.Pins.Add(new Pin
            {
                Label = "Tutaj!",
                Position = new Position(goToLocationObjectiveStep.Latitude, goToLocationObjectiveStep.Longitude)
            });
            geoLocator.StartListeningAsync(TimeSpan.FromSeconds(1).Seconds, 1);
            geoLocator.PositionChanged += (sender, e) =>
            {
                var currentPosition = new Position(e.Position.Latitude, e.Position.Longitude);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(currentPosition, Distance.FromKilometers(3)));

                geoLocator.StopListeningAsync();
            };
            
            var scanCodeButton = new Button
            {
                Text = "Skanuj kod",
                BackgroundColor = Color.Green,
                VerticalOptions = LayoutOptions.End
            };
            scanCodeButton.Clicked += (sender, args) =>
            {
                App.Database.CompleteObjectiveStep(goToLocationObjectiveStep);
                ScanCodeButton_Clicked(sender, args);
            };

            PageContentFrame.Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    textLabel,
                    map,
                    scanCodeButton
                }
            };
        }

        private void CreateViewForTextObjectiveStep(TextObjectiveStep textObjectiveStep)
        {
            var textLabel = new Label
            {
                Text = textObjectiveStep.Text,
                LineBreakMode = LineBreakMode.CharacterWrap,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.Fill
            };

            var confirmReadTextButton = new Button
            {
                Text = "Przeczytałem!",
                BackgroundColor = Color.Green,
                VerticalOptions = LayoutOptions.End
            };
            confirmReadTextButton.Clicked += (sender, args) =>
            {
                App.Database.CompleteObjectiveStep(textObjectiveStep);
                App.Database.StartNextObjectiveStep();
                PopulatePageContent();
            };

            PageContentFrame.Content = new ScrollView
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        textLabel,
                        confirmReadTextButton
                    }
                }
            };
        }

        private void CreateViewForQuestionObjectiveStep(QuestionObjectiveStep questionObjectiveStep)
        {
            var questionLabel = new Label
            {
                Text = questionObjectiveStep.Question,
                LineBreakMode = LineBreakMode.CharacterWrap,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Fill
            };

            var answers = questionObjectiveStep.AnswersString.Split(';')
                .Where(answer => !string.IsNullOrWhiteSpace(answer))
                .Select(answer => new Answer { AnswerText = answer })
                .ToList();
            answers.First().IsCorrect = true;
            answers.Shuffle();

            var answersListView = new ListView
            {
                VerticalOptions = LayoutOptions.Fill,
                ItemsSource = answers,
                ItemTemplate = new DataTemplate(() =>
                {
                    var label = new Label
                    {
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center
                    };
                    label.SetBinding(Label.TextProperty, "AnswerText");

                    var frame = new Frame
                    {
                        HorizontalOptions = LayoutOptions.Fill,
                        Content = new StackLayout
                        {
                            HorizontalOptions = LayoutOptions.Fill,
                            VerticalOptions = LayoutOptions.Fill,
                            Children = { label }
                        } 
                    };
                    frame.SetBinding(BackgroundColorProperty, "BackgroundColor");
                    return new ViewCell { View = frame };
                })
            };
            EventHandler<ItemTappedEventArgs> answersListViewItemTapped = (sender, args) =>
            {
                var selectedAnswer = (Answer)args.Item;
                selectedAnswer.BackgroundColor = Color.Orange;

                foreach (var answer in answers)
                {
                    if (answer != selectedAnswer)
                    {
                        answer.BackgroundColor = Color.LightGray;
                    }
                }
            };
            answersListView.ItemTapped += answersListViewItemTapped;
            
            var confirmAnswerButton = new Button
            {
                Text = "Potwierdź odpowiedź",
                VerticalOptions = LayoutOptions.End,
                IsVisible = true
            };
            var proceedButton = new Button
            {
                Text = "Przejdź dalej",
                BackgroundColor = Color.Green,
                VerticalOptions = LayoutOptions.End,
                IsVisible = false
            };

            confirmAnswerButton.Clicked += (sender, args) =>
            {
                var selecetdAnswer = answersListView.SelectedItem as Answer;
                if (selecetdAnswer == null)
                    return;

                answersListView.ItemTapped -= answersListViewItemTapped;

                if (selecetdAnswer.IsCorrect)
                {
                    selecetdAnswer.BackgroundColor = Color.Green;
                    ViewModel.CorrectlyAnsweredQuestions++;
                }
                else
                {
                    selecetdAnswer.BackgroundColor = Color.Red;
                    answers.Single(ans => ans.IsCorrect).BackgroundColor = Color.Green;
                    ViewModel.IncorrectlyAnsweredQuestions++;
                }
                
                confirmAnswerButton.IsVisible = false;
                proceedButton.IsVisible = true;
            };
            proceedButton.Clicked += (sender, args) =>
            {
                App.Database.CompleteObjectiveStep(questionObjectiveStep);
                App.Database.StartNextObjectiveStep();
                PopulatePageContent();
            };

            PageContentFrame.Content = new ScrollView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        questionLabel,
                        answersListView,
                        confirmAnswerButton,
                        proceedButton
                    }
                }
            };
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
                DefaultOverlayBottomText = ""
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

                        PopulatePageContent();
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
                        DisplayAlert("Niepoprawny kod", ex.Message, "OK");
                    }
                });
            };

            Navigation.PushModalAsync(qrScanPage);
        }
    }

    public static class ListExtenstions
    {
        public static void Shuffle<T>(this List<T> list)
        {
            var rnd = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rnd.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    internal class Answer : INotifyPropertyChanged
    {
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        private Color _backgroundColor = Color.LightGray;
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class IsSelectedToBackgoundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isSelected = (bool) value;

            return isSelected
                ? Color.Orange
                : Color.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
