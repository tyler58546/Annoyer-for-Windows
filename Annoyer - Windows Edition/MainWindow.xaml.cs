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
using System.Speech;
using System.Speech.Synthesis;
using System.IO;
using System.Media;

namespace Annoyer___Windows_Edition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (Convert.ToBoolean(Properties.Settings.Default["autoArm"]))
            {
                armCheck.IsChecked = true;
                toneButton.IsEnabled = true;
            }
            genderBox.SelectedIndex = Convert.ToInt32(Properties.Settings.Default["defaultVoice"]);
            ttsField.Text = Convert.ToString(Properties.Settings.Default["DefaultText"]);
        }

        
        SpeechSynthesizer speechSynthesizerObj;
        Settings settingsWindow = new Settings();

        public static void PlayBeep(UInt16 frequency, int msDuration, UInt16 volume = 16383)
        {
            var mStrm = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mStrm);

            const double TAU = 2 * Math.PI;
            int formatChunkSize = 16;
            int headerSize = 8;
            short formatType = 1;
            short tracks = 1;
            int samplesPerSecond = 44100;
            short bitsPerSample = 16;
            short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
            int bytesPerSecond = samplesPerSecond * frameSize;
            int waveSize = 4;
            int samples = (int)((decimal)samplesPerSecond * msDuration / 1000);
            int dataChunkSize = samples * frameSize;
            int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
            writer.Write(0x46464952); // = encoding.GetBytes("RIFF")
            writer.Write(fileSize);
            writer.Write(0x45564157); // = encoding.GetBytes("WAVE")
            writer.Write(0x20746D66); // = encoding.GetBytes("fmt ")
            writer.Write(formatChunkSize);
            writer.Write(formatType);
            writer.Write(tracks);
            writer.Write(samplesPerSecond);
            writer.Write(bytesPerSecond);
            writer.Write(frameSize);
            writer.Write(bitsPerSample);
            writer.Write(0x61746164); // = encoding.GetBytes("data")
            writer.Write(dataChunkSize);
            {
                double theta = frequency * TAU / (double)samplesPerSecond;
                // 'volume' is UInt16 with range 0 thru Uint16.MaxValue ( = 65 535)
                // we need 'amp' to have the range of 0 thru Int16.MaxValue ( = 32 767)
                double amp = volume >> 2; // so we simply set amp = volume / 2
                for (int step = 0; step < samples; step++)
                {
                    short s = (short)(amp * Math.Sin(theta * (double)step));
                    writer.Write(s);
                }
            }

            mStrm.Seek(0, SeekOrigin.Begin);
            new System.Media.SoundPlayer(mStrm).Play();
            writer.Close();
            mStrm.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            toneButton.IsEnabled = true;
        }

        private void ttsButton_Click(object sender, RoutedEventArgs e)
        {
            speechSynthesizerObj = new SpeechSynthesizer();
            speechSynthesizerObj.Dispose();
            speechSynthesizerObj = new SpeechSynthesizer();
            if (genderBox.SelectedIndex == 0)
            {
                speechSynthesizerObj.SelectVoiceByHints(VoiceGender.Male);
            }
            if (genderBox.SelectedIndex == 1)
            {
                speechSynthesizerObj.SelectVoiceByHints(VoiceGender.Female);
            }
            speechSynthesizerObj.SpeakAsync(ttsField.Text);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            toneButton.IsEnabled = false;
        }

        private void toneButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (toneSlider.Value == 0)
            {
                PlayBeep(1000, 1000);
            }
            if (toneSlider.Value == 1)
            {
                PlayBeep(5000, 1000);
            }
            if (toneSlider.Value == 2)
            {
                PlayBeep(10000, 1000);
            }
            if (toneSlider.Value == 3)
            {
                PlayBeep(15000, 1000);
            }
        }
        private void PlaySound1()
        {
            var uri = new Uri(@"snd01.mp3", UriKind.RelativeOrAbsolute);
            var player = new MediaPlayer();

            player.Open(uri);
            player.Play();
        }
        private void PlaySound2()
        {
            var uri = new Uri(@"snd02.mp3", UriKind.RelativeOrAbsolute);
            var player = new MediaPlayer();

            player.Open(uri);
            player.Play();
        }
        private void PlaySound3()
        {
            var uri = new Uri(@"snd03.mp3", UriKind.RelativeOrAbsolute);
            var player = new MediaPlayer();

            player.Open(uri);
            player.Play();
        }
        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                settingsWindow.Show();
            } catch (InvalidOperationException)
            {
                settingsWindow = new Settings();
                settingsWindow.Show();
            }
            

        }

        private void soundImg01_MouseUp(object sender, MouseButtonEventArgs e)
        {
            PlaySound1();
        }

        private void soundImg02_MouseUp(object sender, MouseButtonEventArgs e)
        {
            PlaySound2();
        }

        private void soundImg03_MouseUp(object sender, MouseButtonEventArgs e)
        {
            PlaySound3();
        }
    }
}
