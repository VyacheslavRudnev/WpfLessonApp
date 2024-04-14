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

       
        private async void bt_Click(object sender, RoutedEventArgs e)
        {
            string path = tbx.Text;
            List<string> files = new List<string>();

            tb.Text = "Починаємо перевірку файлів...\n";
            
            await Task.Run(() =>
            {
                GetFoldersAsync(path, files);
                Dispatcher.Invoke(() => tb.Text += $"{0}: {files.Count} {Environment.NewLine}");
                Parallel.ForEach(files, (file) => ReadFile(file).Wait());


            });
        }

        private void UpdateStatus(string status)
        {
            Dispatcher.Invoke(() =>
            {
                tb.Text += status + "\n";
            });
        }
        private async Task ReadFile(string path)
        {
            int count = 0;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string? text = await sr.ReadLineAsync();
                        if (text != null && text.Contains(word))
                        {
                            count++;
                        }
                    }
                }
            }
            Dispatcher.Invoke(() => tb.Text += $"{System.IO.Path.GetFileName(path)}:  {count}{Environment.NewLine}");
        }
        private async Task<string> GetFoldersAsync(string path, List<string> files)
        {
            try
            {
                StringBuilder filesList = new StringBuilder();

                foreach (var file in Directory.EnumerateFiles(path))
                {
                    files.Add(file);
                }

                foreach (var folder in Directory.EnumerateDirectories(path))
                {
                    GetFoldersAsync(folder, files);
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