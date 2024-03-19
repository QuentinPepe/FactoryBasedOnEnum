using System;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;

namespace FactoryBasedOnEnum
{
    public class InstanceCreator<TEnum, TInterface>
        where TEnum : Enum
        where TInterface : class
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TypeMapper<TEnum, TInterface> _typeMapper;

        public InstanceCreator(IServiceProvider serviceProvider, TypeMapper<TEnum, TInterface> typeMapper)
        {
            _serviceProvider = serviceProvider;
            _typeMapper = typeMapper;
        }

        public TInterface CreateInstance(TEnum enumValue, Func<TInterface, bool> predicate, object[] args)
        {
            Func<IServiceProvider, object[], TInterface> dynamicMapping = _typeMapper.GetDynamicMapping(enumValue);
            if (dynamicMapping != null)
            {
                TInterface instance = dynamicMapping(_serviceProvider, args);
                if (predicate(instance))
                {
                    return instance;
                }
            }

            Type type = _typeMapper.GetTypeForEnum(enumValue);
            if (type != null)
            {
                try
                {
                    if (typeof(TInterface).IsAssignableFrom(type))
                    {
                        TInterface instance = _serviceProvider != null
                            ? _serviceProvider.GetRequiredService(type) as TInterface
                            : Activator.CreateInstance(type, args) as TInterface;

                        if (predicate(instance))
                        {
                            return instance;
                        }
                    }
                    else
                    {
                        object instance = _serviceProvider != null
                            ? ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, type)
                            : Activator.CreateInstance(type, args);

                        if (instance is TInterface castedInstance && predicate(castedInstance))
                        {
                            return castedInstance;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to create an instance of '{type.Name}'. Error: {ex.Message}", ex);
                }
            }

            Type defaultType = _typeMapper.DefaultType;
            if (defaultType != null)
            {
                try
                {
                    TInterface instance = (TInterface)Activator.CreateInstance(defaultType, args);
                    if (predicate(instance))
                    {
                        return instance;
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to create a default instance of '{defaultType.Name}'. Error: {ex.Message}", ex);
                }
            }

            throw new InvalidOperationException($"No suitable class found for the enum value '{enumValue}' and no default class is registered.");
        }
    }
}