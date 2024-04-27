using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnitsNet;

namespace MauiGymApp.Calculations
{
    public class RepMaxResults<T> : IReadOnlyDictionary<int, T>
    {
        private readonly Dictionary<int, T> _results;

        private RepMaxResults(IDictionary<int, T> dictionary)
        {
            _results = new Dictionary<int, T>(dictionary);
        }

        public static RepMaxResults<double> New(double weight, int reps, OneRepMaxFunction[]? funcs = null, int maxReps = 20)
        {
            if (funcs == null || funcs.Length == 0) funcs = OneRepMaxCalculator.GetAllOneRepMaxFunctions();
            var temp = new Dictionary<int, double>
            {
                [1] = OneRepMaxCalculator.Calculate1RM(weight, reps, funcs) // Throw Exception ?
            };
            for (int i = 2; i < maxReps + 1; i++)
            {
                temp[i] = funcs.Average(f => f.WeightMax(temp[1], i));
            }
            return new RepMaxResults<double>(temp);
        }

        public static RepMaxResults<Mass> New(Mass weight, int reps, OneRepMaxFunction[]? funcs = null, int maxReps = 20)
        {
            var results = New(weight.Kilograms, reps, funcs, maxReps)
                          .ToDictionary(x => x.Key, x => Mass.FromKilograms(x.Value));
            return new RepMaxResults<Mass>(results);
        }

        public T this[int key] => _results[key];

        public IEnumerable<int> Keys => _results.Keys;

        public IEnumerable<T> Values => _results.Values;

        public int Count => _results.Count;

        public bool ContainsKey(int key) => _results.ContainsKey(key);

        public IEnumerator<KeyValuePair<int, T>> GetEnumerator() => _results.GetEnumerator();

        public bool TryGetValue(int key, [MaybeNullWhen(false)] out T value)
            => _results.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_results).GetEnumerator();
    }
}
