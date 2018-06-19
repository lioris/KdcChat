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
            ClientCallBack.Instance.addWindow(Constants.LOGIN_WINDOW, this);

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
            txtPassWord.IsReadOnly = true;
            txtUserName.IsReadOnly = true; 

            btnLogIn.IsEnabled = false;
            btnRegister.IsEnabled = false;
        }


        private void setEnable()
        {
            txtPassWord.IsReadOnly = false;
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
            IKdcService proxy = Proxy.Instance.GetProxy();
            string usernameInvoked = string.Empty;
            string passwordInvoked = string.Empty;

            Dispatcher.BeginInvoke(new Action(delegate
            {
                usernameInvoked = txtUserName.Text;
                passwordInvoked = txtPassWord.Text;
            }));


            Thread.Sleep(100);
            // try to log in to the server
            User userData = proxy.LogInApp(usernameInvoked);

            // parse the result
            if (userData == null)
            {
                Application.Current.Resources[Constants.CURRENT_USER] = null;
            }
            else
            {
                string retDecUserName = CAes.SimpleDecryptWithPassword(userData.Name, passwordInvoked);
                string retDecPassword = CAes.SimpleDecryptWithPassword(userData.PassWord, passwordInvoked);
                if (retDecUserName == usernameInvoked && retDecPassword == passwordInvoked)
                {
                    userData.Name = retDecUserName;
                    userData.PassWord = retDecPassword;
                    Application.Current.Resources[Constants.CURRENT_USER] = userData;
                }
            }
        }

        private void login_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            User user = (User)Application.Current.Resources[Constants.CURRENT_USER];

            if (user != null)
            {
                MessageBox.Show("conected with user " + user.Name + "and your id is" + user.ID);
                ClientCallBack.Instance.addWindow(Constants.MAIN_WINDOW, new MainWin()).Show();
                ClientCallBack.Instance.CloseWindow(Constants.LOGIN_WINDOW);
            }
            else
            {
                MessageBox.Show("Wrong user name = "+ txtUserName.Text + " or password = " + txtPassWord.Text +  " , try again!");
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
            IKdcService proxy = Proxy.Instance.GetProxy();
            string usernameInvoked = string.Empty;
            string passwordInvoked = string.Empty;

            Dispatcher.BeginInvoke(new Action(delegate
            {
                usernameInvoked = txtUserName.Text;
                passwordInvoked = txtPassWord.Text;
            }));


            Thread.Sleep(100);
            User user = proxy.RegisterApp(usernameInvoked, passwordInvoked);

            if (user == null)
                Application.Current.Resources[Constants.CURRENT_USER] = null;
            else
                Application.Current.Resources[Constants.CURRENT_USER] = user;
        }

        private void register_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            User user = (User)Application.Current.Resources[Constants.CURRENT_USER];

            if (user != null)
            {
                MessageBox.Show("Registerd with user " + user.Name + "and your id is" + user.ID);
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
