using System;

namespace Lambda.GenH30
{
    public static class LambdaUnitExt
    {
        /// <summary>
        /// если измеренное напряжение равно 27,000В±3%
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool MeasureVoltage3(this LambdaUnit unit)
        {
            var min = unit.Voltage - unit.Voltage*0.03f;
            var max = unit.Voltage + unit.Voltage*0.03f;
            return unit.MeasuredVoltage <= max && unit.MeasuredVoltage >= min;
        }

        /// <summary>
        /// установленный ток не более 6,0А
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool MeasureCurrent(this LambdaUnit unit)
        {
            return unit.MeasuredCurrent <= unit.Current;
        }

        /// <summary>
        /// если измеренное напряжение между 27,000В+3%,27,000В+10%
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool MeasureVoltagMore3Less10(this LambdaUnit unit)
        {
            var min = unit.Voltage + unit.Voltage*0.03f;
            var max = unit.Voltage + unit.Voltage*0.10f;
            return unit.MeasuredVoltage > min && unit.MeasuredVoltage < max;
        }

        /// <summary>
        /// если измеренное напряжение меньше 27,000В-3%
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool MeasureVoltagLess3(this LambdaUnit unit)
        {
            var max = unit.Voltage - unit.Voltage*0.03f;
            return unit.MeasuredVoltage < max;
        }

        /// <summary>
        /// если измеренное напряжение больше 27,000В+3%
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool MeasureVoltagMore10(this LambdaUnit unit)
        {
            var max = unit.Voltage + unit.Voltage*0.10f;
            return unit.MeasuredVoltage > max;
        }

        /// <summary>
        /// равно ли установленная сила тока заданной в конфигурации
        /// </summary>
        public static bool IsSettedVoltage(this LambdaUnit unit)
        {
            return Math.Abs(unit.SettedVoltage - unit.Voltage) < 0.005;
        }

        /// <summary>
        /// равно ли установленная напряжение заданной в конфигурации
        /// </summary>
        public static bool IsSettedCurrent(this LambdaUnit unit)
        {
            return Math.Abs(unit.SettedCurrent - unit.Current) < 0.005;
        }
    }
}