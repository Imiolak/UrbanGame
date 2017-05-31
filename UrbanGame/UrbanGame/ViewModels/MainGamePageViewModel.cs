using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UrbanGame.Annotations;
using UrbanGame.Database.Models;
using Xamarin.Forms;

namespace UrbanGame.ViewModels
{
    public class MainGamePageViewModel : INotifyPropertyChanged
    {
        public MainGamePageViewModel()
        {
            var gameStartTimestamp = 
                DateTime.Parse(App.Database.GetApplicationVariableByName(ApplicationVariables.GameStartedTimestamp).Value);

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                var gameElapsedTime = (GameEnded ? GameEndedTimeStamp : DateTime.UtcNow) - gameStartTimestamp;

                GameElapsedTime = gameElapsedTime.Hours >= 1
                        ? $"{gameElapsedTime.Hours}:{gameElapsedTime.Minutes}:{gameElapsedTime.Seconds:00}"
                        : $"{gameElapsedTime.Minutes}:{gameElapsedTime.Seconds:00}";

                return !GameEnded;
            });
        }
        
        public int NumberOfObjectives => int.Parse(App.Database
            .GetApplicationVariableByName(ApplicationVariables.NumberOfObjectives).Value);

        public string CompletedObjectives
        {
            get { return $"{App.Database.GetApplicationVariableByName(ApplicationVariables.CompletedObjectives).Value} / {NumberOfObjectives}"; }
            set
            {
                App.Database.SetApplicationVariable(ApplicationVariables.CompletedObjectives, value);
                OnPropertyChanged();
            }
        }
        
        public int CorrectlyAnsweredQuestions
        {
            get { return int.Parse(App.Database.GetApplicationVariableByName(ApplicationVariables.CorrectlyAnsweredQuestions).Value); }
            set
            {
                App.Database.SetApplicationVariable(ApplicationVariables.CorrectlyAnsweredQuestions, value.ToString());
                OnPropertyChanged();
            }
        }
        
        public int IncorrectlyAnsweredQuestions
        {
            get { return int.Parse(App.Database.GetApplicationVariableByName(ApplicationVariables.IncorrectlyAnsweredQustions).Value); }
            set
            {
                App.Database.SetApplicationVariable(ApplicationVariables.IncorrectlyAnsweredQustions, value.ToString());
                OnPropertyChanged();
            }
        }

        private string _gameElapsedTime;
        public string GameElapsedTime
        {
            get { return _gameElapsedTime; }
            set
            {
                _gameElapsedTime = value;
                OnPropertyChanged();
            }
        }

        public bool GameEnded
        {
            get { return bool.Parse(App.Database.GetApplicationVariableByName(ApplicationVariables.GameEnded).Value); }
            set
            {
                App.Database.SetApplicationVariable(ApplicationVariables.GameEnded, value.ToString());
                OnPropertyChanged();
            }
        }

        public DateTime GameEndedTimeStamp
        {
            get
            {
                return
                    DateTime.Parse(
                        App.Database.GetApplicationVariableByName(ApplicationVariables.GameEndedTimestamp).Value);
            }
            set
            {
                App.Database.SetApplicationVariable(ApplicationVariables.GameEndedTimestamp, value.ToString());
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
}
