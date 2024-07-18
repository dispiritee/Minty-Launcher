using System;
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

namespace Minty_Launcher
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static string tempPath = Environment.GetEnvironmentVariable("TEMP");

        static async Task DownloadPack()
        {
            IniFile config = new IniFile("settings.ini");

            string dataURL = "https://github.com/kindawindytoday/Minty-Releases/releases/latest/download/minty.zip";

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(dataURL))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        byte[] rawFileBytes = await response.Content.ReadAsByteArrayAsync();
                        System.IO.File.WriteAllBytes(tempPath, rawFileBytes);
                        MessageBox.Show("Файл успешно скачан!");
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка при скачивании пакета!");
                    }
                }
            }
        }

        private void DownloadClicked(object sender, RoutedEventArgs e)
        {
            DownloadPack();
        }
    }
}
