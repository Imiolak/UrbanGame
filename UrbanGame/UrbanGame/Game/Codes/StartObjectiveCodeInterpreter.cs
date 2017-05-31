using System.Text.RegularExpressions;
using UrbanGame.Database.Models;

namespace UrbanGame.Game.Codes
{
    public class StartObjectiveCodeInterpreter : ObjectiveCodeInterpreterBase
    {
        private const string DetailedObjectiveCodePattern = @"\[S\]\[(\d+)\]";

        protected override string ObjectiveCodePattern => @"(\[S\]\[\d+\])(.*)";

        protected override void InterpretInner(string objectiveCode)
        {
            var regex = new Regex(DetailedObjectiveCodePattern);
            var match = regex.Match(objectiveCode);

            var numberOfObjectives = match.Groups[1].Value;

            App.Database.SetApplicationVariable(ApplicationVariables.NumberOfObjectives, numberOfObjectives);
            App.Database.InsertEmptyObjectives(int.Parse(numberOfObjectives));
        }
    }
}
