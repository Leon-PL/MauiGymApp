using MauiGymApp.ViewModels.Workouts.Lifts;
using System.Security.Cryptography;
using System.Windows.Input;

namespace MauiGymApp.Controls;

public partial class AllLifts : ContentView
{
    public AllLifts()
	{
		InitializeComponent();
	}     

    public static readonly BindableProperty LiftsSourceProperty = BindableProperty.Create(nameof(LiftsSource), typeof(IList<LiftViewModel>), typeof(AllLifts), null, BindingMode.TwoWay);
    public IList<LiftViewModel> LiftsSource 
    {
        get => (IList<LiftViewModel>)GetValue(AllLifts.LiftsSourceProperty);
        set => SetValue(AllLifts.LiftsSourceProperty, value);
    } 

    public static readonly BindableProperty SelectedLiftsProperty = BindableProperty.Create(nameof(SelectedLifts), typeof(IEnumerable<object>), typeof(AllLifts), null, BindingMode.TwoWay);
    public IEnumerable<object> SelectedLifts
    {
        get => (IEnumerable<object>)GetValue(AllLifts.SelectedLiftsProperty);
        set => SetValue(AllLifts.SelectedLiftsProperty, value);
    }


    public static readonly BindableProperty PickerOptionsProperty = BindableProperty.Create(nameof(PickerOptions), typeof(IList<string>), typeof(AllLifts), null, BindingMode.TwoWay);
    public IList<string> PickerOptions
    {
        get => (IList<string>)GetValue(AllLifts.PickerOptionsProperty);
        set => SetValue(AllLifts.PickerOptionsProperty, value);
    }

    public static readonly BindableProperty SelectedPickerOptionProperty = BindableProperty.Create(nameof(SelectedPickerOption), typeof(string), typeof(AllLifts), null, BindingMode.TwoWay);
    public string SelectedPickerOption
    {
        get => (string)GetValue(AllLifts.SelectedPickerOptionProperty);
        set => SetValue(AllLifts.SelectedPickerOptionProperty, value);
    }


    public static readonly BindableProperty SearchTextProperty = BindableProperty.Create(nameof(SearchText), typeof(string), typeof(AllLifts), string.Empty, BindingMode.TwoWay);
    public string SearchText
    {
        get => (string)GetValue(AllLifts.SearchTextProperty);
        set => SetValue(AllLifts.SearchTextProperty, value);
    }

    public static readonly BindableProperty InSelectionModeProperty = BindableProperty.Create(nameof(InSelectionMode), typeof(bool), typeof(AllLifts), false, BindingMode.TwoWay);
    public bool InSelectionMode
    {
        get => (bool)GetValue(AllLifts.InSelectionModeProperty);
        set => SetValue(AllLifts.InSelectionModeProperty, value);
    }

    public static readonly BindableProperty ShowConfirmButtonProperty = BindableProperty.Create(nameof(ShowConfirmButton), typeof(bool), typeof(AllLifts), false, BindingMode.TwoWay);
    public bool ShowConfirmButton
    {
        get => (bool)GetValue(AllLifts.ShowConfirmButtonProperty);
        set => SetValue(AllLifts.ShowConfirmButtonProperty, value);
    }

    public static readonly BindableProperty MoreCommandProperty = BindableProperty.Create(nameof(MoreCommand), typeof(ICommand), typeof(AllLifts));
    public ICommand MoreCommand
    {
        get => (ICommand)GetValue(MoreCommandProperty);
        set => SetValue(MoreCommandProperty, value);
    }

    public static readonly BindableProperty SelectedLiftsChangedCommandProperty = BindableProperty.Create(nameof(SelectedLiftsChangedCommand), typeof(ICommand), typeof(AllLifts));
    public ICommand SelectedLiftsChangedCommand
    {
        get => (ICommand)GetValue(SelectedLiftsChangedCommandProperty);
        set => SetValue(SelectedLiftsChangedCommandProperty, value);
    }

    public static readonly BindableProperty ConfirmCommandProperty = BindableProperty.Create(nameof(ConfirmCommand), typeof(ICommand), typeof(AllLifts));
    public ICommand ConfirmCommand
    {
        get => (ICommand)GetValue(ConfirmCommandProperty);
        set => SetValue(ConfirmCommandProperty, value);
    }
}