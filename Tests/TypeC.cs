namespace FactoryBasedOnEnum.Tests
{
    public class TypeC : ITestInterface
    {
        public TypeC(bool flag)
        {
            Name = flag ? "TypeC-True" : "TypeC-False";
        }

        public string Name { get; }
    }
}