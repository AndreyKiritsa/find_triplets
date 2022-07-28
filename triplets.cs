using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;

namespace ConsoleApp1
{
    public class Programm
    {
        public static string GetLink()//считывание пути до текстового файла
        {
            string reference = Console.ReadLine();
            return reference;
        }

        public static string GetText(string str)
        {
            var str1 = "";
            try
            {
                str1 = File.ReadAllText(str);
                return str1;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File is not found {0}.", e.Source);
            }
            return str1;
        }
        public static void WriteResult(Dictionary<string, int> sortedDict)
        {
            int count = 0;
            foreach (var i in sortedDict)
            {
                if(count < 9)
                    Console.Write("{0}, ", i.Key);
                else
                {
                    Console.Write("{0}", i.Key);
                    break;
                }
                count++;
            }
            Console.WriteLine();
        }
        public static void Make(object link)
        {
            var sw = new Stopwatch();
            sw.Start();
            var text = GetText((string)link);
            var dictResult = new Dictionary<string, int>();
            var lineText = text.Split('\n');
            foreach (var line in lineText)
            {
                var wordList = line.Split(' ');
                foreach (var word in wordList)
                {
                    if (word.Length > 2)
                        for (int i = -1; i < word.Length - 3; i++)
                        {
                            string wordBorder = word.Substring(i + 1, 3);
                            if (!Regex.IsMatch(wordBorder, @"[\d\s\W\(!@\#\$%\^&\*\(\)_\+=\-'\\:\|/`~\.,\{}\)]"))
                            {
                                if (!dictResult.ContainsKey(wordBorder))
                                    dictResult[wordBorder] = 1;
                                else
                                    dictResult[wordBorder]++;
                            }
                        }
                }
            }
            var sortedDict = from entry in dictResult orderby entry.Value descending select entry;
            var r = sortedDict.ToDictionary<KeyValuePair<string, int>, string, int>(p=>p.Key, p=>p.Value);
            WriteResult(r);
            sw.Stop();
            Console.WriteLine("Spend of time {0}:", sw.ElapsedMilliseconds);
        }
        public static void Main()
        {
            Thread thread1 = new Thread(new ParameterizedThreadStart(Make));
            var link = GetLink();
            thread1.Start(link);//запуск потока по обработки текста
            Console.ReadLine();
        }
    }
}
