using System.ComponentModel;
using Core.Attributes;
using Core.Configuration;

namespace Lambda.GenH30
{  
    /// <summary>
    /// 
    /// </summary>    
    [TypeRegistration(typeof(LambdaUnit))]
    public sealed class LambdaUnit : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>        
        /// <param name="configuration"></param>
        public LambdaUnit(IConfigurationManager configuration)
        {
            Configre(configuration);
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Address { get; private set; }

        /// <summary>
        /// Результат опроса доступности модуля
        /// </summary>
        public bool Availability { get; private set; }        

        /// <summary>
        /// 
        /// </summary>
        public string ExpectedIdentifier { get; private set; }
        /// <summary>
        /// Измеренное U, В
        /// </summary>
        public float MeasuredVoltage { get; private set; }

        /// <summary>
        /// Установленное U, В
        /// </summary>
        public float SettedVoltage { get; private set; }
        /// <summary>
        /// Измеренное I, В
        /// </summary>
        public float MeasuredCurrent { get; private set; }
        /// <summary>
        /// Установленное I, В (порог)
        /// </summary>
        public float SettedCurrent { get; private set; }
        /// <summary>
        /// Верхний порог U, В
        /// </summary>
        public float Ovp { get; private set; }
        /// <summary>
        /// Нижний порог U, В
        /// </summary>
        public float Uvl { get; private set; }
        /// <summary>
        /// заданное значение напряжения
        /// </summary>
        /// <remarks>
        /// читается из конфига
        /// </remarks>
        public float Voltage { get; private set; }
        /// <summary>
        /// заданное значение тока
        /// </summary>
        /// <remarks>
        /// читается из конфига
        /// </remarks>
        public float Current { get; private set; }

        public void Available(bool value)
        {
            Availability = value;
        }

        public void ClearMeasuredParameters()
        {
            MeasuredParameters(0, 0, 0, 0, 0, 0);
        }

        public void MeasuredParameters(float measuredVoltage, float settedVoltage, float measuredCurrent, float settedCurrent, float ovp, float uvl)
        {
            MeasuredVoltage = measuredVoltage;
            SettedVoltage = settedVoltage;
            MeasuredCurrent = measuredCurrent;
            SettedCurrent = settedCurrent;
            Ovp = ovp;
            Uvl = uvl;
        }

        public void Configre(IConfigurationManager configuration)
        {
            var config = configuration.GetCustomConfig<Config>();

            Voltage = config.Voltage;
            Current = config.Current;
            ExpectedIdentifier = config.Idn;
            Address = config.Address;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}