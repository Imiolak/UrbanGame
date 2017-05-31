using System.Text.RegularExpressions;
using UrbanGame.Database.Models;

namespace UrbanGame.Game.Codes
{
    public class TextObjectiveCodeInterpreter : ObjectiveCodeInterpreterBase
    {
        private const string DetailedObjectiveCodePattern = @"\[T\]\[(.+?)\]";

        protected override string ObjectiveCodePattern => @"(\[T\]\[.+?\])(.*)";

        protected override void InterpretInner(string objectiveCode)
        {
            var regex = new Regex(DetailedObjectiveCodePattern);
            var match = regex.Match(objectiveCode);
            var text = match.Groups[1].Value;

            CurrentlyInterpretedOrbjective.ObjectiveSteps.Add(new TextObjectiveStep
            {
                Text = text,
                IsStarted = false,
                IsCompleted = false
            });
        }
    }
}