using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UrbanGame.Database.Models;
using UrbanGame.Exceptions;

namespace UrbanGame.Game
{
    public class ClueReader
    {
        private const string StartingCodePattern = @"\[(\d+)\]\[(\d+)\]\[(\d+)\]\[(\d+)\](.*)";
        private const string MainObjectiveCodePattern = @"\[(\d+)\]\[0\]\[(\d+)\]\[(.*)\](.*)";
        private const string ExtraObjectiveCodePatetrn = @"\[(\d+)\]\[(\d+)\](.*)";

        public GameStructure ReadStartingCode(string clueCode)
        {
            var regex = new Regex(StartingCodePattern);
            var match = regex.Match(clueCode);

            try
            {
                var numberOfObjectives = int.Parse(match.Groups[1].Value);

                var objectives = new List<Objective>
                {
                    new Objective
                    {
                        ObjectiveNo = 1,
                        IsStarted = true,
                        IsCompleted = false,
                        NumberOfExtraObjectives = int.Parse(match.Groups[4].Value),
                        Clues = new List<Clue>
                        {
                            new Clue
                            {
                                Major = 1,
                                Minor = 0,
                                Content = match.Groups[5].Value.Trim()
                            }
                        }
                    }
                };

                for (var i = 2; i <= numberOfObjectives; i++)
                {
                    objectives.Add(new Objective
                    {
                        ObjectiveNo = i,
                        IsStarted = false,
                        IsCompleted = false,
                        NumberOfExtraObjectives = 0,
                        Clues = new List<Clue>()
                    });
                }

                var gameStructure = new GameStructure
                {
                    NumberOfObjectives = numberOfObjectives,
                    CurrentObjective = 1,
                    PointsPerMainObjective = int.Parse(match.Groups[2].Value),
                    PointsPerExtraObjective = int.Parse(match.Groups[3].Value),
                    Objectives = objectives
                };

                return gameStructure;
            }
            catch (Exception e)
            {
                throw new InvalidClueCodeException(e);
            }
        }

        public Clue ReadClue(string clueCode, out int numberOfExtraObjectives, out string imageUrl)
        {
            numberOfExtraObjectives = 0;
            imageUrl = "";
            var match = Regex.Match(clueCode, MainObjectiveCodePattern);

            if (match.Captures.Count == 1)
            {
                var objectiveNo = int.Parse(match.Groups[1].Value);
                numberOfExtraObjectives = int.Parse(match.Groups[2].Value);
                imageUrl = match.Groups[3].Value.Trim();

                return new Clue
                {
                    Major = objectiveNo,
                    Minor = 0,
                    Content = match.Groups[4].Value.Trim()
                };
            }

            match = Regex.Match(clueCode, ExtraObjectiveCodePatetrn);

            if (match.Captures.Count != 1)
            {
                throw new InvalidClueCodeException();
            }

            return new Clue
            {
                Major = int.Parse(match.Groups[1].Value),
                Minor = int.Parse(match.Groups[2].Value),
                Content = match.Groups[3].Value.Trim()
            };
        }
    }
}
