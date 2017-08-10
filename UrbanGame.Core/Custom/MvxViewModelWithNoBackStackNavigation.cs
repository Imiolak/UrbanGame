using MvvmCross.Core.ViewModels;
using System.Collections.Generic;

namespace UrbanGame.Core.Custom
{
    public class MvxViewModelWithNoBackStackNavigation : MvxViewModel
    {
        protected void ShowViewModelAndClearBackStack<TViewModel>()
            where TViewModel : MvxViewModel
        {
            var presentationBundle = new MvxBundle(
                new Dictionary<string, string>
                {
                    {
                        "ClearBackStack", ""
                    }
                });
            ShowViewModel<TViewModel>(presentationBundle: presentationBundle);
        }
    }
}
