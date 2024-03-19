using System;

namespace FactoryBasedOnEnum.Examples
{
    public class CustomCreationExample
    {
        private readonly GenericFactory<VehicleType, IVehicle> _vehicleFactory;

        public CustomCreationExample(IServiceProvider serviceProvider)
        {
            _vehicleFactory = new GenericFactory<VehicleType, IVehicle>(serviceProvider: serviceProvider);
            _vehicleFactory.SetCustomCreationFunc((_, type, args) => {
                IVehicle vehicle = (IVehicle)Activator.CreateInstance(type);

                string configuration = args.Length > 0 ? (string)args[0] : "Default Configuration";
                vehicle.Initialize(configuration);

                return vehicle;
            });
        }

        public IVehicle CreateVehicle(VehicleType type, string configuration)
        {
            return _vehicleFactory.GetInstance(type, configuration);
        }
    }


    public enum VehicleType
    {
        Car,
        Truck,
        Motorcycle
    }


    public interface IVehicle
    {
        void Initialize(string configuration);
        void StartEngine();
    }

    [EnumAssociated(typeof(VehicleType), VehicleType.Car)]
    public class Car : IVehicle
    {
        public void Initialize(string configuration)
        {
            Console.WriteLine($"Car initialized with configuration: {configuration}");
        }

        public void StartEngine()
        {
            Console.WriteLine("Car engine started.");
        }
    }

    [EnumAssociated(typeof(VehicleType), VehicleType.Truck)]
    public class Truck : IVehicle
    {
        public void Initialize(string configuration)
        {
            Console.WriteLine($"Truck initialized with configuration: {configuration}");
        }

        public void StartEngine()
        {
            Console.WriteLine("Truck engine started.");
        }
    }

    [EnumAssociated(typeof(VehicleType), VehicleType.Motorcycle)]
    public class Motorcycle : IVehicle
    {
        public void Initialize(string configuration)
        {
            Console.WriteLine($"Motorcycle initialized with configuration: {configuration}");
        }

        public void StartEngine()
        {
            Console.WriteLine("Motorcycle engine started.");
        }
    }
}