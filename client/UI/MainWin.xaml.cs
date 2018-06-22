using common;
using Contracts;
using LinqToSql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace client
{
    /// <summary>
    /// Interaction logic for MainWin.xaml
    /// </summary>
    public partial class MainWin : Window
    {
        private readonly BackgroundWorker getSessionKeyWorker = new BackgroundWorker();

        public MainWin()
        {
            InitializeComponent();

            getSessionKeyWorker.DoWork += getSessionKeyWorker_DoWork;
            getSessionKeyWorker.RunWorkerCompleted += getSessionKeyWorker_RunWorkerCompleted;
        }

        private void getSessionKeyWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //User user = (User)Application.Current.Resources[Constants.CURRENT_USER];

            //if (user != null)
            //{
            //    MessageBox.Show("conected with user " + user.Name + "and your id is" + user.ID);
            //    ClientKdcCallBack.Instance.addWindow(Constants.MAIN_WINDOW, new MainWin()).Show();
            //    ClientKdcCallBack.Instance.CloseWindow(Constants.LOGIN_WINDOW);
            //}
            //else
            //{
            //    MessageBox.Show("Wrong user name = " + txtUserName.Text + " or password = " + txtPassWord.Text + " , try again!");
            //    this.setEnable();
            //}
        }

        private void getSessionKeyWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            IKdcService proxy = Proxy.Instance.GetProxy();
            string partnerUsernameInvoked = string.Empty;
            

            Dispatcher.BeginInvoke(new Action(delegate
            {
                partnerUsernameInvoked = partnerUsernameText.Text;
                
            }));


            Thread.Sleep(100);

            User user = (User)Application.Current.Resources[Constants.CURRENT_USER];
            CSessionParams sessionPrm = new CSessionParams();
            sessionPrm.client1UserName = user.Name;
            sessionPrm.client2UserName = partnerUsernameInvoked;
            CSessionKeyResponse sessionRespons = proxy.GetSessionKey(sessionPrm);
            if(sessionRespons == null)
            {
                //report error
                return;
            }
            byte[] sessionKey = CAes.SimpleDecryptWithPassword(sessionRespons.m_sessionKeyA, user.PassWord);
            byte[] sessionKeyPartner = CAes.SimpleDecryptWithPassword(sessionRespons.m_sessionKeyB, user.PassWord);
        }

        private void start_chat_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!getSessionKeyWorker.IsBusy)
            {
                getSessionKeyWorker.RunWorkerAsync();
            }
        }
    }
}
