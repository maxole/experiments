using System;
using System.Windows;
using System.Windows.Controls;

namespace Core.UI
{
    [TemplateVisualState(Name = VisualStates.StateVisible, GroupName = VisualStates.GroupVisibility)]
    [TemplateVisualState(Name = VisualStates.StateHidden, GroupName = VisualStates.GroupVisibility)]
    public class BalloonControl : ContentControl
    {
        static BalloonControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BalloonControl),
                new FrameworkPropertyMetadata(typeof(BalloonControl)));
        }

        public static readonly DependencyProperty OverlayStyleProperty = DependencyProperty.Register(
            "OverlayStyle", typeof(Style), typeof(BalloonControl), new PropertyMetadata(null));
        
        public static readonly DependencyProperty BalloonContentTemplateProperty = DependencyProperty.Register(
            "BalloonContentTemplate", typeof(DataTemplate), typeof(BalloonControl), new PropertyMetadata(null));

        public static readonly DependencyProperty BalloonContentProperty = DependencyProperty.Register(
            "BalloonContent", typeof(IHaveBalloonContent), typeof(BalloonControl), new PropertyMetadata(default(object), OnWaitingContentChanged));

        private static void OnWaitingContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ((BalloonControl) d).OnWaitingContentChanged(args.NewValue);
        }

        private void OnWaitingContentChanged(object content)
        {
            var c = content as IHaveBalloonContent;
            if (c == null)
                throw new NotImplementedException("IHaveBalloonContent");
            c.OnClosing += (s, e) => ChangeVisualState(false);            
            c.OnPopuping += (s, e) => ChangeVisualState(true);
        }

        public IHaveBalloonContent BalloonContent
        {
            get { return (IHaveBalloonContent)GetValue(BalloonContentProperty); }
            set { SetValue(BalloonContentProperty, value); }
        }        

        public DataTemplate BalloonContentTemplate
        {
            get { return (DataTemplate) GetValue(BalloonContentTemplateProperty); }
            set { SetValue(BalloonContentTemplateProperty, value); }
        }

        public Style OverlayStyle
        {
            get { return (Style) GetValue(OverlayStyleProperty); }
            set { SetValue(OverlayStyleProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState(false);
        }

        protected virtual void ChangeVisualState(bool isPopup, bool useTransitions = false)
        {
            VisualStateManager.GoToState(this, isPopup ? VisualStates.StateVisible : VisualStates.StateHidden, useTransitions);
        }
    }
}
