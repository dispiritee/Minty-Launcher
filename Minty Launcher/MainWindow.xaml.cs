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
using System.Windows.Forms;

namespace Minty_Launcher
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string cfgDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "MintyLauncher");
        private string selectedFolderPath; // Хранит путь к выбранной папке
        private bool _isMenuVisible = false;
        private string settingsFilePath;


        public MainWindow()
        {
            InitializeComponent();
            settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "MintyLauncher", "settings.txt");
            this.Opacity = 0; // Устанавливаем изначальную непрозрачность 0
            StartFadeOut();

            if (File.Exists(settingsFilePath))
            {
                var hideInfo = File.ReadAllText(settingsFilePath).Trim();
                if (hideInfo.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    InfoRectangle.Visibility = Visibility.Collapsed;
                    DiscordClose.Visibility = Visibility.Collapsed;
                    DiscordInfo.Visibility = Visibility.Collapsed;
                    InfoRectangle2.Visibility = Visibility.Collapsed;
                    DontShow.Visibility = Visibility.Collapsed;
                    DontShowAgainCheckBox.Visibility = Visibility.Collapsed;
                }
            }
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

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка существования директории
            if (!Directory.Exists(cfgDirectory))
            {
                ShowTextWithAnimation("Такой директории не существует");
                return;
            }

            // Открываем диалог выбора файла
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                string destinationPath = Path.Combine(cfgDirectory, Path.GetFileName(selectedFilePath));

                // Копируем файл в целевую директорию
                File.Copy(selectedFilePath, destinationPath, true);
                ShowTextWithAnimation("Файл успешно скопирован!");
            }
        }

        private void ShowTextWithAnimation(string message)
        {
            AnimatedTextBlock.Text = message;

            // Создаем Storyboard для анимации
            Storyboard storyboard = new Storyboard();

            // Анимация появления
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };
            Storyboard.SetTarget(fadeInAnimation, AnimatedTextBlock);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));
            storyboard.Children.Add(fadeInAnimation);

            // Анимация исчезновения
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                BeginTime = TimeSpan.FromSeconds(3) // Начинаем через 3 секунды
            };
            Storyboard.SetTarget(fadeOutAnimation, AnimatedTextBlock);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity"));
            storyboard.Children.Add(fadeOutAnimation);

            // Очищаем текст после окончания анимации исчезновения
            fadeOutAnimation.Completed += (s, e) => AnimatedTextBlock.Text = string.Empty;

            // Запускаем Storyboard
            storyboard.Begin();
        }

        private void FolderButton_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "MintyLauncher", "ScreenshotFolder.txt");

            // Проверка, существует ли файл
            if (File.Exists(folderPath))
            {
                // Если файл существует, читаем путь из файла
                selectedFolderPath = File.ReadAllText(folderPath);
                Process.Start("explorer.exe", selectedFolderPath);
                return;
            }

            // Если файл не существует, открываем диалог для выбора папки
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Выберите папку для открытия";
                folderDialog.ShowNewFolderButton = true; // Позволить создать новую папку

                // Проверка результата диалога
                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    selectedFolderPath = folderDialog.SelectedPath;

                    // Создаем директорию, если она не существует
                    string launcherPath = Path.GetDirectoryName(folderPath);
                    if (!Directory.Exists(launcherPath))
                    {
                        Directory.CreateDirectory(launcherPath);
                    }

                    // Сохранение пути в файл ScreenshotFolder.txt
                    File.WriteAllText(folderPath, selectedFolderPath);

                    // Открытие выбранной папки в проводнике
                    Process.Start("explorer.exe", selectedFolderPath);
                }
            }
        }

        private void StartFadeOut()
        {
            var fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(3)),
                BeginTime = TimeSpan.FromSeconds(5)
            };

            fadeOutAnimation.Completed += (s, e) => ContentGrid.Visibility = Visibility.Collapsed;
            ContentGrid.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
        }

        private void PlayButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MouseEnterStoryboard");
            sb.Begin();
        }
        private void PlayButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MouseLeaveStoryboard");
            sb.Begin();
        }
        private void PlayButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MouseDownStoryboard");
            sb.Begin();
        }
        private void PlayButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MouseUpStoryboard");
            sb.Begin();
        }

        private void ToggleMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isMenuVisible)
            {
                HideMenu();
            }
            else
            {
                ShowMenu();
            }

            _isMenuVisible = !_isMenuVisible; // Переключаем состояние
        }

        private void ShowMenu()
        {
            MenuPanel.Visibility = Visibility.Visible;
            Storyboard sb = (Storyboard)FindResource("MenuShowStoryboard");
            sb.Begin();
        }
        private void HideMenu()
        {
            Storyboard sb = (Storyboard)FindResource("MenuHideStoryboard");
            sb.Completed += (s, e) => MenuPanel.Visibility = Visibility.Collapsed; // Скрыть, когда закончится анимация
            sb.Begin();
        }

        private void MenuRedownload_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MenuRedownloadEnterStoryboard");
            sb.Begin();
        }
        private void MenuRedownload_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MenuRedownloadLeaveStoryboard");
            sb.Begin();
        }
        private void MenuRedownload_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MenuRedownloadDownStoryboard");
            sb.Begin();
        }
        private void MenuRedownload_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MenuRedownloadUpStoryboard");
            sb.Begin();
        }

        private void MenuCfg_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MenuRedownloadEnterStoryboard");
            sb.Begin();
        }
        private void MenuCfg_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MenuRedownloadLeaveStoryboard");
            sb.Begin();
        }
        private void MenuCfg_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MenuRedownloadDownStoryboard");
            sb.Begin();
        }
        private void MenuCfg_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MenuRedownloadUpStoryboard");
            sb.Begin();
        }

        private void MenuScreenshot_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MenuRedownloadEnterStoryboard");
            sb.Begin();
        }
        private void MenuScreenshot_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MenuRedownloadLeaveStoryboard");
            sb.Begin();
        }
        private void MenuScreenshot_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MenuRedownloadDownStoryboard");
            sb.Begin();
        }
        private void MenuScreenshot_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("MenuRedownloadUpStoryboard");
            sb.Begin();
        }

        private void HideRectangleButton_Click(object sender, RoutedEventArgs e)
        {
            InfoRectangle.Visibility = Visibility.Collapsed;
            InfoRectangle2.Visibility = Visibility.Collapsed;
            DiscordClose.Visibility = Visibility.Collapsed;
            DiscordInfo.Visibility = Visibility.Collapsed;
            DontShow.Visibility = Visibility.Collapsed;
            DontShowAgainCheckBox.Visibility = Visibility.Collapsed;

            // Сохранить состояние CheckBox
            if (DontShowAgainCheckBox.IsChecked == true)
            {
                File.WriteAllText(settingsFilePath, "true");
            }
        }
    }
}