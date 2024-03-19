using System;
using System.Collections.Generic;

namespace FactoryBasedOnEnum
{
    public class InstanceCache<TEnum, TInterface>
        where TEnum : Enum
        where TInterface : class
    {
        private readonly bool _isSingleton;
        private readonly Dictionary<TEnum, Lazy<TInterface>> _instances = new Dictionary<TEnum, Lazy<TInterface>>();

        public IReadOnlyDictionary<TEnum, Lazy<TInterface>> Instances {
            get {
                return _instances;
            }
        }

        public InstanceCache(bool isSingleton)
        {
            _isSingleton = isSingleton;
        }

        public TInterface GetInstance(TEnum enumValue, Func<TInterface> instanceCreator)
        {
            if (!_instances.TryGetValue(enumValue, out Lazy<TInterface> lazyInstance))
            {
                TInterface instance = instanceCreator();
                CacheInstance(enumValue, instance);
                return instance;
            }

            return lazyInstance.Value;
        }

        private void CacheInstance(TEnum enumValue, TInterface instance)
        {
            if (_isSingleton)
            {
                _instances[enumValue] = new Lazy<TInterface>(() => instance);
            }
        }

        public void Clear()
        {
            _instances.Clear();
        }
    }
}