using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TdLib;
using TelegramCore;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string linkGroup;
        private string groupFile;
        private string invitationLink;

        private List<ChatMember> parsedMembers;

        private DispatcherTimer timer;
        private Queue<BotInfo> botsQueue;
        private Queue<int> usersQueue;

        private int maxUsersToInvite;
        private int usersToInviteInterval;
        private int invitedUsersCount;

        private Random random;

        internal List<BotInfo> BotsList { get; set; }
        public ObservableCollection<LogItem> LogItems { get; set; }

        public MainWindow()
        {
            timer = new DispatcherTimer();
            timer.Tick += Scheduler_Tick;

            random = new Random();

            InitializeComponent();

            LogItems = new ObservableCollection<LogItem>();

            groupsDataGrid.ItemsSource = BotsList;
            LogDataGrid.ItemsSource = LogItems;
        }

        private void Log(string level, string line)
        {
            Dispatcher.Invoke(() =>
            {
                //    LogsTextBlock.Text += string.Format("{0}: [{1}] {2}\n", DateTime.Now.ToString(), level, line);
                //});
                LogItems.Add(new LogItem
                {
                    Level = level,
                    Date = DateTime.Now,
                    Line = line
                });
            });
        }

        private void botsFileBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            botsFileTextBox.Text = dialog.FileName;

            BotsList = new List<BotInfo>();
            using (var botsFile = new StreamReader(botsFileTextBox.Text))
            {
                botsFile.ReadLine();
                for (var b1 = botsFile.ReadLine(); !string.IsNullOrEmpty(b1); b1 = botsFile.ReadLine())
                {
                    if (BotInfo.TryParse(b1, out BotInfo botInfo))
                        BotsList.Add(botInfo);
                }
            }

            var colons = new List<Colons>();
            foreach (var group in BotsList.GroupBy(x => x.GroupName))
            {
                colons.Add(new Colons
                {
                    NameGroup = group.Key,
                    AccInGroup = group.Count().ToString(),
                    CheckBox = false
                });
            }

            groupsDataGrid.ItemsSource = colons;
        }

        private void TelegramSession_CodeRequired(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var session = sender as TelegramSession;
                var codeDialog = new CodeDialog();
                codeDialog.phoneNumber.Content = "PhoneNumber: " + session.PhoneNumber;
                if (codeDialog.ShowDialog() == true)
                {
                    var code = codeDialog.CodeTextBox.Text;
                    session.SetCode(code);
                }
            });
        }

        #region ParseUsersTab

        private void BotsFileAppBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var save = new SaveFileDialog();
            save.Filter = "Text file (*.csv)|*.csv";
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                botsFileAppTextBox.Text = save.FileName;
            }
        }

        internal class ChatMemberComparer : IEqualityComparer<ChatMember>
        {
            public bool Equals(ChatMember x, ChatMember y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(ChatMember obj)
            {
                return obj.Id.GetHashCode();
            }
        }

        private async void startAppButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (BotsList.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Bots file not loaded!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(botsFileAppTextBox.Text))
            {
                System.Windows.Forms.MessageBox.Show("Users file not selected!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var usersFileName = botsFileAppTextBox.Text;

            Log("Info", "Start parsing user groups.");

            botsFileAppTextBox.IsEnabled = false;
            BotsFileAppBrowseButton.IsEnabled = false;
            startAppButton.IsEnabled = false;

            parsedMembers = new List<ChatMember>();
            foreach (var bot in BotsList)
            {
                var session = new TelegramSession(bot.PhoneNumber);
                session.CodeRequired += TelegramSession_CodeRequired;
                session.AuthCompleted += TelegramSession_AuthCompleted3;

                await session.RunAsync().ConfigureAwait(false);
            }

            try
            {
                parsedMembers.Distinct(new ChatMemberComparer());
                using (var userFile = new StreamWriter(usersFileName, false, Encoding.UTF8))
                {
                    userFile.WriteLine(ChatMember.CSVHeder());
                    foreach (var member in parsedMembers)
                    {
                        userFile.WriteLine(member);
                    }
                }

                Log("Info", "Finished parsing user groups.");
            }
            catch (Exception ex)
            {
                Log("Error", "Failed to write user file: " + ex.Message);
            }
            finally
            {
                Dispatcher.Invoke(() =>
                {
                    botsFileAppTextBox.IsEnabled = true;
                    BotsFileAppBrowseButton.IsEnabled = true;
                    startAppButton.IsEnabled = true;
                });
            }
        }

        private async void TelegramSession_AuthCompleted3(object sender, EventArgs e)
        {
            var session = sender as TelegramSession;

            try
            {
                await ParseUsers(session.Client).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log("Error", "Failed parsing users: " + ex.Message);
            }
            finally
            {
                session.Stop();
            }
        }

        public async Task ParseUsers(Dialer dialer)
        {
            Log("Info", "Start parsing users info.");
            var me = await dialer.ExecuteAsync(new TdApi.GetMe { });
            var chats = await dialer.ExecuteAsync(new TdApi.GetChats { OffsetOrder = Int64.MaxValue, Limit = 100 });
            foreach (var chatId in chats.ChatIds)
            {
                try
                {
                    var chatInfo = await dialer.ExecuteAsync(new TdApi.GetChat { ChatId = chatId });
                    var members = await dialer.ExecuteAsync(new TdApi.SearchChatMembers { ChatId = chatId, Query = "", Limit = 100 });
                    foreach (var member in members.Members)
                    {
                        if (member.UserId != me.Id)
                        {
                            var user = await dialer.ExecuteAsync(new TdApi.GetUser { UserId = member.UserId });

                            var messages = await dialer.ExecuteAsync(new TdApi.SearchChatMessages { ChatId = chatId, Query = "", SenderUserId = member.UserId, Limit = 3 });
                            var chatMember = new ChatMember { Id = user.Id, ChatId = chatId, ChatName = chatInfo.Title, UserName = user.Username, LastMessages = messages.Messages_ };
                            if (user.Status is TdApi.UserStatus.UserStatusOffline)
                            {
                                chatMember.LastOnline = (user.Status as TdApi.UserStatus.UserStatusOffline).WasOnline;
                            }
                            else if (user.Status is TdApi.UserStatus.UserStatusOnline)
                            {
                                chatMember.LastOnline = (user.Status as TdApi.UserStatus.UserStatusOnline).Expires;
                            }
                            else if (user.Status is TdApi.UserStatus.UserStatusRecently)
                            {
                                chatMember.LastOnline = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                            }
                            else if (user.Status is TdApi.UserStatus.UserStatusLastWeek)
                            {
                                chatMember.LastOnline = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds - 7 * 24 * 60 * 60;
                            }
                            else if (user.Status is TdApi.UserStatus.UserStatusLastMonth)
                            {
                                chatMember.LastOnline = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds - 30 * 24 * 60 * 60;
                            }

                            lock (parsedMembers)
                            {
                                parsedMembers.Add(chatMember);
                            }
                        }
                    }
                    Log("Info", "Finished parsing user info.");
                }
                catch (Exception ex)
                {
                    Log("Error", "Failed to write user info: " + ex.Message);
                }
            }
        }

        #endregion

        #region InviteBotsTab

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (BotsList.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Bots file not loaded!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(linkTextBox.Text))
            {
                System.Windows.Forms.MessageBox.Show("Invitation link is empty!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            linkGroup = linkTextBox.Text;

            foreach (var bot in BotsList)
            {
                var session = new TelegramSession(bot.PhoneNumber);
                session.CodeRequired += TelegramSession_CodeRequired;
                session.AuthCompleted += TelegramSession_AuthCompleted2;

                await session.RunAsync().ConfigureAwait(false);
            }
        }

        private async void TelegramSession_AuthCompleted2(object sender, EventArgs e)
        {
            var session = sender as TelegramSession;

            Log("Info", "Start invite bots to group.");
            try
            {
                await InviteBotsToGroup(session.Client).ConfigureAwait(false);
                Log("Info", "Finished invite bots to group.");
            }
            catch (Exception ex)
            {
                Log("Error", "Failed invite bots to group: " + ex.Message);
            }
            finally
            {
                session.Stop();
            }
        }

        public async Task InviteBotsToGroup(Dialer dialer)
        {
            await dialer.ExecuteAsync(new TdApi.JoinChatByInviteLink() { InviteLink = linkGroup });
        }

        #endregion

        #region InviteUsersTab

        private void inviteFileBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog4 = new OpenFileDialog();
            if (dialog4.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                inviteFileTextBox.Text = dialog4.FileName;
            }
        }

        private async void inviteButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (BotsList.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Bots file not loaded!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(inviteFileTextBox.Text))
            {
                System.Windows.Forms.MessageBox.Show("Users file not selected!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(invitationLinkTab2TextBox.Text))
            {
                System.Windows.Forms.MessageBox.Show("Invitation link is empty!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(maxUsersToInviteTextBox.Text, out int max))
            {
                System.Windows.Forms.MessageBox.Show("Invalid max users value!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                maxUsersToInvite = max;

            if (!int.TryParse(usersToInviteIntervalTextBox.Text, out int interval))
            {
                System.Windows.Forms.MessageBox.Show("Invalid interval value!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                usersToInviteInterval = interval;

            invitationLink = invitationLinkTab2TextBox.Text;

            var group = (groupsDataGrid.ItemsSource as List<Colons>).Find(g => g.CheckBox);
            if (group == null)
            {
                System.Windows.Forms.MessageBox.Show("Group is not selected!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            botsQueue = new Queue<BotInfo>();
            foreach (var bot in BotsList)
            {
                if (bot.GroupName.Equals(group.NameGroup))
                    botsQueue.Enqueue(bot);
            }

            usersQueue = new Queue<int>();
            using (var usersFile = new StreamReader(inviteFileTextBox.Text))
            {
                await usersFile.ReadLineAsync();
                for (var user = await usersFile.ReadLineAsync(); !string.IsNullOrEmpty(user); user = await usersFile.ReadLineAsync())
                {
                    if (int.TryParse(user.Split(',')[0], out int userId))
                    {
                        usersQueue.Enqueue(userId);
                    }
                }
            }

            invitedUsersCount = 0;
            timer.Interval = TimeSpan.FromMinutes(random.Next(0, 0));
            timer.Start();
        }

        private async void Scheduler_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            var bot = botsQueue.Dequeue();

            try
            {
                var proxy = new ProxySettings
                {
                    Ip = bot.ProxyIp,
                    Port = bot.ProxyPort,
                    Login = bot.ProxyLogin,
                    Password = bot.ProxyPassword
                };

                var session = new TelegramSession(bot.PhoneNumber, proxy);
                session.CodeRequired += TelegramSession_CodeRequired;
                session.AuthCompleted += TelegramSession_AuthCompleted;

                await session.RunAsync().ConfigureAwait(false);
                Log("Info", "Used proxy.");
            }
            finally
            {
                botsQueue.Enqueue(bot);
                timer.Interval = TimeSpan.FromHours(usersToInviteInterval) + TimeSpan.FromMinutes(random.Next(-15, 15));
                timer.Start();
            }
        }

        private async void TelegramSession_AuthCompleted(object sender, EventArgs e)
        {
            var session = sender as TelegramSession;

            Log("Info", "Start invite users to group.");
            try
            {
                await InviteUserToGroup(session.Client).ConfigureAwait(false);
                Log("Info", "Finished invite users to group.");
            }
            catch (Exception ex)
            {
                Log("Error", "Failed invite users to group: " + ex.Message);
            }
            finally
            {
                session.Stop();
                invitedUsersCount += 1;
                if (usersQueue.Count == 0 || invitedUsersCount == maxUsersToInvite)
                    (sender as DispatcherTimer).Stop();
            }
        }

        public async Task InviteUserToGroup(Dialer dialer)
        {
            var userId = usersQueue.Dequeue();
            var chatLinkInfo = await dialer.ExecuteAsync(new TdApi.CheckChatInviteLink { InviteLink = invitationLink });
            await dialer.ExecuteAsync(new TdApi.AddChatMember { ChatId = chatLinkInfo.ChatId, UserId = userId });
        }

        #endregion

        private void maxUsersToInviteTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void Grid_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void infoButton_Click(object sender, RoutedEventArgs e)
        {
            var info = new Window1();
            info.Show();

        }
    }
}
