# Generic Factory Based on Enum

This library provides a flexible and extensible implementation of the Factory design pattern based on enumerations. It
allows you to create instances of classes associated with enum values, with support for various advanced features such
as dynamic mappings, custom creation functions, default type handling, singleton instances, and predicate-based instance
retrieval.

## Features

- **Dynamic Mappings**: Define mappings between enumeration values and classes at runtime, offering flexibility for
  complex scenarios.
- **Custom Creation Function**: Customize the instantiation process with a custom function, allowing for dependency
  injection or specific instantiation logic.
- **Default Type Handling**: Specify a default type to be used when no specific mapping is found for an enumeration
  value.
- **Singleton Option**: Optionally maintain a singleton instance for each enumeration value.
- **Predicate-based Instance Retrieval**: Retrieve instances based on a predicate to allow conditional instantiation.
- **Dispose Pattern Support**: Automatically dispose of instances if they implement `IDisposable`.

## Installation

You can install the package via NuGet:

```
Install-Package GenericFactoryBasedOnEnum
```

## Usage

1. Define an enumeration and mark the associated classes with the `EnumAssociatedAttribute` attribute:

```csharp
public enum ShapeType
{
    Circle,
    Square,
    Rectangle
}

[EnumAssociated(typeof(ShapeType), ShapeType.Circle)]
public class Circle : IShape
{
    // Implementation
}

[EnumAssociated(typeof(ShapeType), ShapeType.Square)]
public class Square : IShape
{
    // Implementation
}

[EnumAssociated(typeof(ShapeType), ShapeType.Rectangle)]
public class Rectangle : IShape
{
    // Implementation
}
```

2. Create an instance of `GenericFactory` and use it to retrieve instances:

```csharp
var factory = new GenericFactory<ShapeType, IShape>();

// Get an instance of Circle
var circle = factory.GetInstance(ShapeType.Circle);

// Get an instance of Square with a predicate
var square = factory.GetInstance(ShapeType.Square, s => s.Area > 10);

// Dispose instances
factory.DisposeInstances();
```

## Advanced Usage

### Dynamic Mappings

You can register dynamic mappings at runtime to associate enum values with custom instance creation functions:

```csharp
factory.RegisterDynamicMapping(ShapeType.Circle, (sp, args) =>
{
    // Custom creation logic
    return new Circle(args[0], args[1]);
});
```

### Custom Creation Function

You can set a custom creation function to override the default instantiation logic:

```csharp
factory.SetCustomCreationFunc((sp, type, args) =>
{
    // Custom creation logic
    return Activator.CreateInstance(type, args) as IShape;
});
```

### Default Type Handling

You can register a default type to be used when no specific mapping is found for an enumeration value:

```csharp
factory.RegisterDefaultType(typeof(Circle));
```

### Singleton Instances

By default, the `GenericFactory` maintains a singleton instance for each enumeration value. You can disable this
behavior by passing `false` to the constructor:

```csharp
var factory = new GenericFactory<ShapeType, IShape>(isSingleton: false);
```

## Contributing

Contributions are welcome! Please follow the standard GitHub workflow for contributing to open-source projects:

1. Fork the repository
2. Create a new branch
3. Make your changes and commit them
4. Push your changes to your forked repository
5. Open a pull request against the main repository

Please make sure to update the tests and documentation as appropriate.

## License

This project is licensed under the [MIT License](LICENSE).

## Credits

This library was developed by Pepe Quentin and is maintained by the open-source community.