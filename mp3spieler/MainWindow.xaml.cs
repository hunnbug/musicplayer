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
        private TimeSpan currentPosition    = TimeSpan.Zero;
        private bool Repeating              = false;
        private List<string> pesni          = new List<string>();
        public  List<string> hist           = new List<string>();
        private MediaPlayer pl              = new MediaPlayer();

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
                pl.Open(new Uri((string)ListPesen.SelectedItem)); pl.Position = currentPosition;
                pl.Play();
                hist.Add((string)ListPesen.SelectedItem);

                for(int i = 0;i < pesni.Count; i++)
                {
                    if (pesni[i] == (string)ListPesen.SelectedItem) c = i;
                }
                pesnyaNow                   = (string)ListPesen.SelectedItem;
            }
        }

        private void VkluchitPesnuUnderIndex()
        {
            if (c < pesni.Count)
            {
                pl.Open(new Uri(pesni[c]));
                pl.Position                 = currentPosition;
                pesnyaNow                   = pesni[c];
                pl.Play();
                hist.Add(pesni[c]);
            }
            else return;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (pesni.Count != 0 && c != 0)
            {
                c--;
                VkluchitPesnuUnderIndex();
            }
            else return;
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (pesni.Count != 0 && c < pesni.Count)
            {
                c++;
                VkluchitPesnuUnderIndex();
            }
            else return;
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            Random ra                       = new Random();
            int rand                        = ra.Next(0, pesni.Count);
            if (pesni.Count != 0)
            {
                pl.Open(new Uri(pesni[rand])); pl.Position = currentPosition;
                pl.Play();

                hist.Add(pesni[rand]);
                c                           = pesni.Count - 1;
                pesnyaNow                   = pesni[rand];
            }
            else return;
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        { 
            if(Repeating == false) Repeating = true;
            if(Repeating == true)  Repeating = false;
            else return;
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var wind                        = new Window1();
            wind.HistoryList.ItemsSource    = hist;
            wind.ShowDialog();
            if (wind.DialogResult == false) wind.Close();
            if(wind.DialogResult == true)
            {
                for (int i = 0; i < pesni.Count; i++)
                {
                    if (pesni[i] == wind.selectedPesnya) c = i;
                }
                VkluchitPesnuUnderIndex();
            }
        }

        private void LengthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            pl.Position                     = new TimeSpan(Convert.ToInt64(LengthSlider.Value));
        }

        private void mediaOpened(object sender, RoutedEventArgs e)
        {
            LengthSlider.Maximum            = pl.NaturalDuration.TimeSpan.Ticks;
        }

        private void MediaEnded(object sender, RoutedEventArgs e)
        {
            if (Repeating == true) VkluchitPesnuUnderIndex();
        }
    }
}
