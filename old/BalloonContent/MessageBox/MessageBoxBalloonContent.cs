using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Core.UI
{
    public class MessageBoxBalloonContent : ContentControl, IHaveBalloonContent
    {
        static MessageBoxBalloonContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxBalloonContent),
                new FrameworkPropertyMetadata(typeof(MessageBoxBalloonContent)));
        }

        public MessageBoxBalloonContent()
        {
            AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(Button_Click));
        }
        
        public event EventHandler OnPopuping;
        public event EventHandler OnClosing;

        public void Popup()
        {
            var handler = OnPopuping;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public void Close()
        {
            var handler = OnClosing;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = e.OriginalSource as Button;

            if (button == null)
                return;

            var handle = false;
            switch (button.Name)
            {
                case "PART_SkipButton":
                    Result = BalloonResult.Skip;
                    handle = true;
                    break;
                case "PART_NoButton":
                    Result = BalloonResult.No;
                    handle = true;
                    break;
                case "PART_YesButton":
                    Result = BalloonResult.Yes;
                    handle = true;
                    break;
                case "PART_CancelButton":
                    Result = BalloonResult.Cancel;
                    handle = true;
                    break;
                case "PART_OkButton":
                    Result = BalloonResult.Ok;
                    handle = true;
                    break;
            }
            if (handle)
                Close();

            e.Handled = true;
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            "Message", typeof(string), typeof(MessageBoxBalloonContent), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ResultProperty = DependencyProperty.Register(
            "Result", typeof(BalloonResult), typeof(MessageBoxBalloonContent),
            new PropertyMetadata(BalloonResult.None));

        public static readonly DependencyProperty WaitingButtonsProperty = DependencyProperty.Register(
            "WaitingButtons", typeof(BalloonButtons), typeof(MessageBoxBalloonContent),
            new PropertyMetadata(BalloonButtons.Ok, OnWaitingButtonsChanged));

        private static void OnWaitingButtonsChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ((MessageBoxBalloonContent)d).OnWaitingButtonsChanged();
        }

        public BalloonButtons WaitingButtons
        {
            get { return (BalloonButtons) GetValue(WaitingButtonsProperty); }
            set { SetValue(WaitingButtonsProperty, value); }
        }

        public BalloonResult Result
        {
            get { return (BalloonResult) GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public string Message
        {
            get { return (string) GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        private void OnWaitingButtonsChanged()
        {
            //ChangeVisualState(WaitingButtons.ToString(), true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState(WaitingButtons.ToString(), true);            
        }

        protected void ChangeVisualState(string name, bool useTransitions)
        {            
            var grid = GetTemplateChild("PART_Grid") as Grid;
            if (grid == null) return;
            VisualStateManager.GoToElementState(grid, name, useTransitions);
        }
    }
}