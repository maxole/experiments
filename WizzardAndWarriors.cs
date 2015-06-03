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
    }

    public interface IWeapon
    {
        float Weight { get; }
    }

    public interface IPlayer
    {
        void GiveHim<T>(T item);
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

    public class Axe : IWeapon {
        public float Weight { get { return 0.4f; } }
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
            Weight = bag.GetAll<IWeapon>().Sum(w=>w.Weight);
        }
    }
}
