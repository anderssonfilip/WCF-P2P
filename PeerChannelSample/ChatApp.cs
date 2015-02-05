using System;

namespace PeerChannelSample
{
    public class ChatApp : IChat
    {
        readonly string _member;

        public ChatApp(string member)
        {
            _member = member;
        }

        public event EventHandler<StringEventArgs> UserAdded;
        public event EventHandler<StringEventArgs> UserRemoved;
        public event EventHandler<StringEventArgs> StateAdded;
        public event EventHandler<StateEventArgs> StateChanged;

        public void Join(string member)
        {
            if (UserAdded != null)
            {
                UserAdded(this, new StringEventArgs(member));
            }
        }

        public void AddCheckBox(string member, string msg)
        {
            if (member != _member)
            {
                if (StateAdded != null)
                {
                    StateAdded(this, new StringEventArgs(msg));
                }
            }
        }

        public void UpdateCheckBox(string member, string msg)
        {
            if (member != _member)
            {
                var tuple = msg.Split(':');

                var state = new State(tuple[0]) { IsCompleted = bool.Parse(tuple[1]) };

                if (StateChanged != null)
                {
                    StateChanged(this, new StateEventArgs(state));
                }
            }
        }

        public void Leave(string member)
        {
            if (UserRemoved != null)
            {
                UserRemoved(this, new StringEventArgs(member));
            }
        }
    }
}
