using System.ComponentModel;
using System.Runtime.Remoting.Messaging;

namespace Prom_Simple.StandardWeb.Models
{
    public class RuntimeState
    {
        static State _state;

        public static State Current => _state ?? (_state = new State());


        public class State
        {
            [Description("Anzahl der Requests"), PromCounter]
            [PromLabel("lm_system", "test")]
            public int RequestCounterTotal { get; set; } = 0;

            [Description("Anzahl der laufenden Requests"), PromGauge]
            [PromLabel("lm_system", "test")]
            public int RunningRequests { get; set; } = 0;
        }
    }
}