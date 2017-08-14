using MvvmCross.Core.ViewModels;

namespace UrbanGame.Core.ViewModels.Game
{
    public class ObjectiveHeaderViewModel : MvxViewModel
    {
        public void Init(int objectiveNo, string objectiveTitle)
        {
            ObjectiveNo = objectiveNo;
            ObjectiveTitle = objectiveTitle;
        }

        private int _objectiveNo;
        public int ObjectiveNo
        {
            get => _objectiveNo;
            set
            {
                _objectiveNo = value;
                RaisePropertyChanged(nameof(ObjectiveNo));
            }
        }

        private string _objectiveTitle;
        public string ObjectiveTitle
        {
            get => _objectiveTitle;
            set
            {
                _objectiveTitle = value;
                RaisePropertyChanged(nameof(ObjectiveTitle));
            }
        }
    }
}
