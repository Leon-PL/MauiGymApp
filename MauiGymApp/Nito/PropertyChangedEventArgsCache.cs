namespace MauiGymApp.Nito
{
    using System.Collections.Generic;
    using System.ComponentModel;

    //
    // Summary:
    //     Provides a cache for System.ComponentModel.PropertyChangedEventArgs instances.
    public sealed class PropertyChangedEventArgsCache
    {
        //
        // Summary:
        //     The underlying dictionary. This instance is its own mutex.
        private readonly Dictionary<string, PropertyChangedEventArgs> _cache = new Dictionary<string, PropertyChangedEventArgs>();

        //
        // Summary:
        //     The global instance of the cache.
        public static PropertyChangedEventArgsCache Instance { get; } = new PropertyChangedEventArgsCache();


        //
        // Summary:
        //     Private constructor to prevent other instances.
        private PropertyChangedEventArgsCache()
        {
        }

        //
        // Summary:
        //     Retrieves a System.ComponentModel.PropertyChangedEventArgs instance for the specified
        //     property, creating it and adding it to the cache if necessary.
        //
        // Parameters:
        //   propertyName:
        //     The name of the property that changed.
        public PropertyChangedEventArgs Get(string propertyName)
        {
            lock (_cache)
            {
                if (_cache.TryGetValue(propertyName, out PropertyChangedEventArgs value))
                {
                    return value;
                }

                value = new PropertyChangedEventArgs(propertyName);
                _cache.Add(propertyName, value);
                return value;
            }
        }
    }
}