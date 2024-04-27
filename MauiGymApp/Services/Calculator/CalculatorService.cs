using MauiGymApp.Calculations;
using MauiGymApp.Services.Settings;
using UnitsNet;

namespace MauiGymApp.Services.Calculator
{
    public class CalculatorService : ICalculatorService
    {
        private readonly ISettingsService _settingsService;

        public CalculatorService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public IEnumerable<OneRepMaxFunction> GetChosenOneRepMaxFunctions()
        {
            List<OneRepMaxFunction> functions = [];

            if (_settingsService.UseEpley) functions.Add(OneRepMaxFunctions.Epley);
            if (_settingsService.UseBryzcki) functions.Add(OneRepMaxFunctions.Bryzki);
            if (_settingsService.UseLombardi) functions.Add(OneRepMaxFunctions.Lombardi);
            if (_settingsService.UseMayhew) functions.Add(OneRepMaxFunctions.Mayhew);
            if (_settingsService.UseOConnor) functions.Add(OneRepMaxFunctions.OConnor);
            if (_settingsService.UseWathan) functions.Add(OneRepMaxFunctions.Wathan);

            return functions;
        }

        public double OneRepMax(double weight, int reps)
            => OneRepMaxCalculator.Calculate1RM(weight, reps, GetChosenOneRepMaxFunctions().ToArray());

        public double OneRepMax(double weight, int reps, IEnumerable<OneRepMaxFunction> functions)
            => OneRepMaxCalculator.Calculate1RM(weight, reps, functions.ToArray());

        public Mass OneRepMax(Mass weight, int reps)
            => OneRepMaxCalculator.Calculate1RM(weight, reps, GetChosenOneRepMaxFunctions().ToArray());

        public double WeightMax(double orm, int reps, IEnumerable<OneRepMaxFunction> functions) 
            => functions.Average(f => f.WeightMax(orm, reps));

        public RepMaxResults<double> WeightMaxes(double weight, int reps)
            => RepMaxResults<double>.New(weight, reps, GetChosenOneRepMaxFunctions().ToArray());

        public RepMaxResults<Mass> WeightMaxes(Mass weight, int reps)
            => RepMaxResults<Mass>.New(weight, reps, GetChosenOneRepMaxFunctions().ToArray());


    }
}
