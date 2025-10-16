namespace TaxHelper.Shared
{
    internal static class DependencyResolver
    {
        private static readonly Dictionary<Type, Func<object>> _services = new Dictionary<Type, Func<object>>();
        public static void Register<T>(Func<T> factory) where T : class
        {
            _services[typeof(T)] = () => factory();
        }
        public static T Resolve<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var factory))
            {
                return (T)factory();
            }
            throw new InvalidOperationException($"Service of type {typeof(T)} not registered.");
        }
    }
}
