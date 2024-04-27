using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace MauiGymApp.Services.Visual
{
    public static class SnackBarService
    {   
        
        static readonly SnackbarOptions SuccessOptions = new()
        {
            BackgroundColor = Colors.Green,
            TextColor = Colors.White,
            ActionButtonTextColor = Colors.White,
         
        };

        static readonly SnackbarOptions FailureOptions = new()
        {
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            ActionButtonTextColor = Colors.White,
   
        };

        static readonly SnackbarOptions InfoOptions = new()
        {
            BackgroundColor = Colors.LightBlue,
            TextColor = Colors.White,
            ActionButtonTextColor = Colors.White,
        };

        public static async Task ShowSnackBar(string message, SnackBarType type,double durationSeconds=3)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            string actionButtonText = "X";
        
            TimeSpan duration = TimeSpan.FromSeconds(durationSeconds);

            var snackbar = Snackbar.Make(message, null, actionButtonText, duration, OptionsFromType(type));

            await snackbar.Show(cancellationTokenSource.Token);
        }

        static SnackbarOptions OptionsFromType(SnackBarType type)
        {
            return type switch
            {
                SnackBarType.Success => SuccessOptions,
                SnackBarType.Failure => FailureOptions,
                SnackBarType.Info => InfoOptions,
                _ => throw new ArgumentException("Type not found"),
            };
        }
    }
}
