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

        public static string GetText(string str)//считывание информации из файла
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
        public static void WriteResult(Dictionary<string, int> sortedDict)//вывод результата в консоль
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
            var lineText = text.Split('\n');//построчное разделение
            foreach (var line in lineText)
            {
                var wordList = line.Split(' ');//разделение по пробелам
                foreach (var word in wordList)
                {
                    if (word.Length > 2)
                        for (int i = -1; i < word.Length - 3; i++)
                        {
                            string wordBorder = word.Substring(i + 1, 3);//вырезание триплетов
                            if (!Regex.IsMatch(wordBorder, @"[\d\s\W\(!@\#\$%\^&\*\(\)_\+=\-'\\:\|/`~\.,\{}\)]"))//проверка, что триплеты состоят только из букв
                            {
                                if (!dictResult.ContainsKey(wordBorder))//заполнение словаря с частотой триплетов
                                    dictResult[wordBorder] = 1;
                                else
                                    dictResult[wordBorder]++;
                            }
                        }
                }
            }
            var sortedDict = from entry in dictResult orderby entry.Value descending select entry;//сортировка словаря
            var r = sortedDict.ToDictionary<KeyValuePair<string, int>, string, int>(p=>p.Key, p=>p.Value);
            WriteResult(r);//вывод результата
            sw.Stop();
            Console.WriteLine("Spend of time {0}:", sw.ElapsedMilliseconds);//вывод результата по времени
        }
        
        public static void Main()
        {
            Thread thread1 = new Thread(new ParameterizedThreadStart(Make));//инициализация потока
            var link = GetLink();
            thread1.Start(link);//запуск потока по обработки текста
            Console.ReadLine();
        }
    }
}
