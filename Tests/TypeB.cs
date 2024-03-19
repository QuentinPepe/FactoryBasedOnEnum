namespace FactoryBasedOnEnum.Tests
{
    [EnumAssociated(typeof(TestEnum), TestEnum.TypeB)]
    public class TypeB : ITestInterface
    {
        public TypeB(int id)
        {
            Name = $"TypeB-{id}";
        }

        public string Name { get; }
    }
}