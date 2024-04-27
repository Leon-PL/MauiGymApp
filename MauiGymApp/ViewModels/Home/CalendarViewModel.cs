using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Maui.Core.Internal;
using MauiGymApp.Services.WeightLifting;
using MauiGymApp.State;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Workouts;
using Plugin.Maui.Calendar.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiGymApp.ViewModels.Home
{   
     public partial class CalendarViewModel : BaseViewModel
    {
        private readonly IWorkoutStateService _stateService;

        public CalendarViewModel(IWorkoutStateService stateService)
        {
            _stateService = stateService;

            SelectedDate = DateTime.Now;
            ShownDate = DateTime.Now;
            Year = ShownDate.Year;
            Month = ShownDate.Month;

            OnWorkoutsLoaded();
            _stateService.WorkoutsChanged += OnWorkoutsLoaded;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedWorkout))]
        DateTime selectedDate;

        partial void OnSelectedDateChanged(DateTime value)
        {

            WorkoutViewModel? selected = null;
            Workouts?.TryGetValue(value.Date, out selected);
            SelectedWorkout = selected;

            if (SelectedWorkout is null)
            {
                IsWorkoutAvailable = false;
            }
            else
            {
                IsWorkoutAvailable = true;
            }
        }
         
        [ObservableProperty]
        WorkoutViewModel? selectedWorkout;

        [ObservableProperty]
        bool isWorkoutAvailable;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HeaderDateText))]
        DateTime shownDate;

        [ObservableProperty] 
        int year;

        [RelayCommand]
        void IncrementDate() => ShownDate = ShownDate.AddMonths(1);

        [RelayCommand]
        void DecrementDate() => ShownDate = ShownDate.AddMonths(-1);

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HeaderDateText))]
        int month;

        public string HeaderDateText => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month) + $" {Year}";

        public IDictionary<DateTime, WorkoutViewModel> Workouts { get; private set; }

        [ObservableProperty]
        public EventCollection workoutEvents;

        public void OnWorkoutsLoaded()
        {
            var workouts = _stateService.Workouts;
            var result = new Dictionary<DateTime, WorkoutViewModel>();
            workouts.ForEach(vm => result[vm.DateTime] = vm);
            Workouts = result;

            WorkoutEvents = ToEventCollection(result);
        }


        public static EventCollection ToEventCollection<T>(IDictionary<DateTime, T> dictionary)
        {
            var events = new EventCollection();
            foreach (var dt in dictionary.Keys)
            {
                events[dt] = new List<T>() { dictionary[dt] };
            }
            return events;
        }

        public override void Dispose()
        {
            base.Dispose();
             _stateService.WorkoutsChanged -= OnWorkoutsLoaded;
        }
    }
}
