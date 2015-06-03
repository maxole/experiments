using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WizzardAndWarriors.Test
{
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
            
            var horse = new HorseWithRider(new Horse(), player);

            Assert.AreEqual(25.0f, horse.Power);
            Assert.AreEqual(-15.0f, horse.Speed);
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

    public class WarrionBag : IBag
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
            _bag = new WarrionBag();
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

    public class HorseWithRider : IHorse
    {
        private readonly IHorse _horse;
        private readonly IPlayer _player;
        private readonly float playerWeight = 0.0f;

        public HorseWithRider(IHorse horse, IPlayer player)
        {
            _horse = horse;
            _player = player;

            var calc = new CalcWeigth();
            _player.Accept(calc);
            playerWeight = calc.Weight;
        }

        public IPlayer Player
        {
            get { return _player; }
        }

        public float Power
        {
            get
            {
                if (playerWeight < 0.001f)
                    return _horse.Power;
                return _horse.Power / playerWeight;
            }
        }

        public float Speed
        {
            get { return _horse.Speed - Power; }
        }
    }
}
