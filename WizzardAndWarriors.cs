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
            IPlayer player = new Warrior(new CommonBag());
            player.GiveHim(new Axe());
        }

        [TestMethod]
        public void CalcWeigthTest()
        {
            IPlayer player = new Warrior(new CommonBag());
            player.GiveHim(new Axe());

            var weigth = player.Compute(new ComputeWeigth());

            Assert.AreEqual(0.4f, weigth);
        }

        [TestMethod]
        public void RidedHorse()
        {
            var warrior = new Warrior(new CommonBag());
            var horse = new Horse(new CommonBag());

            var ride = new EquipHorse(new HorsePower(), new ComputeWeigth());
            ride.EquipBy(horse, warrior);

            Assert.AreEqual(Math.Round(10.0f, 2), Math.Round(horse.Power, 2));
            Assert.AreEqual(Math.Round(10.0f, 2), Math.Round(horse.Speed, 2));
        }

        [TestMethod]
        public void HorseWithBaggage()
        {
            var horse = new Horse(new CommonBag());
            
            var equip = new EquipHorse(new HorsePower(), new ComputeWeigth());
            equip
                // todo horse !
                .EquipBy(horse, new Axe())
                .EquipBy(horse, new Axe());

            Assert.AreEqual(Math.Round(9.92f, 2), Math.Round(horse.Power, 2));
            Assert.AreEqual(Math.Round(9.92f, 2), Math.Round(horse.Speed, 2));
        }
    }

    public interface IWeightiness
    {
        T Compute<T>(IComputeWeigth<T> compute);        
    }

    public interface IWeapon : IWeightiness
    {
    }

    public interface IPlayer : IWeightiness
    {
        void GiveHim<T>(T item);        
    }

    public interface IHorse : IWeightiness
    {
        IPlayer Rider { get; set; }
        IBag Bag { get; }
    }

    public interface IBag : IWeightiness
    {
        IBag Put<T>(T item);
        IEnumerable<T> GetAll<T>();
    }

    public interface IComputeWeigth<out T>
    {
        T Compute(Warrior warrior);
        T Compute(Horse horse);
        T Compute(Axe axe);
        T Compute(CommonBag bag);
    }

    public interface IHorseEquip<in T>
    {
        IHorseEquip<T> EquipBy(Horse horse, T item);
    }

    public interface IHorsePower
    {
        void Compute(float power, float speed, float weight);
        float Speed { get; }
        float Power { get; }
    }    

    public class ComputeWeigth : IComputeWeigth<float>
    {
        public float Compute(Warrior warrior)
        {
            return warrior.Bag.Compute(this);
        }

        public float Compute(Horse horse)
        {
            var riderWeigth = horse.Rider == null ? 0.0f : horse.Rider.Compute(this);
            var horseWeigth = horse.Bag.Compute(this);
            return riderWeigth + horseWeigth;
        }

        public float Compute(Axe axe)
        {
            return axe.Weight;
        }

        public float Compute(CommonBag bag)
        {
            // todo bag items is generic !
            return bag.GetAll<IWeapon>().Sum(s => s.Compute(this));
        }
    }

    public class EquipHorse: IHorseEquip<IPlayer>, IHorseEquip<IWeapon>
    {
        private readonly IHorsePower _horsePower;
        private readonly IComputeWeigth<float> _computeWeigth;

        public EquipHorse(IHorsePower horsePower, IComputeWeigth<float> computeWeigth)
        {
            _horsePower = horsePower;
            _computeWeigth = computeWeigth;
        }

        public IHorseEquip<IPlayer> EquipBy(Horse horse, IPlayer player)
        {
            horse.Rider = player;
            ComputeHorsePower(horse, player);
            return this;
        }

        public IHorseEquip<IWeapon> EquipBy(Horse horse, IWeapon weapon)
        {
            horse.Bag.Put(weapon);
            ComputeHorsePower(horse, weapon);
            return this;
        }

        private void ComputeHorsePower(Horse horse, IWeightiness weightiness)
        {
            _horsePower.Compute(horse.Power, horse.Speed, weightiness.Compute(_computeWeigth));
            horse.Power = _horsePower.Power;
            horse.Speed = _horsePower.Speed;
        }
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

        public T Compute<T>(IComputeWeigth<T> compute)
        {
            return compute.Compute(this);
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

        public IBag Bag
        {
            get { return _bag; }
        }

        public T Compute<T>(IComputeWeigth<T> compute)
        {
            return compute.Compute(this);
        }
    }

    public class Axe : IWeapon
    {
        public T Compute<T>(IComputeWeigth<T> compute)
        {
            return compute.Compute(this);
        }

        public float Weight
        {
            get { return 0.4f; }
        }
    }    

    public class Horse : IHorse
    {        
        public IPlayer Rider { get; set; }
        public IBag Bag { get; private set; }

        public float Power { get; set; }
        public float Speed { get; set; }

        public Horse(IBag bag)
        {
            Bag = bag;
            Power = 10;
            Speed = 10;
        }

        public T Compute<T>(IComputeWeigth<T> compute)
        {
            return compute.Compute(this);
        }
    }

    public class HorsePower : IHorsePower
    {
        public void Compute(float power, float speed, float weight)
        {
            Power = power - weight / power;
            Speed = speed - weight / power;
        }

        public float Speed { get; private set; }
        public float Power { get; private set; }
    }    
}