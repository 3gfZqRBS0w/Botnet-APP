using System ;
using BotnetAPP.Shared ;

namespace BotnetAPP.Network {

    public delegate void EventConnectionHandler(object source);

    public class EventConnection : EventArgs {
        private string EventInfo;
        public EventConnection(string Text)
        {
            EventInfo = Text;
        }
        public string GetInfo()
        {
            return EventInfo;
        }

    }
}