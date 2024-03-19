using System;

namespace FactoryBasedOnEnum.Examples
{
    public class DynamicExample
    {
        private readonly GenericFactory<AnimalType, IAnimal> _animalFactory = new GenericFactory<AnimalType, IAnimal>();

        public DynamicExample(IServiceProvider serviceProvider)
        {
            _animalFactory.RegisterDynamicMapping(AnimalType.Lion, (_, args) => {
                Lion lion = new Lion();

                // Do something with the lion instance here.

                return lion;
            });
        }
    }

    public enum AnimalType
    {
        Lion,
        Elephant,
        Monkey
    }

    public interface IAnimal
    {
        void CareFor();
    }

    [EnumAssociated(typeof(AnimalType), AnimalType.Lion)]
    public class Lion : IAnimal
    {
        public void CareFor()
        {
            Console.WriteLine("Caring for a lion.");
        }
    }

    [EnumAssociated(typeof(AnimalType), AnimalType.Elephant)]
    public class Elephant : IAnimal
    {
        public void CareFor()
        {
            Console.WriteLine("Caring for an elephant.");
        }
    }

    [EnumAssociated(typeof(AnimalType), AnimalType.Monkey)]
    public class Monkey : IAnimal
    {
        public void CareFor()
        {
            Console.WriteLine("Caring for a monkey.");
        }
    }
}