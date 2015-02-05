using System;

namespace PeerChannelSample
{
    public class StringEventArgs : EventArgs
    {
        public string Content { get; set; }

        public StringEventArgs(string content)
        {
            Content = content;

        }  
    }
}
