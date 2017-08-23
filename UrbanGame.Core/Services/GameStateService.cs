using UrbanGame.Core.Models;

namespace UrbanGame.Core.Services
{
    public class GameStateService : IGameStateService
    {
        private readonly IApplicationVariableService _applicationVariableService;
        private readonly IObjectiveService _objectiveService;

        public GameStateService(IApplicationVariableService applicationVariableService, IObjectiveService objectiveService)
        {
            _applicationVariableService = applicationVariableService;
            _objectiveService = objectiveService;
        }

        public bool GetGameStarted()
        {
            var gameStarted = _applicationVariableService.GetValueByKey("GameStarted");
            return gameStarted != null && bool.Parse(gameStarted);
        }

        public void StartGame()
        {
            _applicationVariableService.SetValue("GameStarted", true.ToString());
            _applicationVariableService.SetValue("NumberOfObjectives", 3.ToString());
            _applicationVariableService.SetValue("CurrentActiveObjective", 1.ToString());

            //TODO debug, remove later
            _objectiveService.AddObjective(new Objective
            {
                ObjectiveNo = 1,
                ObjectiveHeader = "pierwszy"
            });
            _objectiveService.AddObjectiveStep(new ObjectiveStep
            {
                Text = "no 1 s1",
                ObjectiveNo = 1,
                OrderInObjective = 1
            });
            _objectiveService.AddObjectiveStep(new ObjectiveStep
            {
                Text = "no 1 s2",
                ObjectiveNo = 1,
                OrderInObjective = 2
            });
            _objectiveService.AddObjective(new Objective
            {
                ObjectiveNo = 2,
                ObjectiveHeader = "drugi"
            });
            _objectiveService.AddObjectiveStep(new ObjectiveStep
            {
                Text = "no 2 s1",
                ObjectiveNo = 2,
                OrderInObjective = 1
            });
            _objectiveService.AddObjective(new Objective
            {
                ObjectiveNo = 3,
                ObjectiveHeader = "trzeci"
            });
            _objectiveService.AddObjectiveStep(new ObjectiveStep
            {
                Text = "no 3 s1",
                ObjectiveNo = 3,
                OrderInObjective = 1
            });
            _objectiveService.AddObjectiveStep(new ObjectiveStep
            {
                Text = "no 3 s2",
                ObjectiveNo = 3,
                OrderInObjective = 2
            });
            _objectiveService.AddObjectiveStep(new ObjectiveStep
            {
                Text = "no 3 s3",
                ObjectiveNo = 3,
                OrderInObjective = 3
            });
        }

        public void ResetGame()
        {
            _applicationVariableService.SetValue("GameStarted", false.ToString());
            _objectiveService.RemoveAllObjectives();
        }

        public int GetNumberOfObjectives()
        {
            return int.Parse(_applicationVariableService.GetValueByKey("NumberOfObjectives"));
        }

        public int GetCurrentActiveObjectiveNo()
        {
            return int.Parse(_applicationVariableService.GetValueByKey("CurrentActiveObjective"));
        }

        public void AdvanceToNextObjective()
        {
            throw new System.NotImplementedException();
        }
    }
}
