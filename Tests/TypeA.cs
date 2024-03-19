namespace FactoryBasedOnEnum.Tests
{

    [EnumAssociated(typeof(TestEnum), TestEnum.TypeA)]
    public class TypeA : ITestInterface
    {
        public TypeA(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

}