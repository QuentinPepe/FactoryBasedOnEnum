using System.Numerics;

namespace FactoryBasedOnEnum.Examples
{
    public class WithArgumentExample
    {
        private readonly GenericFactory<UnitType, IUnit> _unitFactory = new GenericFactory<UnitType, IUnit>();

        public IUnit CreateUnit(UnitType type, int health)
        {
            return _unitFactory.GetInstance(type, health);
        }
    }

    internal class Game
    {
        public void StartGame()
        {
            WithArgumentExample unitManager = new WithArgumentExample();

            IUnit infantry = unitManager.CreateUnit(UnitType.Infantry, 100);
            IUnit cavalry = unitManager.CreateUnit(UnitType.Cavalry, 150);
            IUnit archer = unitManager.CreateUnit(UnitType.Archer, 80);

            infantry.Move(new Vector2(10, 10));
            cavalry.Attack();
            archer.Move(new Vector2(20, 20));
        }
    }

    public enum UnitType
    {
        Infantry,
        Cavalry,
        Archer
    }

    public interface IUnit
    {
        void Move(Vector2 position);
        void Attack();
    }

    [EnumAssociated(typeof(UnitType), UnitType.Infantry)]
    public class Infantry : IUnit
    {
        public Infantry(int health)
        {
            /* ... */
        }

        public void Move(Vector2 position)
        {
            /* ... */
        }

        public void Attack()
        {
            /* ... */
        }
    }

    [EnumAssociated(typeof(UnitType), UnitType.Cavalry)]
    public class Cavalry : IUnit
    {
        public Cavalry(int health)
        {
            /* ... */
        }

        public void Move(Vector2 position)
        {
            /* ... */
        }

        public void Attack()
        {
            /* ... */
        }
    }

    [EnumAssociated(typeof(UnitType), UnitType.Archer)]
    public class Archer : IUnit
    {
        public Archer(int health)
        {
            /* ... */
        }

        public void Move(Vector2 position)
        {
            /* ... */
        }

        public void Attack()
        {
            /* ... */
        }
    }
}