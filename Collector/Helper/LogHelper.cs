using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collector.Helper
{
    public class LogHelper
    {
        public static string LogPath = "";
        public static int TcpPort = 8888;
        public static bool Status = true;
        public enum LogEnum
        {
            错误,
            提示
        }

        public static FileInfo FileLog
        {
            get;
            set;
        }
        public static string FileDirectory
        {
            set;
            get;
        }



        private static void CreateFileLog()
        {

            FileDirectory = Path.Combine(LogPath, "Log");
            if (!Directory.Exists(FileDirectory))
                Directory.CreateDirectory(FileDirectory);
            string _fileLogPath = Path.Combine(FileDirectory, "Log" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            if (!File.Exists(_fileLogPath))
            {
                File.Create(_fileLogPath).Close();
            }
            FileLog = new FileInfo(_fileLogPath);
        }
        public static bool WriteLog(string message, LogEnum logType)
        {
            CreateFileLog();
            using (FileStream fs = new FileStream(FileLog.FullName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                StreamWriter sw = new StreamWriter(fs);
                string pLog = string.Format("时间: {0}  {2}: {1}", DateTime.Now.ToString(), message, logType.ToString());
                sw.WriteLine(pLog);

                sw.Close();
            }
            return true;
        }
    }
}
