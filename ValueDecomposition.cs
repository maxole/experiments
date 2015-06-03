using System.Collections.Generic;
using System.Linq;

/*
     [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var d = new Splitter(new[] {0.5f, 1.0f, 1.5f, 2.0f, 2.5f});
            var actual = d.Split(3);
            var expected = new[] { 0.5f, 0f, 0f, 0f, 2.5f };
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var d = new Splitter(new[] { 0.5f, 1.0f, 1.5f, 2.0f, 2.5f });
            var actual = d.Split(9);
            var expected = new[] { 0.5f, 1.0f, 1.5f, 2.0f, 2.5f };
            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
 */
namespace FloatDecomposition
{
    /// <summary>
    /// разложение цисла на слагаемые использую заданный массив 
    /// допустимых чисел, каждое число их набора 
    /// может использоваться один раз
    /// </summary>    
    public class Splitter 
    {
        private struct Bucket
        {
            public bool IsUsed;
            public readonly float Value;

            public Bucket(float value)
            {
                IsUsed = false;
                Value = value;
            }

            public void Reset()
            {
                IsUsed = false;
            }

            public void Set()
            {
                IsUsed = true;
            }
        }

        private readonly Bucket[] _bucket;

        public Splitter(IEnumerable<float> array)
        {
            _bucket = array.Select(r => new Bucket(r)).ToArray();
        }

        private void Reset()
        {
            for (var i = _bucket.Length - 1; i >= 0; i--)
                _bucket[i].Reset();
        }

        private void DoSplit(float value)
        {
            for (var i = _bucket.Length - 1; i >= 0; i--)
            {
                if (_bucket[i].IsUsed)
                    continue;
                if (!(_bucket[i].Value <= value))
                    continue;
                value -= _bucket[i].Value;                
                _bucket[i].Set();
                if (value <= 0)
                    break;
                DoSplit(value);
                break;
            }
        }

        public IEnumerable<float> Split(float value)
        {
            Reset();
            DoSplit(value);
            foreach (var t in _bucket)
                if (!t.IsUsed)
                    yield return 0;
                else
                    yield return t.Value;
        }
    }
}