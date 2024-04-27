namespace MauiGymApp.ViewModels.Interfaces
{
    public interface ITemplate<TImplementation>
    {
        TImplementation ToImplementation();
    }
}
