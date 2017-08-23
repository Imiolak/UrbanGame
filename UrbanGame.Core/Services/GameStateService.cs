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
                ObjectiveNo = 1
            });
            _objectiveService.AddObjectiveStep(new TextObjectiveStep
            {
                Text = Lorem1,
                ObjectiveNo = 1,
                OrderInObjective = 1
            });
            _objectiveService.AddObjectiveStep(new TextObjectiveStep
            {
                Text = Lorem2,
                ObjectiveNo = 1,
                OrderInObjective = 2
            });
            _objectiveService.AddObjective(new Objective
            {
                ObjectiveNo = 2
            });
            _objectiveService.AddObjectiveStep(new TextObjectiveStep
            {
                Text = "no 2 s1",
                ObjectiveNo = 2,
                OrderInObjective = 1
            });
            _objectiveService.AddObjective(new Objective
            {
                ObjectiveNo = 3
            });
            _objectiveService.AddObjectiveStep(new TextObjectiveStep
            {
                Text = Lorem3,
                ObjectiveNo = 3,
                OrderInObjective = 1
            });
            _objectiveService.AddObjectiveStep(new TextObjectiveStep
            {
                Text = Lorem4,
                ObjectiveNo = 3,
                OrderInObjective = 2
            });
            _objectiveService.AddObjectiveStep(new TextObjectiveStep
            {
                Text = Lorem5,
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

        private const string Lorem1 = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec placerat imperdiet augue ut interdum. Phasellus viverra leo vel dolor congue convallis. Mauris augue nulla, porttitor ac mauris et, sollicitudin posuere elit. Aliquam erat volutpat. Fusce pretium pulvinar dolor quis efficitur. Etiam scelerisque, metus at bibendum feugiat, elit nibh elementum massa, in gravida dolor nisl ac metus. Aenean eget arcu tempor, euismod libero id, dignissim lectus. Sed pellentesque ullamcorper lorem, at interdum ipsum finibus et. Proin auctor odio diam, in vestibulum elit sollicitudin sit amet. Vestibulum et magna vel nisi scelerisque tincidunt. Etiam vitae eros viverra, tincidunt erat et, sollicitudin arcu. Donec a elementum orci.";
        private const string Lorem2 = "Sed pulvinar ante sit amet felis elementum sollicitudin. Morbi quis orci vitae lectus volutpat lobortis. Cras eu pharetra dolor. Quisque sagittis fringilla enim vel malesuada. Pellentesque eu odio in erat mollis placerat feugiat vitae sem. Donec sit amet leo cursus, vestibulum purus vitae, malesuada nibh. Sed non vulputate velit, non rhoncus nulla. Integer et dui ac dolor bibendum tempor eu quis ex. Cras pharetra metus id arcu dictum, quis condimentum dui varius. Proin eu diam elementum, consequat leo in, euismod ante. Aliquam erat volutpat. Morbi gravida laoreet lectus, at lobortis lorem volutpat ut.";
        private const string Lorem3 = "Proin tempor in magna eu faucibus. Vivamus nec mollis ante. Donec sed aliquet libero. Nulla ac rutrum mauris. Aenean lobortis feugiat magna. Aliquam consectetur leo sit amet ligula finibus eleifend. Donec ut lorem nibh. Fusce erat orci, molestie ac dui et, cursus bibendum augue. Praesent congue congue nibh. Duis varius mauris et interdum malesuada. Aliquam pharetra volutpat ligula, quis hendrerit massa semper nec. Integer semper est ex, quis scelerisque ex varius ut. Nunc dictum felis elit, eu egestas ipsum semper a. Nulla facilisi. Suspendisse tincidunt non augue in accumsan. Praesent egestas lorem nec ligula volutpat congue.";
        private const string Lorem4 = "Cras egestas, mauris id hendrerit iaculis, arcu nunc accumsan ante, nec ornare enim ligula ultricies mauris. Ut placerat interdum accumsan. Fusce commodo ultricies mi, eget dignissim felis porttitor eget. Integer a semper justo. Donec in pretium sapien, ut interdum magna. Aliquam bibendum interdum hendrerit. Nunc sit amet vestibulum felis. Duis quis libero molestie lectus vehicula tincidunt sed sed tellus. Morbi semper blandit felis, ultricies venenatis lacus tempor quis. Vestibulum ac tellus lectus. Cras hendrerit velit vitae ornare suscipit. Phasellus volutpat turpis fermentum sem feugiat bibendum. Phasellus et semper diam. Duis lacinia nisi odio, et cursus velit bibendum sed. Proin ultrices arcu sed cursus pellentesque.";
        private const string Lorem5 = "Duis in mi tincidunt, interdum sem sit amet, iaculis urna. Quisque ut dictum mauris, a blandit neque. Pellentesque malesuada enim efficitur lorem pellentesque, vitae mollis nulla efficitur. Ut facilisis metus id justo convallis pulvinar. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec at tempus turpis. Donec pharetra, turpis sed sodales suscipit, diam felis ullamcorper erat, vitae varius est nibh eget est. Nam mollis ex elit, in sodales dolor aliquet eu. Fusce auctor mattis tellus vitae convallis. Aliquam interdum cursus cursus. Aliquam malesuada, ipsum quis scelerisque tincidunt, turpis purus sagittis tortor, in porta lacus turpis vel metus.";

    }
}
