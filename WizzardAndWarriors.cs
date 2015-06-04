using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            IPlayer player = new Warrior(new CommonBag());
            player.GiveHim(new Axe());
        }

        [TestMethod]
        public void CalcWeigthTest()
        {
            IPlayer player = new Warrior(new CommonBag());
            player.GiveHim(new Axe());

            var weigth = player.Accept(new CalcWeigth());

            Assert.AreEqual(0.4f, weigth);
        }

        [TestMethod]
        public void RidedHorse()
        {
            var warrior = new Warrior(new CommonBag());

            var horse = new Horse(
                new HorsePowerCalculator(), 
                new CommonBag(), 
                new CalcWeigth());
            horse
                .ToRide(warrior);

            Assert.AreEqual(Math.Round(10.0f, 2), Math.Round(horse.Power, 2));
            Assert.AreEqual(Math.Round(10.0f, 2), Math.Round(horse.Speed, 2));
        }

        [TestMethod]
        public void HorseWithBaggage()
        {
            var horse = new Horse(
                new HorsePowerCalculator(), 
                new CommonBag(), 
                new CalcWeigth());

            horse
                .Load(new Axe())
                .Load(new Axe());

            Assert.AreEqual(Math.Round(9.88f, 2), Math.Round(horse.Power, 2));
            Assert.AreEqual(Math.Round(9.88f, 2), Math.Round(horse.Speed, 2));
        }
    }

    public interface IWeapon
    {
        T Accept<T>(IVisitor<T> visitor);        
    }

    public interface IPlayer
    {
        T Accept<T>(IVisitor<T> visitor);

        void GiveHim<T>(T item);        
    }

    public interface IHorse
    {
        T Accept<T>(IVisitor<T> visitor);

        IHorse ToRide(IPlayer player);
        IHorse Load<T>(T item);
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

        public Warrior(IBag bag)
        {
            _bag = bag;
        }

        public void GiveHim<T>(T item)
        {
            _bag.Put(item);
        }

        // todo !
        public ReadOnlyCollection<IWeapon> Baggage()
        {
            return _bag.GetAll<IWeapon>().ToList().AsReadOnly();
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Axe : IWeapon
    {
        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public float Weight
        {
            get { return 0.4f; }
        }
    }

    public interface IVisitor<out T>
    {
        T Visit(Warrior warrior);
        T Visit(Horse horse);
        T Visit(Axe axe);
    }

    public class CalcWeigth : IVisitor<float>
    {
        public float Visit(Warrior warrior)
        {            
            return warrior.Baggage().Sum(s => s.Accept(this));
        }

        public float Visit(Horse horse)
        {
            var riderWeigth = horse.Rider == null ? 0.0f : horse.Rider.Accept(this);
            var horseWeigth = horse.Baggage().Sum(s => s.Accept(this));
            return riderWeigth + horseWeigth;
        }

        public float Visit(Axe axe)
        {
            return axe.Weight;
        }
    }

    public class Horse : IHorse
    {
        private readonly IHorsePowerCalculator _calculator;
        private readonly IBag _bag;
        private readonly IVisitor<float> _weigthVisitor;

        public IPlayer Rider { get; private set; }
        public float Power { get; private set; }
        public float Speed { get; private set; }

        public Horse(IHorsePowerCalculator calculator, IBag bag, IVisitor<float> weigthVisitor)
        {
            _calculator = calculator;
            _bag = bag;
            _weigthVisitor = weigthVisitor;
            Power = 10;
            Speed = 10;
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public IHorse ToRide(IPlayer player)
        {
            Rider = player;
            CalcHorsePower();
            return this;
        }

        public IHorse Load<T>(T item)
        {
            _bag.Put(item);
            CalcHorsePower();
            return this;
        }

        // todo !
        public ReadOnlyCollection<IWeapon> Baggage()
        {            
            return _bag.GetAll<IWeapon>().ToList().AsReadOnly();
        }

        private void CalcHorsePower()
        {
            _calculator.Calculate(Power, Speed, Accept(_weigthVisitor));
            Power = _calculator.Power;
            Speed = _calculator.Speed;
        }
    }

    public interface IHorsePowerCalculator
    {
        void Calculate(float power, float speed, float weight);
        float Speed { get; }
        float Power { get; }
    }

    public class HorsePowerCalculator : IHorsePowerCalculator
    {
        public void Calculate(float power, float speed, float weight)
        {
            Power = power - weight / power;
            Speed = speed - weight / power;
        }

        public float Speed { get; private set; }
        public float Power { get; private set; }
    }    
}