using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace client
{
    class ClientCallBack : IClientCallBack
    {
        public event EventHandler<string> chat_massage;

        Dictionary<string, Window> list = new Dictionary<string, Window>();
 
        #region Singelton
        private static ClientCallBack _instance;
        
        private ClientCallBack(){

        }

        public static ClientCallBack Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ClientCallBack();

                return _instance;
            }
        }
        #endregion

        // mange windows

        public Window addWindow(string name, Window w)
        {
            if (list.ContainsKey(name) )
                list[name] = w;
            else
                list.Add(name,w);

            return w;
        }

        public void CloseWindow(string name)
        {
            if (list.ContainsKey(name))
            {
                if (list[name] != null)
                {
                    list[name].Close();
                    list[name] = null;
                }
            }
        }

        internal Window GetWindow(string p)
        {
            return list[p];
        }

        public void printHello(string massage)
        {
            Console.WriteLine("lior");//throw new NotImplementedException();
        }
    }
}



