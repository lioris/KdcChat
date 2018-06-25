using client.resources;
using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        userPortData m_userPort;
        public ChatWindow(userPortData userPort)
        {
            InitializeComponent();
            m_userPort = userPort;
            MainWin mainWindow = (MainWin)WindowsMgr.Instance.GetWindow(Constants.MAIN_WINDOW);
            mainWindow.startChatHandshakeEvnt += (sender, sessionRespons) =>
            {
                Dispatcher.Invoke(() => startChatHandshake(sessionRespons));
            };

        }

        public void startChatHandshake(CSessionKeyResponse sessionRespons)
        {
            Console.WriteLine("chatwindow satrt");
        }
    }
}
