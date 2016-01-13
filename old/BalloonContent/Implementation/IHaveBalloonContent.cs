using System;

namespace Core.UI
{
    /// <summary>
    /// интерфейс управление содержимым всплывающего окна
    /// </summary>
    public interface IHaveBalloonContent
    {
        /// <summary>
        /// отобразить окно с содержимым
        /// </summary>
        void Popup();
        /// <summary>
        /// скрыть окно с содержимым
        /// </summary>
        void Close();
        /// <summary>
        /// текст отображаемый в всплывающем окне
        /// </summary>
        string Message { get; set; }
        /// <summary>
        /// Реакция пользователя
        /// </summary>
        BalloonResult Result { get; set; }
        event EventHandler OnClosing;
        event EventHandler OnPopuping;        
    }
}
