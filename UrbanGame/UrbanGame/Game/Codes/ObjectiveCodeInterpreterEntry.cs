using System.Text.RegularExpressions;

namespace UrbanGame.Game.Codes
{
    public class ObjectiveCodeInterpreterEntry : IObjectiveCodeInterpreter
    {
        protected string ObjectiveCodePattern => @"\[(.+?)\].*";

        public void Interpret(string objectiveCode)
        {
            var regex = new Regex(ObjectiveCodePattern);
            var match = regex.Match(objectiveCode);
            var initialObjectiveSymbol = match.Groups[1].Value;

            var interpreter = ObjectiveCodeInterpreterFactory.CreateInterpreterFromCodeSymbol(initialObjectiveSymbol);
            interpreter.Interpret(objectiveCode);
        }
    }
}