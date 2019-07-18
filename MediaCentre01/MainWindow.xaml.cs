using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;

namespace MediaCentre01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;

        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (MePlayer.Source != null && MePlayer.NaturalDuration.HasTimeSpan)
            {
                SliProgress.Minimum = 0;
                SliProgress.MaxHeight = MePlayer.NaturalDuration.TimeSpan.TotalSeconds;
                SliProgress.Value = MePlayer.Position.TotalSeconds;
            }
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files(*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                MePlayer.Source = new Uri(openFileDialog.FileName);
            }
        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (MePlayer != null) && (MePlayer.Source != null);
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MePlayer.Play();
            mediaPlayerIsPlaying = true;
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MePlayer.Pause();
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MePlayer.Stop();
            mediaPlayerIsPlaying = false;
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MePlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        private void SliProgress_OnDragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void SliProgress_OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            MePlayer.Position = TimeSpan.FromSeconds(SliProgress.Value);
        }

        private void SliProgress_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LblProgressStatus.Text = TimeSpan.FromSeconds(SliProgress.Value).ToString();
        }

        private void Pomo_Startup(object sender, RoutedEventArgs e)
        {
            var pomoWnd = new Pomo();
            pomoWnd.Show();
        }
    }
}
