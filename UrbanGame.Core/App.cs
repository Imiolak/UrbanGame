using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using UrbanGame.Core.Custom;
using UrbanGame.Core.Services;

namespace UrbanGame.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();           
            
            RegisterAppStart(new CustomAppStart(Mvx.Resolve<IApplicationVariableService>()));
        }
    }
}
