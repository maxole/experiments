using System;

namespace OCR
{
    public interface IAlphabet
    {
        int Labels { get; }
        string[] Data { get; }
        void Initialize(string data);
    }

    public class Alphabet : IAlphabet
    {
        private string[] _data;
        private string _raw;

        public Alphabet()
        {
            Data = new string[] {};
        }

        public int Labels
        {
            get { return _data.Length; }
        }

        public string[] Data
        {
            get { return _data; }
            private set { _data = value; }
        }

        public void Initialize(string data)
        {
            _raw = data;
            Data = data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        public override string ToString()
        {
            return _raw;
        }
    }
}