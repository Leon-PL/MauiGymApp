 using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiGymApp.ViewModels.Common
{
    public abstract class DTOViewModel<T> : BaseViewModel
    {
        /// <summary>
        /// Returns instance of the original model with properties set in the view model
        /// </summary>
        /// <returns></returns>
        public  abstract T ToModel();
    }
}
