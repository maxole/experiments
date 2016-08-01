/// <summary>
    /// форматирование строки калибровочных коэфициентов
    /// </summary>
    /// <remarks>
    /// пример входного буфера
    /// K1=931D1B3E\r\nB1=0100\r\nK2=4B1C6A3F\r\nB2=0000\r\nKDC=FB85CD40\r\nBDC=0D00\r\nFREQCORR1=FFFF\r\nFREQCORR2=FFFF
    /// </remarks>
    public static class CoefficientFormatter
    {
        /// <summary>
        /// преобразовать буфер данных
        /// </summary>
        /// <param name="buffer">данные</param>
        /// <returns></returns>
        public static CoefficientItem Format(byte[] buffer)
        {
            if (buffer == null)
                return new CoefficientItem();

            var str = Encoding.ASCII.GetString(buffer);

            if (string.IsNullOrEmpty(str))
                return new CoefficientItem();

            // конвертеры
            Func<byte[], float> tosingle = b => BitConverter.ToSingle(b, 0);
            Func<byte[], short> toint16 = b => BitConverter.ToInt16(b, 0);

            // маппер коэффициентов
            var mapper = new Dictionary<string, Func<byte[], CoefficientItem, CoefficientItem>>
            {
                {"K1", (b, item) =>{ item.K1 = tosingle(b); return item;}},
                {"K2", (b, item) =>{ item.K2 = tosingle(b); return item;}},
                {"KDC", (b, item) =>{ item.KDC = tosingle(b); return item;}},
                {"B1", (b, item) =>{ item.B1 = toint16(b); return item;}},
                {"B2", (b, item) =>{ item.B2 = toint16(b); return item;}},
                {"BDC", (b, item) =>{ item.BDC = toint16(b); return item;}},
                {"FREQCORR1", (b, item) =>{ item.FREQCORR1 = toint16(b); return item;}},
                {"FREQCORR2", (b, item) =>{ item.FREQCORR2 = toint16(b); return item;}},
            };
            // паттерн парсинг key=value значения
            const string pattern = @"(?<Key>[^\n\r]+)(?:\=)(?<Value>[^\n\r]+)(?:\|?)";
            // распарсить строку, получить словарь, сделать маппирование
            return Regex.Matches(str, pattern)
                        .Cast<Match>()
                        .ToDictionary(m => m.Groups["Key"].Value, m => StringToByteArray(m.Groups["Value"].Value))
                        .Aggregate(new CoefficientItem(), (item, pair) =>
                        {
                            if (!mapper.ContainsKey(pair.Key))
                                throw new Exception("Не ожидаемый параметр '" + pair.Key + "'");
                            return mapper[pair.Key](pair.Value, item);
                        });
        }

        /// <summary>
        /// преобразовать строку в массив байт
        /// </summary>
        /// <param name="str">строка</param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string str)
        {
            const int chunkSize = 2;
            return Enumerable
                .Range(0, str.Length/chunkSize)
                .Select(i =>
                {
                    var t = str.Substring(i*chunkSize, chunkSize);
                    return byte.Parse(t, NumberStyles.HexNumber, CultureInfo.CurrentCulture);
                })
                .ToArray();
        }
    }