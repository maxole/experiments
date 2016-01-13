using System;
using System.Globalization;

namespace Lambda.GenH30
{
    public static class ProtocolExt
    {
        public static void Adr(this ILambdaProtocol lambdaProtocol, string identifier,
                               byte address, float voltage)
        {
            var response = lambdaProtocol.Adr(address);

            if (response.Error != null)
                throw new LambdaFailureException(
                    string.Format(
                        Properties.Resources.NotFoundPowerSource,
                        identifier, address, VoltageRound(voltage)),
                    response.Error);
        }

        public static void OutOn(this ILambdaProtocol lambdaProtocol, string identifier, byte address, float voltage)
        {
            var response = lambdaProtocol.OutOn();

            if (response.Error != null)
                throw new LambdaFailureException(string.Format(Properties.Resources.NotFoundPowerSource,
                    identifier, address, VoltageRound(voltage)), response.Error);
        }

        public static void OutOff(this ILambdaProtocol lambdaProtocol, string identifier, byte address, float voltage)
        {
            var response = lambdaProtocol.OutOff();

            if (response.Error != null)
                throw new LambdaFailureException(string.Format(Properties.Resources.NotFoundPowerSource,
                    identifier, address, VoltageRound(voltage)), response.Error);
        }

        public static void Idn(this ILambdaProtocol lambdaProtocol, string identifier, byte address)
        {
            var response = lambdaProtocol.Idn();

            if (!response.Response.Equals(identifier))
                throw new LambdaFailureException(string.Format(Properties.Resources.NotFoundPowerSourceWithResponse,
                    identifier, address, response.Response), response.Error);
        }

        public static float[] Dvc(this ILambdaProtocol lambdaProtocol, string identifier, byte address, float voltage)
        {            
            var response = lambdaProtocol.Dvc();
            if (response.Error != null)
                throw new LambdaFailureException(string.Format(Properties.Resources.NotFoundPowerSource,
                    identifier, address, VoltageRound(voltage)), response.Error);
            var strings = response.Response.Split(',');
            return new[]
            {
                ConvertToFloat(strings[0]),
                ConvertToFloat(strings[1]),
                ConvertToFloat(strings[2]),
                ConvertToFloat(strings[3]),
                ConvertToFloat(strings[4]),
                ConvertToFloat(strings[5])
            };
        }

        public static void Pv(this ILambdaProtocol lambdaProtocol, string identifier, byte address, float voltage)
        {
            var response = lambdaProtocol.Pv(voltage);
            if (response.Error != null)
                throw new LambdaFailureException(string.Format(Properties.Resources.NotFoundPowerSource,
                    identifier, address, VoltageRound(voltage)), response.Error);
        }

        public static void Pc(this ILambdaProtocol lambdaProtocol, string identifier, byte address, float voltage, float current)
        {
            var response = lambdaProtocol.Pc(current);
            if (response.Error != null)
                throw new LambdaFailureException(string.Format(Properties.Resources.NotFoundPowerSource,
                    identifier, address, VoltageRound(voltage)), response.Error);
        }

        private static float VoltageRound(float voltage)
        {
            return (float)Math.Round(voltage, 2);
        }

        private static readonly NumberFormatInfo NumberFormat = new NumberFormatInfo
        {
            NumberDecimalSeparator = "."
        };

        private static float ConvertToFloat(string answer)
        {
            return Convert.ToSingle(answer, NumberFormat);
        }
    }
}
