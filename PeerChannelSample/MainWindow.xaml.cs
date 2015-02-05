using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using PeerChannelSample;

namespace WcfPeerChannelSample
{
    public partial class MainWindow
    {
        readonly IChatChannel _participant;
        readonly DuplexChannelFactory<IChatChannel> _factory;
        readonly string _member;

        public ObservableCollection<State> States { get; set; }
        public ObservableCollection<string> Users { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            States = new ObservableCollection<State>();
            Users = new ObservableCollection<string>();

            States.Add(new State("Type above and click Add to add more Checkboxes"));

            _member = Guid.NewGuid().ToString();

            var chat = new ChatApp(_member);

            chat.UserAdded += (sender, args) => Users.Add(args.Content);

            chat.UserRemoved += (sender, args) => Users.Remove(args.Content);

            chat.StateAdded += (sender, args) => States.Add(new State(args.Content));

            chat.StateChanged += delegate(object sender, StateEventArgs args)
                                     {
                                         foreach (var state in States.Where(state => state.Name.Equals(args.State.Name)))
                                         {
                                             state.IsCompleted = args.State.IsCompleted;
                                         }
                                     };
           
            var instanceContext = new InstanceContext(chat);

            _factory = new DuplexChannelFactory<IChatChannel>(instanceContext, "ChatEndpoint");

            _participant = _factory.CreateChannel();

            var ostat = _participant.GetProperty<IOnlineStatus>();
            ostat.Online += (sender, args) => Console.WriteLine("**  Online");
            ostat.Offline += (sender, args) => Console.WriteLine("**  Offline");

            try
            {
                _participant.Open();
            }
            catch (CommunicationException)
            {
                return;
            }

            Application.Current.Exit += CurrentOnExit;

            _participant.Join(_member);
        }

        private void CurrentOnExit(object sender, ExitEventArgs exitEventArgs)
        {
            _participant.Leave(_member);
            _participant.Close();
            _factory.Close();
        }


        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Content.Text))
            {
                States.Add(new State(Content.Text));
                _participant.AddCheckBox(_member, Content.Text);
                Content.Text = null;
            }
        }

        private void CheckBoxChecked(object sender, RoutedEventArgs e)
        {
            _participant.UpdateCheckBox(_member, (sender as CheckBox).Content + ":" + true);
        }

        private void CheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            _participant.UpdateCheckBox(_member, (sender as CheckBox).Content + ":" + false);
        }
    }
}
