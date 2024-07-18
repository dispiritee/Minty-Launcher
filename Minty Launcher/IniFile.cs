using System;
using System.Collections.Generic;
using System.IO;

public class IniFile
{
    private string filePath;
    private Dictionary<string, Dictionary<string, string>> data;

    // Конструктор класса IniFile
    public IniFile(string fileName)
    {
        filePath = fileName;
        data = new Dictionary<string, Dictionary<string, string>>();

        // Если файл существует, читаем его
        if (File.Exists(filePath))
        {
            ReadData();
        }
    }

    // Метод для чтения данных из файла
    private void ReadData()
    {
        string currentSection = null;

        foreach (string line in File.ReadLines(filePath))
        {
            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                currentSection = line.Trim('[', ']');
                data[currentSection] = new Dictionary<string, string>();
            }
            else if (!string.IsNullOrWhiteSpace(line) && line.Contains("=") && currentSection != null)
            {
                string[] parts = line.Split(new char[] { '=' }, 2);
                data[currentSection][parts[0].Trim()] = parts[1].Trim();
            }
        }
    }

    // Метод для записи данных в INI файл
    public void Write(string section, string key, string value)
    {
        // Если секция не существует, добавляем ее
        if (!data.ContainsKey(section))
        {
            data[section] = new Dictionary<string, string>();
        }

        // Записываем или обновляем значение ключа в секции
        data[section][key] = value;

        // Сохраняем изменения в файле
        SaveData();
    }

    // Метод для сохранения данных в файле
    private void SaveData()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var section in data)
            {
                writer.WriteLine($"[{section.Key}]");

                foreach (var kvp in section.Value)
                {
                    writer.WriteLine($"{kvp.Key}={kvp.Value}");
                }

                // Пустая строка между секциями
                writer.WriteLine();
            }
        }
    }

    // Метод для получения значения из INI файла по указанному ключу и секции
    public string GetValue(string section, string key)
    {
        if (data.ContainsKey(section) && data[section].ContainsKey(key))
        {
            return data[section][key];
        }
        return null; // Вернуть null если значение не найдено
    }
}
