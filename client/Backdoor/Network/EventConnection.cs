
namespace LegitimeAPP.Network {
    public delegate void EventConnectionHandler() ;

    public class EventConnection : EventArgs {
        private string EventInfo ;

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