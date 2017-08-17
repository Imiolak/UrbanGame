using MvvmCross.Core.ViewModels;
using UrbanGame.Core.Services;
using UrbanGame.Core.ViewModels.Game;

namespace UrbanGame.Core.ViewModels.Objective
{
    public class ObjectiveViewModel : MvxViewModel
    {
        private readonly IObjectiveService _objectiveService;
        private Models.Objective _objective;

        public ObjectiveViewModel(IObjectiveService objectiveService)
        {
            _objectiveService = objectiveService;
        }

        public void Init(int objectiveNo)
        {
            _objective = _objectiveService.GetObjectiveByObjectiveNo(objectiveNo);
        }

        public override void Start()
        {
            ShowViewModel<ObjectiveHeaderViewModel>(new
            {
                objectiveNo = _objective.ObjectiveNo,
                objectiveHeader = _objective.ObjectiveHeader
            });
        }

        public string Header => _objective.ObjectiveHeader;
    }
}
