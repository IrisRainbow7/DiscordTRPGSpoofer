using DiscordRPC;
using DiscordRPC.Logging;
using System.Diagnostics;
using System.Windows;

namespace DiscordTRPGSpoofer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string[] clientIDs = new string[] { "862331543418699827", "865166411110154301", "878155965780869131", "862386382558986260", "874389940249714761" };
        private readonly string[] ImageTexts = new string[] { "いあ！いあ！", "", "", "エリンディル", "" };
        private string currentClientID = "";
        private DiscordRpcClient client;
        private bool first = true;
        private Timestamps ts;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void InitializeDiscord(string clientID)
        {
            currentClientID = clientID;
            client = new DiscordRpcClient(clientID);
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            //Subscribe to events
            client.OnReady += (sender, e) =>
            {
                Debug.Print("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Debug.Print("Received Update! {0}", e.Presence);
            };
            client.Initialize();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInput() == false)
            {
                return;
            }

            if (string.IsNullOrEmpty(currentClientID))
            {
                InitializeDiscord(clientIDs[TRPGSystemName.SelectedIndex]);
            } else if (currentClientID != clientIDs[TRPGSystemName.SelectedIndex])
            {
                client.Dispose();
                first = true;
                InitializeDiscord(clientIDs[TRPGSystemName.SelectedIndex]);
            }

            if (first)
            {
                first = false;
                ts = Timestamps.Now;
            }
            var rp = new RichPresence()
            {
                State = State.Text,
                Details = Detail.Text,
                Assets = new Assets()
                {
                    LargeImageKey = "icon",
                    LargeImageText = ImageTexts[TRPGSystemName.SelectedIndex]

                },
            };
            if ((bool)isEnableTime.IsChecked)
            {
                rp.Timestamps = ts;
            }

            client.SetPresence(rp);
        }


        private bool CheckInput()
        {
            if (string.IsNullOrEmpty(TRPGSystemName.Text))
            {
                MessageBox.Show("TRPGシステムを選択してください。");
                return false;
            }
            else
            {
                // チェックOK
                return true;
            }
        }
    }
}
