using System.Text.RegularExpressions;
using UrbanGame.Database.Models;
using UrbanGame.ViewModels;
using Xamarin.Forms;

namespace UrbanGame.Game.Codes
{
    public abstract class ObjectiveCodeInterpreterBase : IObjectiveCodeInterpreter
    {
        protected static Objective CurrentlyInterpretedOrbjective;
        private const string ObjectiveSymbolPattern = @"\[(.+?)\](.*)";
        
        protected abstract string ObjectiveCodePattern { get; }
        
        public void Interpret(string objectiveCode)
        {
            var regex = new Regex(ObjectiveCodePattern);
            var match = regex.Match(objectiveCode);
            var currentObjectiveCode = match.Groups[1].Value;
            var remainingObjectiveCode = match.Groups[2].Value;

            InterpretInner(currentObjectiveCode);

            if (!string.IsNullOrWhiteSpace(remainingObjectiveCode))
            {
                var objectiveSymbol = ExtractObjectiveSymbol(remainingObjectiveCode);
                var codeInterpreter = ObjectiveCodeInterpreterFactory.CreateInterpreterFromCodeSymbol(objectiveSymbol);

                codeInterpreter.Interpret(remainingObjectiveCode);
            }
            else
            {
                FinalizeObjectiveInterpretation();
            }
        }
        
        protected abstract void InterpretInner(string objectiveCode);

        private static string ExtractObjectiveSymbol(string objectiveCode)
        {
            var regex = new Regex(ObjectiveSymbolPattern);
            var match = regex.Match(objectiveCode);

            return match.Groups[1].Value;
        }

        private static void FinalizeObjectiveInterpretation()
        {
            if (CurrentlyInterpretedOrbjective.ObjectiveNo > 1)
            {
                var previousObjective = App.Database.GetObjectiveByObjectiveNo(CurrentlyInterpretedOrbjective.ObjectiveNo - 1);
                previousObjective.IsCompleted = true;
                App.Database.UpdateObjective(previousObjective);
                ((MainGamePageViewModel) Application.Current.MainPage.BindingContext).CompletedObjectives =
                    previousObjective.ObjectiveNo.ToString();
            }

            for (var i = 0; i < CurrentlyInterpretedOrbjective.ObjectiveSteps.Count; i++)
            {
                var objectiveStep = CurrentlyInterpretedOrbjective.ObjectiveSteps[i];

                objectiveStep.ObjectiveId = CurrentlyInterpretedOrbjective.Id;
                objectiveStep.OrderInObjective = i;

                if (i == 0)
                {
                    objectiveStep.IsStarted = true;
                }

                App.Database.AddObjectiveStep(objectiveStep);
            }

            CurrentlyInterpretedOrbjective.IsStarted = true;
            App.Database.UpdateObjective(CurrentlyInterpretedOrbjective);
        }
    }
}
