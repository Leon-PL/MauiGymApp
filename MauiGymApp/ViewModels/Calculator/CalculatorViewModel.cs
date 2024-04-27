using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.Calculations;
using MauiGymApp.Services.Calculator;
using MauiGymApp.Services.Measurables;
using MauiGymApp.Services.Settings;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.MeasurableQuantities;
using UnitsNet;
using UnitsNet.Units;

namespace MauiGymApp.ViewModels.Calculator
{
    public partial class CalculatorViewModel : BaseViewModel
    {
        private readonly ICalculatorService _calculatorService;
        private readonly ISettingsService _settingsService;
        private readonly IMeasurablesService _measurablesService;

        public CalculatorViewModel(ICalculatorService calculatorService, IMeasurablesService measurablesService, ISettingsService settingsService)
        {
            _calculatorService = calculatorService;
            _settingsService = settingsService;
            _measurablesService = measurablesService;

            Weight = 100;//Preferences.Get("OneRepMaxCalculatorWeight", 100);
            Reps = 5;//Preferences.Get("OneRepMaxCalculatorReps", 5);

            BodyWeightUnit = _settingsService.MassUnit;

            _settingsService.MassUnitChanged += OnMassUnitChanged;
            _settingsService.EquationsInUseChanged += OnEquationsInUseChanged;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(WilksScore))]
        [NotifyPropertyChangedFor(nameof(Results))]
        [NotifyPropertyChangedFor(nameof(FormattedResults))]
        double weight;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(WilksScore))]
        [NotifyPropertyChangedFor(nameof(Results))]
        [NotifyPropertyChangedFor(nameof(FormattedResults))]
        int reps;

        [ObservableProperty]
        double weightIncrement = 5;

        [ObservableProperty]
        bool isSetBodyWeightPromptOpen;

        [ObservableProperty]
        bool isSelectBodyWeightQuantityPromptOpen;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FormattedResults))]
        [NotifyPropertyChangedFor(nameof(BodyWeightUnitString))]
        [NotifyPropertyChangedFor(nameof(BodyWeight))]
        MassUnit bodyWeightUnit;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BodyWeightEditorQuantity))]
        [NotifyCanExecuteChangedFor(nameof(SetBodyWeightCommand))]
        string? bodyWeightEditorText;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BodyWeightEditorQuantity))]
        [NotifyPropertyChangedFor(nameof(WilksScore))]
        public Mass? bodyWeight;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(WilksScore))]
        public bool isMale;

        public RepMaxResults<Mass> Results
            => _calculatorService.WeightMaxes(Mass.From(Weight, _settingsService.MassUnit), Reps);

        public Dictionary<int, string> FormattedResults
            => Results.ToDictionary(x => x.Key,
                                    x => $"{Math.Round(x.Value.As(BodyWeightUnit), 0)} {UnitsNetHelpers.GetUnitAbreviation(BodyWeightUnit)}");

        public bool IsBodyWeightQuantitySet => BodyWeight is not null;

        public IEnumerable<MeasurableQuantityViewModel>? MassQuantities { get; private set; }

        public double? WilksScore => BodyWeight is not null ?
            Metrics.WilksScore(BodyWeight!.Value.Kilograms, Results[1].Kilograms, IsMale) : null;


        public string BodyWeightUnitString
            => UnitsNetHelpers.GetUnitAbreviation(BodyWeightUnit);

        public bool IsValidBodyWeight
            => !string.IsNullOrWhiteSpace(BodyWeightEditorText) && BodyWeightEditorText.All(char.IsDigit);

        public Mass? BodyWeightEditorQuantity
        {
            get
            {
                if (BodyWeightEditorText is null) return null;
                return Mass.From(double.Parse(BodyWeightEditorText), BodyWeightUnit);
            }
        }

        [RelayCommand]
        void IncrementWeight() => Weight += WeightIncrement;

        [RelayCommand]
        void DecrementWeight()
        {
            if (Weight > WeightIncrement)
            {
                Weight -= WeightIncrement;
            }
        }

        [RelayCommand]
        void IncrementReps() => Reps += 1;

        [RelayCommand]
        void DecrementReps()
        {
            if (Reps > 1)
            {
                Reps -= 1;
            }
        }

        [RelayCommand]
        async Task OpenActionSheetAsync()
        {
            string action = await Shell.Current.DisplayActionSheet("Body Weight", "Cancel", null, "Manually Select", "Select From Measurements");
            if (action == "Manually Select")
            {
                OpenSetBodyWeightPrompt();
            }

            if (action == "Select From Measurements") await OpenSelectBodyWeightQuantityPrompt();
        }

        [RelayCommand]
        void ToggleGender() => IsMale = !IsMale;

        [RelayCommand(CanExecute = nameof(IsValidBodyWeight))]
        void SetBodyWeight()
        {
            BodyWeight = BodyWeightEditorQuantity;
            IsSetBodyWeightPromptOpen = false;
        }

        partial void OnWeightChanged(double value) => Preferences.Set("OneRepMaxCalculatorWeight", value);

        partial void OnRepsChanged(int value) => Preferences.Set("OneRepMaxCalculatorReps", value);

        void OpenSetBodyWeightPrompt() => IsSetBodyWeightPromptOpen = true;

        async Task OpenSelectBodyWeightQuantityPrompt()
        {
            var quantities = await _measurablesService.GetAllAsync();
            MassQuantities = quantities.Where(q => q.QuantityType == Models.QuantityType.Mass)
                                       .Select(q => new MeasurableQuantityViewModel(q));
            if (MassQuantities.Any())
                IsSelectBodyWeightQuantityPromptOpen = true;
            else await DisplayGenericErrorPrompt("No measurements to select from");
        }

        async Task GetBodyWeight()
        {
            var id = Preferences.Get("OneRepMaxCalculatorBodyWeightQuantityId", null);

            if (id is null) return;
            try
            {
                var dto = _measurablesService.GetAsync(Convert.ToInt32(id)).Result;
                var vm = new MeasurableQuantityViewModel(dto);

                if (vm.LatestMeasurement is not null)
                    BodyWeight = (Mass?)vm.LatestMeasurement.Value;
            }
            catch
            {
                await DisplayGenericErrorPrompt($"Can not find bodyweight id : {id}"); ;
            }
        }

        private void OnMassUnitChanged()
        {
            BodyWeightUnit = _settingsService.MassUnit;

            if (BodyWeight is not null)
            {
                var bw = (Mass)BodyWeight;
                BodyWeight = bw.ToUnit(_settingsService.MassUnit);
            }
        }

        private void OnEquationsInUseChanged()
        {
            OnPropertyChanged(nameof(Results));
            OnPropertyChanged(nameof(FormattedResults));
        }


        public override void Dispose()
        {
            base.Dispose();
            _settingsService.MassUnitChanged -= OnMassUnitChanged;
            _settingsService.EquationsInUseChanged -= OnEquationsInUseChanged;
        }
    }
}
