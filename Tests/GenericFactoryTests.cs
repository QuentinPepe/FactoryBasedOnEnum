using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace FactoryBasedOnEnum.Tests
{
    [TestFixture]
    public class GenericFactoryTests
    {
        [Test]
        public void GetInstance_WithValidEnum_ReturnsCorrectInstance()
        {
            GenericFactory<TestEnum, ITestInterface> factory = new GenericFactory<TestEnum, ITestInterface>();
            ITestInterface instance = factory.GetInstance(TestEnum.TypeA, "Test");

            Assert.IsInstanceOf<TypeA>(instance);
            Assert.AreEqual("Test", instance.Name);
        }

        [Test]
        public void GetInstance_WithPredicate_ReturnsCorrectInstance()
        {
            GenericFactory<TestEnum, ITestInterface> factory = new GenericFactory<TestEnum, ITestInterface>();
            ITestInterface instance = factory.GetInstance(TestEnum.TypeB, i => i.Name.Contains("42"), 42);

            Assert.IsInstanceOf<TypeB>(instance);
            Assert.AreEqual("TypeB-42", instance.Name);
        }

        [Test]
        public void GetInstance_WithInvalidEnum_ThrowsException()
        {
            GenericFactory<TestEnum, ITestInterface> factory = new GenericFactory<TestEnum, ITestInterface>();

            Assert.Throws<InvalidOperationException>(() => factory.GetInstance((TestEnum)42));
        }

        [Test]
        public void GetInstance_WithDynamicMapping_ReturnsCorrectInstance()
        {
            GenericFactory<TestEnum, ITestInterface> factory = new GenericFactory<TestEnum, ITestInterface>();
            factory.RegisterDynamicMapping(TestEnum.TypeC, (_, args) => new TypeC((bool)args[0]));

            ITestInterface instance = factory.GetInstance(TestEnum.TypeC, true);

            Assert.IsInstanceOf<TypeC>(instance);
            Assert.AreEqual("TypeC-True", instance.Name);
        }

        [Test]
        public void GetInstance_WithCustomCreationFunc_ReturnsCorrectInstance()
        {
            GenericFactory<TestEnum, ITestInterface> factory = new GenericFactory<TestEnum, ITestInterface>();
            factory.SetCustomCreationFunc((_, type, args) => (ITestInterface)Activator.CreateInstance(type, args));

            ITestInterface instance = factory.GetInstance(TestEnum.TypeB, 123);

            Assert.IsInstanceOf<TypeB>(instance);
            Assert.AreEqual("TypeB-123", instance.Name);
        }

        [Test]
        public void GetInstance_WithDefaultType_ReturnsCorrectInstance()
        {
            GenericFactory<TestEnum, ITestInterface> factory = new GenericFactory<TestEnum, ITestInterface>();
            factory.RegisterDefaultType(typeof(TypeC));

            ITestInterface instance = factory.GetInstance((TestEnum)42, false);

            Assert.IsInstanceOf<TypeC>(instance);
            Assert.AreEqual("TypeC-False", instance.Name);
        }

        [Test]
        public void DisposeInstances_WithDisposableInstances_DisposesInstances()
        {
            int disposableInstanceCount = 0;
            GenericFactory<TestEnum, IDisposable> factory = new GenericFactory<TestEnum, IDisposable>();
            factory.RegisterDynamicMapping(TestEnum.TypeA, (_, _) => new DisposableInstance(() => disposableInstanceCount++));
            factory.RegisterDynamicMapping(TestEnum.TypeB, (_, _) => new DisposableInstance(() => disposableInstanceCount++));

            factory.GetInstance(TestEnum.TypeA);
            factory.GetInstance(TestEnum.TypeB);

            factory.DisposeInstances();

            Assert.AreEqual(2, disposableInstanceCount);
        }

        [Test]
        public void RegisterDynamicMapping_WithValidEnum_RegistersMapping()
        {
            GenericFactory<TestEnum, ITestInterface> factory = new GenericFactory<TestEnum, ITestInterface>();
            factory.RegisterDynamicMapping(TestEnum.TypeC, (_, args) => new TypeC((bool)args[0]));

            ITestInterface instance = factory.GetInstance(TestEnum.TypeC, true);

            Assert.IsInstanceOf<TypeC>(instance);
            Assert.AreEqual("TypeC-True", instance.Name);
        }

        [Test]
        public void UnregisterDynamicMapping_WithValidEnum_UnregistersMapping()
        {
            GenericFactory<TestEnum, ITestInterface> factory = new GenericFactory<TestEnum, ITestInterface>();
            factory.RegisterDynamicMapping(TestEnum.TypeC, (_, args) => new TypeC((bool)args[0]));
            factory.UnregisterDynamicMapping(TestEnum.TypeC);

            Assert.Throws<InvalidOperationException>(() => factory.GetInstance(TestEnum.TypeC, true));
        }

        [Test]
        public void RegisterDefaultType_WithValidType_RegistersDefaultType()
        {
            GenericFactory<TestEnum, ITestInterface> factory = new GenericFactory<TestEnum, ITestInterface>();
            factory.RegisterDefaultType(typeof(TypeC));

            ITestInterface instance = factory.GetInstance((TestEnum)42, false);

            Assert.IsInstanceOf<TypeC>(instance);
            Assert.AreEqual("TypeC-False", instance.Name);
        }

        [Test]
        public void RegisterDefaultType_WithInvalidType_ThrowsException()
        {
            GenericFactory<TestEnum, ITestInterface> factory = new GenericFactory<TestEnum, ITestInterface>();

            Assert.Throws<ArgumentException>(() => factory.RegisterDefaultType(typeof(string)));
        }

        [Test]
        public void UnregisterDefaultType_RemovesDefaultType()
        {
            GenericFactory<TestEnum, ITestInterface> factory = new GenericFactory<TestEnum, ITestInterface>();
            factory.RegisterDefaultType(typeof(TypeC));
            factory.UnregisterDefaultType();

            Assert.Throws<InvalidOperationException>(() => factory.GetInstance((TestEnum)42, false));
        }

        [Test]
        public void GetInstance_WithSingletonDisabled_ReturnsNewInstanceEachTime()
        {
            GenericFactory<TestEnum, ITestInterface> factory = new GenericFactory<TestEnum, ITestInterface>(false);

            ITestInterface instance1 = factory.GetInstance(TestEnum.TypeA, "Test");
            ITestInterface instance2 = factory.GetInstance(TestEnum.TypeA, "Test");

            Assert.AreNotSame(instance1, instance2);
        }

        public class DisposableInstance : IDisposable
        {
            private readonly Action _onDispose;

            public DisposableInstance(Action onDispose)
            {
                _onDispose = onDispose;
            }

            public void Dispose()
            {
                _onDispose?.Invoke();
            }
        }
    }
}