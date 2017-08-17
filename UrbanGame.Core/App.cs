using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using UrbanGame.Core.Custom;

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
            
            RegisterAppStart(Mvx.IocConstruct<CustomAppStart>());
        }
    }
}
