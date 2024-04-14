using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfLessonApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string word = "Завдання";
        public MainWindow()
        {
            InitializeComponent();
        }

        //private async void bt_Click(object sender, RoutedEventArgs e)
        //{

        //    string filePath = "sys_004.txt";
        //    try
        //    {
        //        string fileContent;
        //        using (StreamReader sr = new StreamReader(filePath))
        //        {
        //            fileContent = await sr.ReadToEndAsync();                     
        //        }
        //        tb.Text = fileContent;
        //    }
        //    catch (FileNotFoundException)
        //    {
        //        MessageBox.Show("Файл не знайдено!");
        //    }
        //}
        private async void bt_Click(object sender, RoutedEventArgs e)
        {
            string path = tbx.Text;
            try
            {
                string filesList = await GetFoldersAsync(path);
                tb.Text = filesList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
        }
        private async Task<string> GetFoldersAsync(string path)
        {
            try
            {
                StringBuilder filesList = new StringBuilder();

                foreach (var file in Directory.EnumerateFiles(path))
                {
                    filesList.AppendLine(file);
                }

                foreach (var folder in Directory.EnumerateDirectories(path))
                {
                    string subFolderFiles = await GetFoldersAsync(folder);
                    filesList.AppendLine(subFolderFiles);
                }

                return filesList.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка обробки каталогу {path}: {ex.Message}");
            }
        }
    }
}