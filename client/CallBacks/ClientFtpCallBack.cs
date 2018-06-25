using common;
using Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace client.CallBacks
{
    class ClientFtpCallBack : IClientFtpCallBack
    {
        public event EventHandler<List<string>> finishRequstConnectionProcessEvent;
        private readonly BackgroundWorker saveFiletWorker = new BackgroundWorker();

        #region Singelton
        private static ClientFtpCallBack _instance;

        private ClientFtpCallBack()  {
            saveFiletWorker.DoWork += saveFileWorker_DoWork;
        }

        public static ClientFtpCallBack Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ClientFtpCallBack();

                return _instance;
            }
        }

        public void finishRequstConnectionProcess(List<string> finishStatus)
        {
            finishRequstConnectionProcessEvent?.Invoke(this, finishStatus);
        }

        public void finishRequstForDownloadFile(byte[] fileStream, string fileNameEncrypted)
        {
            string folderPath = "";
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
            }

            byte[] folderPathToByte = Encoding.UTF8.GetBytes(folderPath);
            byte[] fileNameEncryptedToByte = Encoding.UTF8.GetBytes(fileNameEncrypted);
            

            List<byte[]> arguments = new List<byte[]>();
            arguments.Add(fileStream);
            arguments.Add(folderPathToByte);
            arguments.Add(fileNameEncryptedToByte);



            if (!saveFiletWorker.IsBusy)
            {
                saveFiletWorker.RunWorkerAsync(arguments);
            }          
        }

        private void saveFileWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<byte[]> genericlist = e.Argument as List<byte[]>;

            byte[] fileStream = genericlist[0];
            byte[] folderPath = genericlist[1];
            byte[] fileNameEncrypted = genericlist[2];

            string folderPathToString = Encoding.UTF8.GetString(folderPath);
            string fileNameEncryptedToString = Encoding.UTF8.GetString(fileNameEncrypted);

            string decrryptedFileName = CAes.SimpleDecrypt(fileNameEncryptedToString, ClientAllData.Instance.getMyClient().m_ftpSessionKey, ClientAllData.Instance.getMyClient().m_ftpSessionKey);


            byte[] fileStreamDecrypt = CAes.SimpleDecrypt(fileStream, ClientAllData.Instance.getMyClient().m_ftpSessionKey, ClientAllData.Instance.getMyClient().m_ftpSessionKey);


            try
            {
                using (var fs = new FileStream(folderPathToString+"\\"+decrryptedFileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fileStreamDecrypt, 0, fileStreamDecrypt.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
            }

        }

        #endregion


    }
}
