using System;
using System.Activities.Tracking;
using System.Diagnostics;
using System.Text;
using KpkWorkflow.Model;

namespace Sparc.Kpk12.Domain
{
    public class DebugTrackingParticipant : TrackingParticipant, ITrackingParticipant
    {
        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            var sb = new StringBuilder(record.ToString());
            //foreach (var annotation in record.Annotations)
            //    sb.AppendLine(annotation.Key + " " + annotation.Value);
            Debug.WriteLine(sb.ToString());
        }

        void ITrackingParticipant.Track(TrackingRecord record, TimeSpan timeout)
        {
            Track(record, timeout);
        }
    }
}
