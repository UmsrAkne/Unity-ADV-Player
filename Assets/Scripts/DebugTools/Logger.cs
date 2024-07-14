using System;
using System.Collections.Generic;
using System.IO;

namespace DebugTools
{
    public class Logger
    {
        public static List<string> ErrorMessages { get; private set; } = new ();

        /// <summary>
        /// 入力した文字に、入力時刻を加え、内部で保持しているログの文字列の末尾に加えます。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isError">true に設定すると、ログの出力に加えて、静的プロパティの ErrorMessages にもメッセージを追加します。</param>
        public static void Add(string text, bool isError = false)
        {
            var str = $"{DateTime.Now:u} {text}";
            System.Diagnostics.Debug.WriteLine(str);

            var sw = new StreamWriter("playLog.txt", true);
            sw.WriteLine(str);
            sw.Flush();
            sw.Close();

            if (isError)
            {
                ErrorMessages.Add(text);
            }
        }
    }
}