using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Contracts;
using LinqToSql;
using System.Threading;
using common;
using System.Security.Cryptography;
using client.resources;
using Contracts.logicClasses;

namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker logInWorker = new BackgroundWorker();
        private readonly BackgroundWorker registerWorker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();

            //register to callback singeltone
            WindowsMgr.Instance.addWindow(Constants.LOGIN_WINDOW, this);

            //if ((Process.GetProcessesByName("GameHost")).Length == 0)
            //    Process.Start("D:\\pokerproj\\MyGame\\GameHost\\bin\\Debug\\GameHost.exe");

            // login methoed 
            logInWorker.DoWork += login_worker_DoWork;
            logInWorker.RunWorkerCompleted += login_worker_RunWorkerCompleted;

            //register methoed
            registerWorker.DoWork += register_worker_DoWork;
            registerWorker.RunWorkerCompleted += register_worker_RunWorkerCompleted;

        }

        private void setDisable()
        {
            //txtPassWord.IsReadOnly = true;
            txtUserName.IsReadOnly = true; 

            btnLogIn.IsEnabled = false;
            btnRegister.IsEnabled = false;
        }


        private void setEnable()
        {
            //txtPassWord.IsReadOnly = false;
            txtUserName.IsReadOnly = false; 

            btnLogIn.IsEnabled = true;
            btnRegister.IsEnabled = true;
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            setDisable();
            
            if (!logInWorker.IsBusy)
            {
                logInWorker.RunWorkerAsync();
            }
        }

        private void login_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool looginSuccess = false;
            IKdcService proxy = KdcProxy.Instance.GetProxy();
            string usernameInvoked = string.Empty;
            string passwordInvoked = string.Empty;

            Dispatcher.BeginInvoke(new Action(delegate
            {
                usernameInvoked = txtUserName.Text;
                passwordInvoked = txtPassWord.Password.ToString();
            }));


            Thread.Sleep(100);
            // try to log in to the server
            ClientAllData.Instance.setMyUsername(usernameInvoked);
            CKdcToClientLogInData logginData = proxy.LogInApp(usernameInvoked);

            // parse the result
            if (logginData != null)
            {
                string passWordHash = string.Empty;
                using (MD5 md5Hash = MD5.Create())
                {
                    passWordHash = CMd5hash.GetMd5Hash(md5Hash, passwordInvoked);
                }

                string retDecUserName = CAes.SimpleDecryptWithPassword(logginData.m_username, passWordHash);
                byte[] retDecPassword = new byte[32];
                retDecPassword = CAes.SimpleDecryptWithPassword(logginData.m_kdcAsSessionKey, passWordHash);

                if (retDecUserName == usernameInvoked )
                {
                    string retChallenge = CAes.SimpleDecryptWithPassword(logginData.m_challenge, passWordHash);

                    CLogInStatus logginStatus = new CLogInStatus();
                    logginStatus.m_username = retDecUserName;
                    logginStatus.m_logInFail = false;
                    logginStatus.m_challenge = CAes.SimpleEncrypt(retChallenge, retDecPassword, retDecPassword);
                    if(proxy.SetLoginStatus(logginStatus))
                    {
                        clientPrivateData myClient =  ClientAllData.Instance.getMyClient();
                        myClient.username = retDecUserName;
                        myClient.m_kdcAsSessionKey = retDecPassword;
                        myClient.m_loginSucccess = true;
                        looginSuccess = true;
                    }
                }
            }

            if(!looginSuccess)
            {
                CLogInStatus logginStatus = new CLogInStatus();
                logginStatus.m_username = usernameInvoked;
                logginStatus.m_logInFail = true;
                proxy.SetLoginStatus(logginStatus);
            }
        }

        private void login_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            clientPrivateData user = ClientAllData.Instance.getMyClient();

            if (user != null && user.m_loginSucccess)
            {
                MessageBox.Show("conected succesfully");
                WindowsMgr.Instance.addWindow(Constants.MAIN_WINDOW, new MainWin()).Show();
                WindowsMgr.Instance.CloseWindow(Constants.LOGIN_WINDOW);
            }
            else
            {
                MessageBox.Show("Wrong user name or password try again!");
                this.setEnable();
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            this.setDisable();

            if (!registerWorker.IsBusy)
            {
                registerWorker.RunWorkerAsync();
            }
        }


        private void register_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            IKdcService proxy = KdcProxy.Instance.GetProxy();
            string usernameInvoked = string.Empty;
            string passwordInvoked = string.Empty;

            Dispatcher.BeginInvoke(new Action(delegate
            {
                usernameInvoked = txtUserName.Text;
                passwordInvoked = txtPassWord.Password.ToString();
            }));


            Thread.Sleep(100);
            /*User user = proxy.RegisterApp(usernameInvoked, passwordInvoked);

            if (user == null)
            {
                ClientAllData.Instance.setMyClient(null);
            }

            else
            {
                ClientAllData.Instance.setMyClient(user);
            }*/
        }

        private void register_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            clientPrivateData user = ClientAllData.Instance.getMyClient();

            if (user != null)
            {
                MessageBox.Show("Registerd with user " + user.username);
                if (!logInWorker.IsBusy)
                    logInWorker.RunWorkerAsync();
            }

            else
            {
                MessageBox.Show("somthing went wrong, try Agian!");
                this.setEnable();
            }
        }


    }
}
