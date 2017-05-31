using UrbanGame.Exceptions;

namespace UrbanGame.Game.Codes
{
    public static class ObjectiveCodeInterpreterFactory
    {
        public static IObjectiveCodeInterpreter CreateInterpreterEntryPoint()
        {
            return new ObjectiveCodeInterpreterEntry();
        }

        public static IObjectiveCodeInterpreter CreateInterpreterFromCodeSymbol(string codeSymbol)
        {
            int objectiveNo;
            if (int.TryParse(codeSymbol, out objectiveNo))
            {
                return new ObjectiveCodeInterpreter();
            }

            switch (codeSymbol)
            {
                case "S":
                    return new StartObjectiveCodeInterpreter();
                case "STP":
                    return new GoToPointObjectiveCodeInterpreter();
                case "T":
                    return new TextObjectiveCodeInterpreter();
                case "Q":
                    return new QuestionObjectiveCodeInterpreter();
                case "E":
                    return new GameEndingObjectiveCodeInterpreter();
                default:
                    throw new InvalidObjectiveCodeException();
            }
        }
    }
}
