using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

class Program
{
    static void Main(string[] args)
    {
        // Установить кодировку консоли на UTF-8
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.GetEncoding("utf-16");


        // Проверка наличия пути
        var path = args.FirstOrDefault();
        if (string.IsNullOrEmpty(path))
        {
            Console.WriteLine("Пожалуйста, укажите путь к файлу.");
            return;
        }

        // Проверка наличия файла
        if (!File.Exists(path))
        {
            Console.WriteLine("Файл не найден.");
            return;
        }

        DictionaryReader reader = new DictionaryReader(path);

        // Выбор режима перевода
        Console.WriteLine("Выберите режим перевода:");
        Console.WriteLine("1 - с русского на английский");
        Console.WriteLine("2 - с английского на русский");
        Console.WriteLine("3 - случайный");
        int mode = int.Parse(Console.ReadLine());

        // Перемешать список слов
        Random random = new Random();
        var shuffledWords = reader.Words.OrderBy(x => random.Next()).ToList();

        foreach (var wordPair in shuffledWords)
        {
            string wordToTranslate;
            string correctTranslation;

            switch (mode)
            {
                case 1:
                    wordToTranslate = wordPair.Value;
                    correctTranslation = wordPair.Key;
                    break;
                case 2:
                    wordToTranslate = wordPair.Key;
                    correctTranslation = wordPair.Value;
                    break;
                case 3:
                    bool translateToRussian = random.Next(2) == 0;
                    wordToTranslate = translateToRussian ? wordPair.Key : wordPair.Value;
                    correctTranslation = translateToRussian ? wordPair.Value : wordPair.Key;
                    break;
                default:
                    Console.WriteLine("Неверный режим.");
                    return;
            }

            // Запросить у пользователя перевод слова
            Console.Write("Введите перевод слова " + wordToTranslate + ": ");
            string translation = Console.ReadLine();

            // Проверить правильность перевода
            if (translation.Equals(correctTranslation, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Верно!");
            }
            else
            {
                Console.WriteLine("Неверно! Правильный перевод: " + correctTranslation);
            }
        }

        Console.WriteLine("Все слова пройдены.");
    }
}

class DictionaryReader
{
    public Dictionary<string, string> Words { get; private set; }

    public DictionaryReader(string filePath)
    {
        Words = new Dictionary<string, string>();
        LoadDictionary(filePath);
    }

    private void LoadDictionary(string filePath)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(filePath);

        XmlNodeList wordNodes = doc.SelectNodes("/dictionary/words/word");
        foreach (XmlNode wordNode in wordNodes)
        {
            string enWord = wordNode.SelectSingleNode("en").InnerText;
            string ruWord = wordNode.SelectSingleNode("ru").InnerText;
            Words.Add(enWord, ruWord);
        }
    }
}