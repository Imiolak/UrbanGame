using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UrbanGame.Annotations;
using UrbanGame.Database.Models;
using UrbanGame.Exceptions;
using Xamarin.Forms;

namespace UrbanGame.ViewModels
{
    public class MainGameViewModel : INotifyPropertyChanged
    {
        private readonly int _pointsPerMainObjective;
        private readonly int _pointsPerExtraObjective;
        private readonly int _numberOfObjectives;
        private readonly DateTime _gameStartTimestamp;
        
        public MainGameViewModel()
        {
            _pointsPerMainObjective =
                int.Parse(App.Database.GetApplicationVariableByName(ApplicationVariables.PointsPerMainObjective).Value);
            _pointsPerExtraObjective =
                int.Parse(App.Database.GetApplicationVariableByName(ApplicationVariables.PointsPerExtraObjective).Value);
            _gameStartTimestamp =
                DateTime.Parse(App.Database.GetApplicationVariableByName(ApplicationVariables.GameStartedTimestamp).Value);
            _numberOfObjectives =
                int.Parse(App.Database.GetApplicationVariableByName(ApplicationVariables.NumberOfObjectives).Value);

            Objectives = new ObservableCollection<Objective>(App.Database.GetAllObjectives());
            
            Points = CountPoints();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                TimeSpan gameElapsedTime;

                if (!GameEnded)
                {
                    gameElapsedTime = DateTime.UtcNow - _gameStartTimestamp;
                    
                }
                else
                {
                    gameElapsedTime = GameEndedTimeStamp - _gameStartTimestamp;
                }

                GameElapsedTime = gameElapsedTime.Hours >= 1
                        ? $"{gameElapsedTime.Hours}:{gameElapsedTime.Minutes}:{gameElapsedTime.Seconds:00}"
                        : $"{gameElapsedTime.Minutes}:{gameElapsedTime.Seconds:00}";

                return !GameEnded;
            });
        }

        public void AddClue(Clue clue, int numberOfExtraObjectives)
        {
            if (!ClueInCurrentGameScopeOrStageEnding(clue))
            {
                throw new ClueNotInCurrentGameScopeException();
            }

            if (IsMainClue(clue))
            {
                if (IsGameEndingClue(clue))
                {
                    GameEndedTimeStamp = DateTime.UtcNow;
                    GameEnded = true;
                }
                else
                {
                    var currentObjective = App.Database.GetObjectiveForClue(clue.Major);
                    currentObjective.IsStarted = true;
                    currentObjective.NumberOfExtraObjectives = numberOfExtraObjectives;
                    currentObjective.Clues.Add(clue);

                    App.Database.AddClue(clue);
                    App.Database.UpdateObjective(currentObjective);

                    CurrentObjective = clue.Major;
                }

                var previousObjective = App.Database.GetObjectiveForClue(clue.Major - 1);
                previousObjective.IsCompleted = true;

                App.Database.UpdateObjective(previousObjective);
            }
            else
            {
                var objective = App.Database.GetObjectiveForClue(clue.Major);
                objective.Clues.Add(clue);

                App.Database.AddClue(clue);
                App.Database.UpdateObjective(objective);
            }

            Objectives = new ObservableCollection<Objective>(App.Database.GetAllObjectives());
            Points = CountPoints();
        }

        private int CountPoints()
        {
            return Objectives.Count(o => o.IsCompleted) * _pointsPerMainObjective
                   + Objectives.Sum(o => o.ExtraClues.Count()) * _pointsPerExtraObjective;
        }

        #region Conditional helpers
        private bool ClueInCurrentGameScopeOrStageEnding(Clue clue)
        {
            var currentObjecrive = CurrentObjective;

            return clue.Major <= currentObjecrive
                   || (clue.Major == currentObjecrive + 1 && clue.Minor == 0);
        }

        private static bool IsMainClue(Clue clue)
        {
            return clue.Minor == 0;
        }

        private bool IsGameEndingClue(Clue clue)
        {
            return clue.Major == _numberOfObjectives + 1;
        }
        #endregion

        #region ViewModel Properties
        public event PropertyChangedEventHandler PropertyChanged;

        public bool GameEnded
        {
            get { return bool.Parse(App.Database.GetApplicationVariableByName(ApplicationVariables.GameEnded).Value); }
            set
            {
                App.Database.SetApplicationVariable(ApplicationVariables.GameEnded, value.ToString());
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

        private int _points;
        public int Points
        {
            get { return _points; }
            set
            {
                _points = value;
                OnPropertyChanged();
            }
        }
        
        public int CurrentObjective
        {
            get { return int.Parse(App.Database.GetApplicationVariableByName(ApplicationVariables.CurrentObjective).Value); }
            set
            {
                App.Database.SetApplicationVariable(ApplicationVariables.CurrentObjective, value.ToString());
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Objective> _objectives;
        public ObservableCollection<Objective> Objectives
        {
            get { return _objectives; }
            set
            {
                _objectives = value;
                OnPropertyChanged();
            }
        }

        private Clue _mainClue;
        public Clue MainClue { get { return _mainClue; }
            set
            {
                _mainClue = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Clue> _extraClues;
        public ObservableCollection<Clue> ExtraClues { get { return _extraClues; }
            set
            {
                _extraClues = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
