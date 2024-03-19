using System;

namespace FactoryBasedOnEnum
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class EnumAssociatedAttribute : Attribute
    {
        public Type EnumType { get; }
        public object EnumValue { get; }

        public EnumAssociatedAttribute(Type enumType, object enumValue)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("enumType must be an enum type");

            if (!Enum.IsDefined(enumType, enumValue))
                throw new ArgumentException($"The value '{enumValue}' is not a valid member of '{enumType}'.");

            EnumType = enumType;
            EnumValue = enumValue;
        }
    }
}