﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Http;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.IO.Compression;
using Path = System.IO.Path;
using Ionic.Zip;
using System.Security.Policy;
using System.Windows.Media.Animation;

namespace Minty_Launcher
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool isDragging = false;
        private Point dragStartPoint;

        public MainWindow()
        {
            InitializeComponent();
            this.Opacity = 0; // Устанавливаем изначальную непрозрачность 0
        }

        static string tempPath = Environment.GetEnvironmentVariable("TEMP");

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Создаем анимацию для прозрачности
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0, // Начальное значение
                To = 1,   // Конечное значение
                Duration = new Duration(TimeSpan.FromSeconds(1)) // Длительность анимации
            };

            // Применяем анимацию к свойству Opacity
            this.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        static void DownloadPack()

            {
            string url = "https://github.com/kindawindytoday/Minty-Releases/releases/latest/download/minty.zip";
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"MintyLauncher", "minty.zip");
            string extractPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "MintyLauncher");
            string launcherPath = Path.Combine(extractPath, "launcher.exe"); // Путь к launcher.exe

            Console.WriteLine("Путь для сохранения файла: " + downloadPath);

            using (WebClient client = new WebClient())
            {
                try
                {
                    if (File.Exists(launcherPath))
                    {
                        Process.Start(launcherPath);
                        Console.WriteLine("Файл launcher.exe запущен.");
                    }
                    else
                    {
                     // Создаем директорию, если она не существует
                     if (!Directory.Exists(extractPath))
                     {
                        Directory.CreateDirectory(extractPath);
                        Console.WriteLine("Создана директория: " + extractPath);
                     }

                     // Скачиваем файл
                     client.DownloadFile(url, downloadPath);
                     Console.WriteLine("Файл успешно скачан: " + downloadPath);

                     // Распаковка ZIP-архива с использованием DotNetZip
                     using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(downloadPath))
                     {
                        zip.ExtractAll(extractPath, ExtractExistingFileAction.OverwriteSilently);
                     }
                     Console.WriteLine("Архив успешно распакован в: " + extractPath);

                     // Удаляем ZIP-файл после распаковки
                     File.Delete(downloadPath);
                     Console.WriteLine("ZIP файл успешно удалён: " + downloadPath);

                     // Запуск launcher.exe
                     if (File.Exists(launcherPath))
                     {
                        Process.Start(launcherPath);
                        Console.WriteLine("Файл launcher.exe запущен.");
                     }
                     else
                     {
                        Console.WriteLine("Файл launcher.exe не найден по пути: " + launcherPath);
                     }
                    }
                    
                }
                catch (UnauthorizedAccessException uaEx)
                {
                    Console.WriteLine("Ошибка доступа: " + uaEx.Message);
                }
                catch (DirectoryNotFoundException dnEx)
                {
                    Console.WriteLine("Директорий не найден: " + dnEx.Message);
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine("Ошибка ввода-вывода: " + ioEx.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }

        private void DownloadClicked(object sender, RoutedEventArgs e)
        {
            DownloadPack();
        }

        static void RedownloadPack()
        {

            string url = "https://github.com/kindawindytoday/Minty-Releases/releases/latest/download/minty.zip";
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "MintyLauncher", "minty.zip");
            string extractPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "MintyLauncher");

            using (WebClient client = new WebClient())
            {
                try
                {
                 // Создаем директорию, если она не существует
                 if (!Directory.Exists(extractPath))
                 {
                  Directory.CreateDirectory(extractPath);
                  Console.WriteLine("Создана директория: " + extractPath);
                 }

                 // Скачиваем файл
                 client.DownloadFile(url, downloadPath);
                 Console.WriteLine("Файл успешно скачан: " + downloadPath);

                 // Распаковка ZIP-архива с использованием DotNetZip
                 using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(downloadPath))
                 {
                  zip.ExtractAll(extractPath, ExtractExistingFileAction.OverwriteSilently);
                 }
                 Console.WriteLine("Архив успешно распакован в: " + extractPath);

                 // Удаляем ZIP-файл после распаковки
                 File.Delete(downloadPath);
                 Console.WriteLine("ZIP файл успешно удалён: " + downloadPath);
                }
                catch (UnauthorizedAccessException uaEx)
                {
                    Console.WriteLine("Ошибка доступа: " + uaEx.Message);
                }
                catch (DirectoryNotFoundException dnEx)
                {
                    Console.WriteLine("Директорий не найден: " + dnEx.Message);
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine("Ошибка ввода-вывода: " + ioEx.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }

                
        }

        private void RedownloadClicked(object sender, RoutedEventArgs e)
        {
            RedownloadPack();
        }

        private void GitHub_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://github.com/dispiritee/Minty-Launcher";
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }

        private void Discord_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://dsc.gg/mintylaunch";
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OpenSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Settings settingsWindow = new Settings();
            settingsWindow.Show();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDragging = true;
                dragStartPoint = e.GetPosition(this);
                Mouse.Capture((Rectangle)sender);
            }
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentMousePosition = Mouse.GetPosition(this);

                // Вычисляем смещение
                double offsetX = currentMousePosition.X - dragStartPoint.X;
                double offsetY = currentMousePosition.Y - dragStartPoint.Y;

                // Перемещаем окно на основе текущих координат мыши
                this.Left += offsetX;
                this.Top += offsetY;

                // Обновляем точку начала перетаскивания
                dragStartPoint = currentMousePosition;
            }
        }

        private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                Mouse.Capture(null);
            }
        }
    }
}
