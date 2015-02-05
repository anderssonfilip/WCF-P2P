using System.ComponentModel;

namespace PeerChannelSample
{
    public class State : INotifyPropertyChanged
    {
        readonly string _name;
        private bool _isCompleted;

        public State(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                OnPropertyChanged("IsCompleted");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
