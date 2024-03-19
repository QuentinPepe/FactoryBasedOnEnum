using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FactoryBasedOnEnum
{
    public class TypeMapper<TEnum, TInterface>
        where TEnum : Enum
        where TInterface : class
    {
        private readonly Dictionary<TEnum, Type> _typeCache = new Dictionary<TEnum, Type>();
        private readonly Dictionary<TEnum, Func<IServiceProvider, object[], TInterface>> _dynamicMappings = new Dictionary<TEnum, Func<IServiceProvider, object[], TInterface>>();
        private Func<IServiceProvider, Type, object[], TInterface> _customCreationFunc;
        public Type DefaultType;

        public TypeMapper()
        {
            CacheTypes();
        }

        private void CacheTypes()
        {
            List<Type> typesWithAttribute = Assembly.GetAssembly(typeof(TInterface)).GetTypes()
                .Where(type => typeof(TInterface).IsAssignableFrom(type) &&
                               type.GetCustomAttribute<EnumAssociatedAttribute>() != null)
                .ToList();

            foreach (Type type in typesWithAttribute)
            {
                EnumAssociatedAttribute attr = type.GetCustomAttribute<EnumAssociatedAttribute>();
                if (attr?.EnumValue is TEnum enumValue)
                {
                    if (_typeCache.TryGetValue(enumValue, out Type value))
                    {
                        throw new InvalidOperationException(
                            $"Duplicate enum value '{enumValue}' found in types '{value.Name}' and '{type.Name}'.");
                    }

                    _typeCache.Add(enumValue, type);
                }
            }
        }

        public Type GetTypeForEnum(TEnum enumValue)
        {
            return _typeCache.TryGetValue(enumValue, out Type type) ? type : null;
        }

        public Func<IServiceProvider, object[], TInterface> GetDynamicMapping(TEnum enumValue)
        {
            return _dynamicMappings.TryGetValue(enumValue, out Func<IServiceProvider, object[], TInterface> mapping) ? mapping : null;
        }

        public void SetCustomCreationFunc(Func<IServiceProvider, Type, object[], TInterface> creationFunc)
        {
            _customCreationFunc = creationFunc;
        }

        public void RegisterDynamicMapping(TEnum enumValue, Func<IServiceProvider, object[], TInterface> creationFunc)
        {
            _dynamicMappings[enumValue] = creationFunc;
        }

        public void UnregisterDynamicMapping(TEnum enumValue)
        {
            _dynamicMappings.Remove(enumValue);
        }

        public void RegisterDefaultType(Type defaultType)
        {
            if (!typeof(TInterface).IsAssignableFrom(defaultType))
            {
                throw new ArgumentException(
                    $"The type {defaultType.Name} does not implement {typeof(TInterface).Name}");
            }

            DefaultType = defaultType;
        }

        public void UnregisterDefaultType()
        {
            DefaultType = null;
        }
    }
}