using System;
using System.Threading;
using System.Threading.Tasks;
using TdLib;

namespace TelegramCore
{
    public class TelegramSession
    {
        private const int APP_API_ID = 369529;
        private const string APP_API_HASH = "ea516bf498500507283348075118e7f4";
        private const string APP_VERSION = "1.0";

        public TelegramSession(string phoneNumber, ProxySettings proxySettings = null)
        {
            PhoneNumber = phoneNumber;
            mProxySettings = proxySettings;

            var client = new Client();
            mHub = new Hub(client);
            Client = new Dialer(client, mHub);

            mHub.Received += Hub_Received;
        }

        public async Task RunAsync()
        {
            var task = new Task(() => { mHub.Start(); });
            task.Start();

            await task;
        }

        public void Stop()
        {
            mHub.Stop();
        }

        public void SetCode(string code)
        {
            Client.Send(new TdApi.CheckAuthenticationCode { Code = code });
        }

        public static void SetLogLevel(int level)
        {
            TdLib.Client.Log.SetVerbosityLevel(level);
        }

        #region Event Handlers

        private void Hub_Received(object sender, TdApi.Object data)
        {
            if (data is TdApi.Update.UpdateAuthorizationState)
            {
                var state = (data as TdApi.Update.UpdateAuthorizationState).AuthorizationState;

                if (state is TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters)
                {
                    Client.Send(new TdApi.SetTdlibParameters
                    {
                        Parameters = new TdApi.TdlibParameters
                        {
                            DatabaseDirectory = "users\\" + PhoneNumber,
                            UseMessageDatabase = true,
                            UseSecretChats = true,
                            ApiId = APP_API_ID,
                            ApiHash = APP_API_HASH,
                            SystemLanguageCode = "en",
                            DeviceModel = "Desktop",
                            SystemVersion = "Unknown",
                            ApplicationVersion = APP_VERSION,
                            EnableStorageOptimizer = true,
                            UseChatInfoDatabase = true
                        }
                    });

                    if (mProxySettings != null)
                    {
                        Client.Send(new TdApi.AddProxy
                        {
                            Type = new TdApi.ProxyType.ProxyTypeHttp
                            {
                                Username = mProxySettings.Login,
                                Password = mProxySettings.Password
                            },
                            Server = mProxySettings.Ip,
                            Port = mProxySettings.Port,
                            Enable = true
                        });
                    }
                }
                else if (state is TdApi.AuthorizationState.AuthorizationStateWaitEncryptionKey)
                {
                    Client.Send(new TdApi.CheckDatabaseEncryptionKey { });
                }
                else if (state is TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber)
                {
                    Client.Send(new TdApi.SetAuthenticationPhoneNumber { PhoneNumber = PhoneNumber });
                }
                else if (state is TdApi.AuthorizationState.AuthorizationStateWaitCode)
                {
                    OnCodeRequired(EventArgs.Empty);
                }
                else if (state is TdApi.AuthorizationState.AuthorizationStateWaitPassword)
                {
                    string pass = Console.ReadLine();
                    Client.Send(new TdApi.CheckAuthenticationPassword { Password = pass });
                }
                else if (state is TdApi.AuthorizationState.AuthorizationStateReady)
                {
                    OnAuthCompleted(EventArgs.Empty);
                }
                else if (state is TdApi.AuthorizationState.AuthorizationStateLoggingOut)
                {
                    Console.WriteLine("Logging out");
                }
                else if (state is TdApi.AuthorizationState.AuthorizationStateClosing)
                {
                    Console.WriteLine("Closing");
                }
                else if (state is TdApi.AuthorizationState.AuthorizationStateClosed)
                {
                    Console.WriteLine("Closed");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Unsupported authorization state: " + state);
                }
            }
            if (data is TdApi.Proxy)
            {

            }
            if (data is TdApi.Ok)
            {
                // do something
            }
            else if (data is TdApi.Error)
            {
                // handle error
            }
        }

        #endregion

        #region Public Events

        public event EventHandler CodeRequired;
        public event EventHandler AuthCompleted;

        private void OnCodeRequired(EventArgs e)
        {
            CodeRequired?.Invoke(this, e);
        }

        private void OnAuthCompleted(EventArgs e)
        {
            AuthCompleted?.Invoke(this, e);
        }

        #endregion

        public Dialer Client { get; set; }
        public string PhoneNumber { get; set; }

        private ProxySettings mProxySettings;
        private Hub mHub;
    }
}
