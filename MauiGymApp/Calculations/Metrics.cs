namespace MauiGymApp.Calculations
{
    public static class Metrics
    {
        private static readonly Dictionary<string, double> maleWilksCoefficients = new()
        {
            { "a", 47.46178854 },
            { "b", 8.472061379 },
            { "c", 0.07369410346 },
            { "d", -0.001395833811 },
            { "e", 7.07665973070743E-6 },
            { "f", -1.20804336482315E-8 },
        };

        private static readonly Dictionary<string, double> femaleWilksCoefficients = new()
        {
            { "a", -125.4255398 },
            { "b",  13.71219419 },
            { "c",  -0.03307250631 },
            { "d", -0.001050400051 },
            { "e", 9.38773881462799E-6 },
            { "f", -2.3334613884954E-8 },
        };

        /// <summary>
        /// https://en.wikipedia.org/wiki/Wilks_coefficient
        /// </summary>
        public static double WilksCoefficient(double bodyWeightKg, bool male = true)
        {
            Dictionary<string, double> coeffs = male ? maleWilksCoefficients : femaleWilksCoefficients;
            return 600 / EvaluatePolynomial(bodyWeightKg, coeffs.Values.ToArray());
        }

        public static double WilksScore(double bodyWeightKg, double oneRepMaxKg, bool male = true)
            => WilksCoefficient(bodyWeightKg, male) * oneRepMaxKg;

        private static double EvaluatePolynomial(double x, double[] coefficients)
        {
            int degree = 0;
            double sum = 0;

            foreach (var c in coefficients)
            {
                sum += c * Math.Pow(x, degree);
                degree += 1;
            }

            return sum;
        }
    }
}
