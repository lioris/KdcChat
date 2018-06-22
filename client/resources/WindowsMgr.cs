using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace client.resources
{
    class WindowsMgr
    {
        Dictionary<string, Window> list = new Dictionary<string, Window>();

        #region Singelton
        private static WindowsMgr _instance;

        private WindowsMgr()
        {

        }

        public static WindowsMgr Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new WindowsMgr();

                return _instance;
            }
        }
        #endregion

        public Window addWindow(string name, Window w)
        {
            if (list.ContainsKey(name))
                list[name] = w;
            else
                list.Add(name, w);

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
    }
}
