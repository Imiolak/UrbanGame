using System.Text.RegularExpressions;

namespace UrbanGame.Game.Codes
{
    public class ObjectiveCodeInterpreter : ObjectiveCodeInterpreterBase
    {
        private const string DetailedObjectiveCodePattern = @"\[(\d+)\]";

        protected override string ObjectiveCodePattern => @"(\[\d+\])(.*)";

        protected override void InterpretInner(string objectiveCode)
        {
            var regex = new Regex(DetailedObjectiveCodePattern);
            var match = regex.Match(objectiveCode);
            var objectiveNo = int.Parse(match.Groups[1].Value);

            CurrentlyInterpretedOrbjective = App.Database.GetObjectiveByObjectiveNo(objectiveNo);
        }
    }
}