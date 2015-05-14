using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using KpkWorkflow.Model;
using KpkWorkflow.WPF.Components;
using Sparc.Kpk12.Domain;

namespace KpkWorkflow.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var eventBroker = new EventBroker();
            var messageRepository = new FileMessageProvider();

            Type[] events =
            {
                typeof(MessageNofiticationEvent),
                typeof(VisualNotificationEvent)
            };            
            var eventAgent = new EventAgent();
            foreach (var eventType in events)
                eventAgent.Register(eventType);

            var debugTracking = new DebugTrackingParticipant();
            var agent = new MessageAgent();

            IPrototype[] messages =
            {
                new MessageInfo(messageRepository),
                new PowerSupplayVisualComponent(eventBroker, agent, eventAgent), 
                new MessageError(messageRepository, eventBroker), 
            };

            foreach (var message in messages)
                agent.RegisterPrototype(message);

            RegisterComponentTemplate(messages);

            var workflowProvide = new WorkflowProvider(SynchronizationContext.Current, eventBroker, debugTracking, agent, eventAgent);

            var model = new WorkflowRunner(workflowProvide, eventBroker);            

            var componentViewCollection = new ComponentViewCollection();
            var userNotificationView = new UserNotificationView();            

            var view = new EnableViewModel(model, componentViewCollection, userNotificationView, eventBroker);            

            eventBroker.Register(componentViewCollection);
            eventBroker.Register(userNotificationView);
            eventBroker.Register(view);
            eventBroker.Register(model);            

            // ioc for each IStartable.Start()

            //this.Closing += view.ViewClosing;
            //this.Closed += view.ViewClosed;

            DataContext = view;
        }

        private void RegisterComponentTemplate(params IPrototype[] messages)
        {
            foreach (var prototype in messages)
            {
                var type = prototype.GetType();
                var attribute = type.GetCustomAttributes(typeof (MessageTypeViewAttribute), false).Cast<MessageTypeViewAttribute>().Single();

                var template = new DataTemplate
                {
                    DataType = type,
                    VisualTree = new FrameworkElementFactory(attribute.View)
                };

                Application.Current.Resources.Add(new DataTemplateKey(type), template);   
            }
        }       
    }
}
