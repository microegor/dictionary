﻿using System;
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
        Console.InputEncoding = Encoding.UTF8;

        DictionaryReader reader = new DictionaryReader("dictionary.xml");

        Console.WriteLine("Выберите язык (1 - русский, 2 - английский):");
        int fromLanguage = int.Parse(Console.ReadLine());

        foreach (var wordPair in reader.Words)
        {
            string wordToTranslate = fromLanguage == 1 ? wordPair.Value : wordPair.Key;
            Console.WriteLine("Слово: " + wordToTranslate);

            // Запросить у пользователя перевод слова
            Console.Write("Введите перевод слова " + wordToTranslate + ": ");
            string translation = Console.ReadLine();
            // Отладочная информация
            Console.WriteLine("Translation entered: " + translation);
            Console.WriteLine("Translation length: " + translation.Length);

            // write translation to console in hex
            byte[] translationBytes = Encoding.UTF8.GetBytes(translation);
            Console.WriteLine("Translation in hex: " + BitConverter.ToString(translationBytes).Replace("-", " "));

            // Проверить правильность перевода
            string correctTranslation = fromLanguage == 1 ? wordPair.Key : wordPair.Value;

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