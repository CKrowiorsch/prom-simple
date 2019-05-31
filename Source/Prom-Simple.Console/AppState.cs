using System;
using System.ComponentModel;
using Prom_Simple;

namespace Krowiorsch
{
    public class AppState
    {
        readonly DateTimeOffset _startTime;

        public AppState(DateTimeOffset startTime)
        {
            _startTime = startTime;
        }

        [PromDescription("gibt an, wie lang die App schon läuft")]
        [PromCounter]
        public long UpTime => Convert.ToInt64(DateTimeOffset.Now.Subtract(_startTime).TotalMilliseconds);

        [PromDescription("gibt eine gesamtzahl an")]
        [PromCounter]
        public int CountTotal { get; set; }
    }
}