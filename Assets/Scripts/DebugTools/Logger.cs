using System;
using System.IO;

namespace DebugTools
{
    public class Logger
    {
        /// <summary>
        /// 入力した文字に、入力時刻を加え、内部で保持しているログの文字列の末尾に加えます。
        /// </summary>
        /// <param name="text"></param>
        public static void Add(string text)
        {
            var str = $"{DateTime.Now:u} {text}";
            System.Diagnostics.Debug.WriteLine(str);

            var sw = new StreamWriter("playLog.txt", true);
            sw.WriteLine(text);
            sw.Flush();
            sw.Close();
        }
    }
}