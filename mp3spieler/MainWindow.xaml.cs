using System.Runtime.InteropServices;
using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Drawing;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Collections.Generic;

namespace mp3spieler
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int c;
        List<string> pesni                  = new List<string>();
        List<string> hist                   = new List<string>();
        MediaPlayer pl                      = new MediaPlayer();
        TimeSpan currentPosition;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenMusicFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dial                        = new CommonOpenFileDialog { IsFolderPicker = true };
            var result                      = dial.ShowDialog();


            if(result == CommonFileDialogResult.Ok)
            {
                string[] files              = Directory.GetFiles(dial.FileName);
                
                foreach(var file in files)
                {
                    if (file.EndsWith(".mp3") || file.EndsWith(".wav"))
                    {
                        pesni.Add(file);
                    }
                }
                ListPesen.ItemsSource       = pesni;
            }
        }

        private void ListPesen_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListPesen.SelectedItem != null)
            {
                pl.Open(new Uri((string)ListPesen.SelectedItem));
                pl.Position = currentPosition;
                pl.Play();
                hist.Add((string)ListPesen.SelectedItem);
                c = hist.Count - 1;
            }
        }

        private void VkluchitPesnuUnderIndex()
        {
            if (c < hist.Count)
            {
                pl.Open(new Uri(hist[c]));
                pl.Position = currentPosition;
                pl.Play();
            }
            else return;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (hist.Count != 0 && c != 0 && c != 0)
            {
                c--;
                VkluchitPesnuUnderIndex();
            }
            else return;
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (hist.Count != 0 && c != hist.Count - 1)
            {
                c++;
                VkluchitPesnuUnderIndex();
            }
            else return;
        }
    }
}
