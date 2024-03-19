using System;

namespace FactoryBasedOnEnum
{
    public class GenericFactory<TEnum, TInterface>
        where TEnum : Enum
        where TInterface : class
    {
        private readonly TypeMapper<TEnum, TInterface> _typeMapper;
        private readonly InstanceCreator<TEnum, TInterface> _instanceCreator;
        private readonly InstanceCache<TEnum, TInterface> _instanceCache;
        private readonly InstanceDisposer<TEnum, TInterface> _instanceDisposer;

        public GenericFactory(bool isSingleton = true, IServiceProvider serviceProvider = null)
        {
            _typeMapper = new TypeMapper<TEnum, TInterface>();
            _instanceCreator = new InstanceCreator<TEnum, TInterface>(serviceProvider, _typeMapper);
            _instanceCache = new InstanceCache<TEnum, TInterface>(isSingleton);
            _instanceDisposer = new InstanceDisposer<TEnum, TInterface>();
        }

        public TInterface GetInstance(TEnum enumValue, params object[] args)
        {
            return GetInstance(enumValue, _ => true, args);
        }

        public TInterface GetInstance(TEnum enumValue, Func<TInterface, bool> predicate, params object[] args)
        {
            TInterface instance = _instanceCache.GetInstance(enumValue, () => _instanceCreator.CreateInstance(enumValue, predicate, args));
            return instance;
        }

        public void DisposeInstances()
        {
            _instanceDisposer.DisposeInstances(_instanceCache.Instances);
            _instanceCache.Clear();
        }
        public void SetCustomCreationFunc(Func<IServiceProvider, Type, object[], TInterface> creationFunc)
        {
            _typeMapper.SetCustomCreationFunc(creationFunc);
        }

        public void RegisterDynamicMapping(TEnum enumValue, Func<IServiceProvider, object[], TInterface> creationFunc)
        {
            _typeMapper.RegisterDynamicMapping(enumValue, creationFunc);
        }

        public void UnregisterDynamicMapping(TEnum enumValue)
        {
            _typeMapper.UnregisterDynamicMapping(enumValue);
        }

        public void RegisterDefaultType(Type defaultType)
        {
            _typeMapper.RegisterDefaultType(defaultType);
        }

        public void UnregisterDefaultType()
        {
            _typeMapper.UnregisterDefaultType();
        }
    }
}