using System;
using System.Collections.Generic;
using System.Linq;

namespace FactoryBasedOnEnum
{
    public class InstanceDisposer<TEnum, TInterface>
        where TEnum : Enum
        where TInterface : class
    {
        private static void DisposeInstances(IEnumerable<Lazy<TInterface>> instances)
        {
            foreach (TInterface instance in instances.Select(lazy => lazy.Value))
            {
                if (instance is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        public void DisposeInstances(IReadOnlyDictionary<TEnum, Lazy<TInterface>> instances)
        {
            DisposeInstances(instances.Values);
        }
    }
}