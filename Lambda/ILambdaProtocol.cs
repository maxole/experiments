namespace Lambda.GenH30
{
    public interface ILambdaProtocol
    {
        /// <summary>
        /// ������� ������������ ��������-��.
        /// ���� ���������� ����������, ��� ����� ������ �����, ����� �����-������.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        WriteResponse Adr(byte address);

        /// <summary>
        /// ������� ������ ���� ����������.
        /// </summary>
        /// <returns></returns>
        WriteResponse Idn();

        /// <summary>
        /// ��������� �����-���� ���������� (������ U��� �� ������� ��)
        /// </summary>
        /// <remarks>
        /// OUT 0
        /// </remarks>
        /// <returns></returns>
        WriteResponse OutOn();

        /// <summary>
        /// ���������� �����-���� ���������� (������ U��� �� ������� ��)
        /// </summary>
        /// <remarks>
        /// OUT 1
        /// </remarks>
        /// <returns></returns>
        WriteResponse OutOff();

        /// <summary>
        /// ������ ������������� � �������-��� ����������:
        /// ���������� U, �
        /// ������������� U, �
        /// ���������� I, �
        /// ������������� I, � (�����)
        /// ������� ����� U, �
        /// ������ ����� U, �
        /// </summary>
        /// <returns></returns>
        WriteResponse Dvc();

        /// <summary>
        /// ��������� �������� ��������� ����������
        /// </summary>
        /// <returns></returns>
        WriteResponse Pv(float value);

        /// <summary>
        /// ��������� �������� ���� �������-����� ������� ������
        /// </summary>
        /// <returns></returns>
        WriteResponse Pc(float value);
    }
}