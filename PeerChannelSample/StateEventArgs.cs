using System;

namespace PeerChannelSample
{
    public class StateEventArgs : EventArgs
    {
        public State State { get; set; }

        public StateEventArgs(State state)
        {
            State = state;
        }
    }
}
