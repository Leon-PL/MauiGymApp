using System.Reflection;
using UnitsNet;

namespace MauiGymApp.Calculations
{
    public static class OneRepMaxCalculator
    {
        public static double Calculate1RM(double weight, int reps)
        => OneRepMaxFunctions.Epley.OneRepMax(weight, reps);

        public static Mass Calculate1RM(Mass weight, int reps)
            => Mass.FromKilograms(Calculate1RM(weight.Kilograms, reps));

        public static double Calculate1RM(double weight, int reps, OneRepMaxFunction[]? funcs)
        {
            funcs ??= GetAllOneRepMaxFunctions();
            return funcs.Average(f => f.OneRepMax(weight, reps));
        }
        public static Mass Calculate1RM(Mass weight, int reps, OneRepMaxFunction[]? funcs)
            => Mass.FromKilograms(Calculate1RM(weight.Kilograms, reps, funcs));

        public static RepMaxResults<double> CalculateRepMaxes(double weight, int reps)
        => RepMaxResults<double>.New(weight, reps);

        public static RepMaxResults<Mass> CalculateRepMaxes(Mass weight, int reps)
            => RepMaxResults<Mass>.New(weight, reps);

        static PropertyInfo[] FindOneRepMaxFunctions() => typeof(OneRepMaxFunctions).GetProperties(BindingFlags.Static | BindingFlags.Public)
            .Where(m => m.PropertyType.Equals(typeof(OneRepMaxFunction))).ToArray();

        public static OneRepMaxFunction[] GetAllOneRepMaxFunctions()
            => FindOneRepMaxFunctions().Select(f => f.GetValue(null))
                                       .Where(f => f != null)
                                       .Select(f => (OneRepMaxFunction)f!)
                                       .ToArray();
    }
}
