namespace Hardware.AwGenerators.Sparc.Protocol
{
    /// <summary>
    ///     Коды команд и ответов генератора
    /// </summary>
    public enum Command : byte
    {
        /// <summary>
        ///     Получить идентификатор устройства
        /// </summary>
        GetDeviceId = 0xfd,

        /// <summary>
        ///     Получить версию прошивки
        /// </summary>
        GetSoftwareVersion = 0x1e,

        /// <summary>
        ///     Включить 2-й канал в режим генерации синусоидального 
        /// сигнала заданной частоты и амплитуды
        /// </summary>
        SetChannel2 = 0x01,

        /// <summary>
        ///     Задать величину напряжения на выходе генератора 
        /// постоянного напряжения
        /// </summary>
        SetConstantVoltage = 0x02,

        /// <summary>
        ///     Включить 2-й канал в режим генератора шума с 
        /// заданной максимальной амплитудой
        /// </summary>
        SetChannel2Noise = 0x03,

        /// <summary>
        ///     Включить 2-й канал в режим генерации сигнала, 
        /// представляющего собой сумму двух синусоидальных сигналов
        /// с заданными характеристиками
        /// </summary>
        SetChannel2Hybrid = 0x04,

        /// <summary>
        ///     Включить 1-й канал в режим генерации синусоидального
        /// сигнала заданной частоты и амплитуды
        /// </summary>
        SetChannel1 = 0x05,

        /// <summary>
        ///     Отключение и сброс настроек всех каналов
        /// </summary>
        ResetAllChannels = 0x06,

        /// <summary>
        ///     Задать коэффициент k для 1-го канала
        /// </summary>
        SetChannel1K = 0x07,

        /// <summary>
        ///     Задать коэффициент b для 1-го канала
        /// </summary>
        SetChannel1B = 0x08,

        /// <summary>
        ///     Задать коэффициент k для 2-го канала
        /// </summary>
        SetChannel2K = 0x09,

        /// <summary>
        ///     Задать коэффициент b для 2-го канала
        /// </summary>
        SetChannel2B = 0x1a,

        /// <summary>
        ///     Задать коэффициент k для канала постоянного 
        /// напряжения
        /// </summary>
        SetConstantK = 0x1b,

        /// <summary>
        ///     Задать коэффициент b для 1-го постоянного 
        /// напряжения
        /// </summary>
        SetConstantB = 0x1c,

        /// <summary>
        ///     Задать поправочный коэффициент по частоте 
        /// для 1-го канала
        /// </summary>
        SetChannel1FrequencyCorrection = 0x20,

        /// <summary>
        ///     Задать поправочный коэффициент по частоте 
        /// для 2-го канала
        /// </summary>
        SetChannel2FrequencyCorrection = 0x21,

        /// <summary>
        ///     Вывести записанные коэффициенты
        /// </summary>
        GetCalibration = 0x1d
    }
}