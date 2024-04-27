namespace MauiGymApp.Calculations
{
    public class OneRepMaxFunction(Func<double, int, double> oneRepMaxFunction, Func<double, int, double> weightMaxFunction)
    {
        private Func<double, int, double> _oneRepMaxFunction { get; } = oneRepMaxFunction;
        private Func<double, int, double> _weightMaxFunction { get; } = weightMaxFunction;

        public double OneRepMax(double weight, int reps)
        {
            double w = GetValidWeight(weight);
            int r = GetValidReps(reps);
            return _oneRepMaxFunction(w, r);
        }

        public double WeightMax(double weight, int reps)
        {
            double w = GetValidWeight(weight);
            int r = GetValidReps(reps);
            return _weightMaxFunction(w, r);
        }

        public Func<double, int, double> GetOneRepMaxFunction => _oneRepMaxFunction;
        public Func<double, int, double> GetWeightMaxFunction => _weightMaxFunction;

        static double GetValidWeight(double weight)
        {
            if (weight < 0) throw new ArgumentException("Weight must be > 0", nameof(weight));
            return weight;
        }
        static int GetValidReps(int reps)
        {
            if (reps == 1) return 1;
            if (reps < 1) throw new ArgumentException("Reps must be > 1", nameof(reps));
            return reps;
        }
    }
}
