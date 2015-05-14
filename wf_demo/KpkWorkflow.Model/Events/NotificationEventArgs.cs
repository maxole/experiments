using System;
using Sparc.Kpk12.Domain;

namespace KpkWorkflow.Model
{
    public class NotificationEventArgs : EventArgs
    {
        public string MessageKey { get; set; }
    }
}