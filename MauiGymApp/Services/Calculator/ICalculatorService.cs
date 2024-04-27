using MauiGymApp.Calculations;
using UnitsNet;

namespace MauiGymApp.Services.Calculator
{
    public interface ICalculatorService
    {
        IEnumerable<OneRepMaxFunction> GetChosenOneRepMaxFunctions();
        double OneRepMax(double weight, int reps);
        double OneRepMax(double weight, int reps, IEnumerable<OneRepMaxFunction> functions);
        Mass OneRepMax(Mass weight, int reps);
        double WeightMax(double orm, int reps, IEnumerable<OneRepMaxFunction> functions);
        RepMaxResults<double> WeightMaxes(double weight, int reps);
        RepMaxResults<Mass> WeightMaxes(Mass weight, int reps);
    }
}
