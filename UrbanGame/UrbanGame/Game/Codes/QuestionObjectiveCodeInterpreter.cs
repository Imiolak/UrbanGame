using System.Text.RegularExpressions;
using UrbanGame.Database.Models;

namespace UrbanGame.Game.Codes
{
    public class QuestionObjectiveCodeInterpreter : ObjectiveCodeInterpreterBase
    {
        private const string DetailedObjectiveCodePattern = @"\[Q\]\[(.+?)\]\[(.+?)\]";

        protected override string ObjectiveCodePattern => @"(\[Q\]\[.+?\]\[.+?\])(.*)";

        protected override void InterpretInner(string objectiveCode)
        {
            var regex = new Regex(DetailedObjectiveCodePattern);
            var match = regex.Match(objectiveCode);
            var question = match.Groups[1].Value;
            var answersString = match.Groups[2].Value;

            CurrentlyInterpretedOrbjective.ObjectiveSteps.Add(new QuestionObjectiveStep
            {
                Question = question,
                AnswersString = answersString
            });
        }
    }
}