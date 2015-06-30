namespace Lambda.GenH30
{
    public interface ILambdaProtocol
    {
        /// <summary>
        /// Команда тестирования аппарату-ры.
        /// Если устройство отсутсвует, или имеет другой адрес, ответ отсут-ствует.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        WriteResponse Adr(byte address);

        /// <summary>
        /// Команда опроса типа аппаратуры.
        /// </summary>
        /// <returns></returns>
        WriteResponse Idn();

        /// <summary>
        /// Включение выход-ного напряжения (подача Uвых на зажимах ИП)
        /// </summary>
        /// <remarks>
        /// OUT 0
        /// </remarks>
        /// <returns></returns>
        WriteResponse OutOn();

        /// <summary>
        /// выключение выход-ного напряжения (снятие Uвых на зажимах ИП)
        /// </summary>
        /// <remarks>
        /// OUT 1
        /// </remarks>
        /// <returns></returns>
        WriteResponse OutOff();

        /// <summary>
        /// Чтение установленных и измерен-ных параметров:
        /// Измеренное U, В
        /// Установленное U, В
        /// Измеренное I, В
        /// Установленное I, В (порог)
        /// Верхний порог U, В
        /// Нижний порог U, В
        /// </summary>
        /// <returns></returns>
        WriteResponse Dvc();

        /// <summary>
        /// Установка значения выходного напряжения
        /// </summary>
        /// <returns></returns>
        WriteResponse Pv(float value);

        /// <summary>
        /// Установка значения тока срабаты-вания токовой защиты
        /// </summary>
        /// <returns></returns>
        WriteResponse Pc(float value);
    }
}