using System;
using System.Activities.Tracking;

namespace Sparc.Kpk12.Domain
{
    public interface ITrackingParticipant
    {
        void Track(TrackingRecord record, TimeSpan timeout);
    }
}