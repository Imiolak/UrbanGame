using System.Globalization;
using System.Text.RegularExpressions;
using UrbanGame.Database.Models;

namespace UrbanGame.Game.Codes
{
    public class GoToPointObjectiveCodeInterpreter : ObjectiveCodeInterpreterBase
    {
        private const string DetailedObjectiveCodePattern = @"\[STP\]\[(.+?)\]\[(\d+\.\d+)\]\[(\d+\.\d+)\]";

        protected override string ObjectiveCodePattern => @"(\[STP\]\[.+?\]\[\d+\.\d+\]\[\d+\.\d+\])(.*)";

        protected override void InterpretInner(string objectiveCode)
        {
            var regex = new Regex(DetailedObjectiveCodePattern);
            var match = regex.Match(objectiveCode);

            var objectiveText = match.Groups[1].Value;
            var latitude = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
            var longitude = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

            CurrentlyInterpretedOrbjective.ObjectiveSteps.Add(new GoToLocationObjectiveStep
            {
                ObjectiveId = CurrentlyInterpretedOrbjective.Id,
                Text = objectiveText,
                Latitude = latitude,
                Longitude = longitude
            });
        }
    }
}