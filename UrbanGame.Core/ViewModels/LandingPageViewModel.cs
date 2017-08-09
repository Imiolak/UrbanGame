using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UrbanGame.Core.Interactions;
using UrbanGame.Core.Services;
using ZXing;
using ZXing.Mobile;

namespace UrbanGame.Core.ViewModels
{
    public class LandingPageViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IApplicationVariableService _applicationVariableService;

        public LandingPageViewModel(IMvxNavigationService navigationService, 
            IApplicationVariableService applicationVariableService)
        {
            _navigationService = navigationService;
            _applicationVariableService = applicationVariableService;
        }

        public IMvxCommand StartGameCommand => new MvxCommand(StartGame);

        public IMvxCommand ActionNotImplementedCommand => new MvxCommand(ActionNotImplemented);

        private readonly MvxInteraction<DialogInteraction> _showNotImplementedDialogIteraction 
            = new MvxInteraction<DialogInteraction>();

        public IMvxInteraction<DialogInteraction> ShowNotImplementedDialogIteraction =>
            _showNotImplementedDialogIteraction;
        
        public override void ViewAppearing()
        {
            var gameStarted = _applicationVariableService.GetValueByKey("GameStarted");
            if (gameStarted != null && bool.Parse(gameStarted))
            {
                _navigationService.Navigate<GamePageViewModel>();
            }

            base.ViewAppearing();
        }
        
        private async void StartGame()
        {
            var scannerOptions = new MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<BarcodeFormat>
                {
                    BarcodeFormat.QR_CODE
                }
            };
            var scanner = new MobileBarcodeScanner();

            try
            {
                var scanResult = await scanner.Scan(scannerOptions);
                _showNotImplementedDialogIteraction.Raise(new DialogInteraction
                {
                    Title = "Zeskanowa³em",
                    Text = scanResult.Text
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            

            _applicationVariableService.SetValue("GameStarted", true.ToString());
            await _navigationService.Navigate<GamePageViewModel>();
        }

        private void ActionNotImplemented()
        {
            _showNotImplementedDialogIteraction.Raise(new DialogInteraction
            {
                Title = "Jeszcze niczego tu nie ma :(",
                Text = "Soon\u2122"
            });
        }
    }
}