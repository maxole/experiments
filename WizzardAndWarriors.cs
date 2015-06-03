using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject5_1
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
    }

    public interface IWeapon
    {
    }

    public interface IPlayer
    {
        void GiveHim<T>(T item);
    }

    public interface IBag
    {
        IBag Put<T>(T item);
    }

    public class WarrionBag : IBag
    {
        private readonly HashSet<object> _hashSet = new HashSet<object>();

        public IBag Put<T>(T item)
        {
            _hashSet.Add(item);
            return this;
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
    }

    public class Axe : IWeapon { }
}
