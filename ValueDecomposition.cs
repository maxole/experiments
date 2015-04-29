using System.Collections.Generic;
using System.Linq;

namespace FloatDecomposition
{
    /// <summary>
    /// разложение числа на сумму из цисел в заданном наборе
    /// </summary>
    public class ValueDecomposition 
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

        public ValueDecomposition(IEnumerable<float> array)
        {
            _bucket = array.Select(r => new Bucket(r)).ToArray();
        }

        private void Reset()
        {
            for (var i = _bucket.Length - 1; i >= 0; i--)
                _bucket[i].Reset();
        }

        private void DoDecomposition(float value)
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
                DoDecomposition(value);
                break;
            }
        }

        public IEnumerable<float> Decomposition(float value)
        {
            Reset();
            DoDecomposition(value);
            foreach (var t in _bucket)
                if (!t.IsUsed)
                    yield return 0;
                else
                    yield return t.Value;
        }
    }
}