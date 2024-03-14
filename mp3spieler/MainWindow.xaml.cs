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
    public partial class MainWindow : Window
    {
        //ползунокползунокползунокползунокползунокползунокползунокползунокползунокползунокползунокползунокползунокползунокползунок
        private int c;
        private string pesnyaNow;
        private TimeSpan currentPosition;
        private List<string> pesni                  = new List<string>();
        public  List<string> hist                   = new List<string>();
        private MediaPlayer pl                      = new MediaPlayer();

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
                pesnyaNow = (string)ListPesen.SelectedItem;
            }
        }

        private void VkluchitPesnuUnderIndex()
        {
            if (c < hist.Count)
            {
                pl.Open(new Uri(hist[c]));
                pl.Position = currentPosition;
                pl.Play();
                pesnyaNow = hist[c];
            }
            else return;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (hist.Count != 0 && c != 0)
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

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            Random ra = new Random();
            int rand = ra.Next(0, pesni.Count);
            if (pesni.Count != 0)
            {
                pl.Open(new Uri(pesni[rand]));
                pl.Position = currentPosition;
                pl.Play();
                hist.Add(pesni[rand]);
                c = hist.Count - 1;
                pesnyaNow = pesni[rand];
            }
            else return;
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (pesnyaNow != null)
            {
                pl.Open(new Uri((string)pesnyaNow));
                pl.Position = currentPosition;
                pl.Play();
            }
            else return;
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            Window1 wind = new Window1();
            wind.Show(); //доделай эту ебалу
        }
    }
}
