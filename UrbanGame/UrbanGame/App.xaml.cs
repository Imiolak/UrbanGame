
using UrbanGame.Database;
using UrbanGame.Database.Models;
using Xamarin.Forms;
using MainGamePage = UrbanGame.UI.Pages.MainGamePage;
using WelcomePage = UrbanGame.UI.Pages.WelcomePage;

namespace UrbanGame
{
    public partial class App : Application
    {
        private static UrbanGameDatabase _database;

        public App()
        {
            InitializeComponent();

            Instance = this;
            MainPage = ResolveMainPage();
        }
        
        public static App Instance { get; private set; }

        public static UrbanGameDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new UrbanGameDatabase();
                }

                return _database;
            }
        }

        public void SetNewMainPage(Page newMainPage)
        {
            MainPage = newMainPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private static Page ResolveMainPage()
        {
            var gameStartedVariable = Database.GetApplicationVariableByName(ApplicationVariables.GameStarted);

            return gameStartedVariable == null || !bool.Parse(gameStartedVariable.Value)
                ? (Page) new WelcomePage()
                : new MainGamePage();
        }
    }
}
