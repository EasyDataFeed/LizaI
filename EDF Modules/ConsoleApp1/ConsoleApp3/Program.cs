using System;
using System.IO;
using System.Threading.Tasks;
using TdLib;
using TelegramCore;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            TelegramSession.SetLogLevel(0);

            Console.WriteLine("Write phone number");
            var session1 = new TelegramSession(Console.ReadLine());
            session1.CodeRequired += TelegramSession_CodeRequired;
            session1.AuthCompleted += TelegramSession_AuthCompleted;

            Task.WaitAll(session1.RunAsync());
        }

        public static async Task DoTdApi(Dialer dialer)
        {
            try
            {
                var sw = new StreamWriter("D:/Leprizoriy/test.csv", false, System.Text.Encoding.UTF8);
                sw.WriteLine(ChatMember.CSVHeder());

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

                                sw.WriteLine(chatMember);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                } 
                sw.Close();
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                Environment.Exit(1);
            }
        }

        private static void TelegramSession_AuthCompleted(object sender, EventArgs e)
        {
            Console.WriteLine("Ready");
            Task.Run(async ()=> {
                await DoTdApi((sender as TelegramSession).Client);
            });
        }

        private static void TelegramSession_CodeRequired(object sender, EventArgs e)
        {
            var session = sender as TelegramSession;
            Console.WriteLine("Enter code");
            var code = Console.ReadLine();
            session.SetCode(code);
        }
    }
}
