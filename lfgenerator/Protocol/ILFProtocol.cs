using LFGenerator2.Transport;

namespace LFGenerator2.Protocol
{
    /// <summary>
    /// Протокол обмена с Генератором многофункциональным СПАН 468789.001 
    /// </summary>
    public interface ILfProtocol
    {
        /// <summary>
        /// Получить идентификатор устройства
        /// </summary>
        /// <returns></returns>
        WriteRequest GetId();
        /// <summary>
        /// Получить версию прошивки
        /// </summary>
        /// <returns></returns>
        WriteRequest GetSoftwareVersion();
        /// <summary>
        /// Включить 2-й канал в режим генерации синусоидального сигнала заданной частоты и амплитуды.
        /// </summary>
        /// <returns></returns>
        WriteRequest SetChannel2(ushort frequency, ushort rms);
        /// <summary>
        ///     Задаёт величину напряжения на выходе генератора постоянного напряжения
        /// </summary>
        /// <param name="voltage">Величина напряжения, 0..10000 мВ</param>
        WriteRequest SetConstantVoltage(ushort voltage);
        /// <summary>
        ///     Включает канал 2 в режим генерации шума
        /// </summary>
        /// <param name="amplitude">Максимальная амплитуда шума, 0..1000 мВ</param>
        WriteRequest SetChannel2Noise(ushort amplitude);
        /// <summary>
        ///     Включает 2-й канал в режим генерации сигнала, представляющего собой сумму двух синусоидальных сигналов 
        /// с заданными характеристиками
        /// </summary>
        /// <param name="frequency1">Частота 1-й составляющей, 0..65535 Гц</param>
        /// <param name="amplitude1">Амплитуда 1-й составляющей, 0..1000 мВ</param>
        /// <param name="frequency2">Частота 2-й составляющей, 0..65535 Гц</param>
        /// <param name="amplitude2">Амплитуда 2-й составляющей, 0..1000 мВ</param>
        WriteRequest SetChannel2Summator(ushort frequency1, ushort amplitude1, ushort frequency2, ushort amplitude2);
        /// <summary>
        ///     Включает 1-й канал в режим генерации синусоидального сигнала заданной частоты и амплитуды
        /// </summary>
        /// <param name="frequency">Частота, 0..65535 Гц</param>
        /// <param name="rms">Амплитуда, 0..65000 мВ</param>
        WriteRequest SetChannel1(ushort frequency, ushort rms);
        /// <summary>
        ///     Отключает и сбрасывает настройки всех каналов
        /// </summary>
        WriteRequest ResetAllChannels();
        /// <summary>
        ///     Коэффициент K для канала 1
        /// </summary>
        WriteRequest SetChannel1K(float k);
        /// <summary>
        ///     Коэффициент B для канала 1
        /// </summary>
        WriteRequest SetChannel1B(ushort b1);
        /// <summary>
        ///     Коэффициент K для канала 2
        /// </summary>
        WriteRequest SetChannel2K(float k);
        /// <summary>
        ///     Коэффициент B для канала 2
        /// </summary>
        WriteRequest SetChannel2B(ushort b1);
        /// <summary>
        ///     Коэффициент K для канала постоянного напряжения
        /// </summary>
        WriteRequest SetConstantK(float k);
        /// <summary>
        ///     Коэффициент B для канала постоянного напряжения
        /// </summary>
        WriteRequest SetConstantB(ushort b);
        /// <summary>
        /// Вывести записанные коэффициенты.
        /// </summary>
        WriteRequest GetCalibration();
    }
}
