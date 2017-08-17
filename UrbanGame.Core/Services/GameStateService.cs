namespace UrbanGame.Core.Services
{
    public class GameStateService : IGameStateService
    {
        private readonly IApplicationVariableService _applicationVariableService;

        public GameStateService(IApplicationVariableService applicationVariableService)
        {
            _applicationVariableService = applicationVariableService;
        }

        public bool GetGameStarted()
        {
            var gameStarted = _applicationVariableService.GetValueByKey("GameStarted");
            return gameStarted != null && bool.Parse(gameStarted);
        }

        public void StartGame()
        {
            _applicationVariableService.SetValue("GameStarted", true.ToString());
            _applicationVariableService.SetValue("NumberOfObjectives", 10.ToString());
            _applicationVariableService.SetValue("CurrentActiveObjective", 1.ToString());
        }

        public void ResetGame()
        {
            _applicationVariableService.SetValue("GameStarted", false.ToString());
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
