using System.Text.RegularExpressions;
using UrbanGame.Database.Models;

namespace UrbanGame.Game.Codes
{
    public class GameEndingObjectiveCodeInterpreter : ObjectiveCodeInterpreterBase
    {
        private const string DetailedObjectiveCodePattern = @"\[E\]\[(.+?)\]";

        protected override string ObjectiveCodePattern => @"(\[E\]\[.+?\])(.*)";

        protected override void InterpretInner(string objectiveCode)
        {
            var regex = new Regex(DetailedObjectiveCodePattern);
            var match = regex.Match(objectiveCode);
            var text = match.Groups[1].Value;

            CurrentlyInterpretedOrbjective.ObjectiveSteps.Add(new EndGameObjectiveStep
            {
                EndGameMessageText = text
            });
        }
    }
}
