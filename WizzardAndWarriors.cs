using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
/*
 http://ericlippert.com/2015/04/27/wizards-and-warriors-part-one/
 */
namespace WizzardAndWarriors.Test
{//*/
    [TestClass]
    public class WizzardAndWarriors
    {
        [TestMethod]
        public void TestMethod1()
        {
            var player = new Warrior();
            player.GiveHim(new Axe());
        }

        [TestMethod]
        public void CalcWeigthTest()
        {
            var player = new Warrior();
            player.GiveHim(new Axe());

            var calc = new CalcWeigth();
            player.Accept(calc);
            Assert.AreEqual(0.4f, calc.Weight);
        }

        [TestMethod]
        public void RidedHorse()
        {
            var player = new Warrior();
            player.GiveHim(new Axe());
            
            var horse = new HorseWithRider(new Horse());
            horse.ToRide(player);

            Assert.AreEqual(Math.Round(9.96f, 2), Math.Round(horse.Power, 2));
            Assert.AreEqual(Math.Round(9.96f, 2), Math.Round(horse.Speed, 2));
        }

        [TestMethod]
        public void HorseWithBaggage()
        {            
            var horse = new HorseWithBaggage(new Horse());
            horse.CarryOn(new Axe());
            horse.CarryOn(new Axe());

            Assert.AreEqual(Math.Round(9.88f, 2), Math.Round(horse.Power, 2));
            Assert.AreEqual(Math.Round(9.88f, 2), Math.Round(horse.Speed, 2));
        }
    }

    public interface IWeapon
    {
        float Weight { get; }
    }

    public interface IPlayer
    {
        void GiveHim<T>(T item);
        void Accept(IVisitor visitor);
    }

    public interface IBag
    {
        IBag Put<T>(T item);
        IEnumerable<T> GetAll<T>();
    }

    public class CommonBag : IBag
    {
        private readonly HashSet<object> _hashSet = new HashSet<object>();

        public IBag Put<T>(T item)
        {
            _hashSet.Add(item);
            return this;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _hashSet.Cast<T>();
        }
    }

    public class Warrior : IPlayer
    {
        private readonly IBag _bag;

        public Warrior()
        {
            _bag = new CommonBag();
        }

        public void GiveHim<T>(T item)
        {
            _bag.Put(item);
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(_bag);
        }
    }

    public class Axe : IWeapon
    {
        public float Weight
        {
            get { return 0.4f; }
        }
    }

    public interface IVisitor
    {
        void Visit(IBag bag);
    }

    public class CalcWeigth : IVisitor
    {
        public float Weight { get; private set; }

        public void Visit(IBag bag)
        {
            Weight = bag.GetAll<IWeapon>().Sum(w => w.Weight);
        }
    }

    public interface IHorse
    {
        float Power { get; }
        float Speed { get; }
    }

    public class Horse : IHorse
    {
        public float Power { get; private set; }
        public float Speed { get; private set; }

        public Horse()
        {
            Power = 10;
            Speed = 10;
        }
    }

    public class HorsePhysicsCalculator
    {
        private float _weight;

        public HorsePhysicsCalculator(float power, float speed, float weight)
        {
            Power = power;
            Speed = speed;
            _weight = weight;
        }

        public void Calculate()
        {
            if (!(_weight > 0.001f)) return;
            Power = Power - _weight / Power;
            Speed = Speed - _weight / Power;
        }

        public float Speed { get; private set; }
        public float Power { get; private set; }
    }

    public class HorseWithRider : IHorse
    {
        private readonly IHorse _horse;

        private IPlayer _player;

        public HorseWithRider(IHorse horse)
        {
            _horse = horse;
            Power = _horse.Power;
            Speed = _horse.Speed;
        }

        public IPlayer Player
        {
            get { return _player; }
        }

        public IHorse Horse
        {
            get { return _horse; }
        }

        public void ToRide(IPlayer player)
        {
            _player = player;

            var calc = new CalcWeigth();
            _player.Accept(calc);

            var calculator = new HorsePhysicsCalculator(Power, Speed, calc.Weight);
            calculator.Calculate();
            Power = calculator.Power;
            Speed = calculator.Speed;
        }

        public float Power { get; private set; }
        public float Speed { get; private set; }
    }

    public class HorseWithBaggage : IHorse
    {
        private readonly IHorse _horse;
        private readonly IBag _bag;

        public float Power { get; private set; }
        public float Speed { get; private set; }

        public IHorse Horse
        {
            get { return _horse; }
        }

        public HorseWithBaggage(IHorse horse)
        {
            _horse = horse;
            _bag = new CommonBag();

            Power = _horse.Power;
            Speed = _horse.Speed;
        }        

        public void CarryOn<T>(T item)
        {
            _bag.Put(item);

            var calc = new CalcWeigth();
            calc.Visit(_bag);

            var calculator = new HorsePhysicsCalculator(Power, Speed, calc.Weight);
            calculator.Calculate();
            Power = calculator.Power;
            Speed = calculator.Speed;
        }
    }
}