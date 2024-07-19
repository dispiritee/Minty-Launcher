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
using System.Diagnostics;

namespace Minty_Launcher
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {

        private bool isDragging = false;
        private Point dragStartPoint;

        public Settings()
        {
            InitializeComponent();
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GitHub_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://github.com/kindawindytoday/Minty-Releases";
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }

        private void Discord_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://discord.gg/kindawindytoday";
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
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
