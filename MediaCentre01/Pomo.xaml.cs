using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace MediaCentre01
{
    /// <summary>
    /// Interaction logic for Pomo.xaml
    /// </summary>
    public partial class Pomo : Window
    {
        private NotifyIcon notifyIcon;
        private System.ComponentModel.BackgroundWorker worker;

        private bool isBeeping = false;
        private bool isWorking = false;

        public Pomo()
        {

            worker = new BackgroundWorker();
            worker.DoWork += WorkerDoWork;
            worker.RunWorkerCompleted += WorkerDone;

            InitializeComponent();

            this.Icon = BitmapFrame.Create(new Uri(@"tomato.png", UriKind.Relative));

            BtnStpWork.Visibility = Visibility.Hidden;

            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new Icon(@"tomato.ico");

            ToolStripMenuItem tmiClose = new ToolStripMenuItem();
            tmiClose.Text = "Close";
            tmiClose.Click += new System.EventHandler(TmiClose_Click);

            ToolStripMenuItem tmiShow = new ToolStripMenuItem();
            tmiShow.Text = "Show";
            tmiShow.Click += TmiShow_Click;

            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

            contextMenuStrip.Items.Add(tmiShow);
            contextMenuStrip.Items.Add(tmiClose);

            notifyIcon.ContextMenuStrip = contextMenuStrip;
            notifyIcon.BalloonTipText =
                Properties.Resources.Pomo_Tray_Stuff;
            notifyIcon.Visible = true;

        }

        private void TmiShow_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        // Close from the Tray Icon.
        private void TmiClose_Click(object sender, System.EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void BtnWork_OnClick(object sender, RoutedEventArgs e)
        {
            //this.ShowInTaskbar = false;
            //this.Hide();
            worker.RunWorkerAsync();

            BtnWork.Visibility = Visibility.Hidden;
            BtnStpWork.Visibility = Visibility.Visible;

        }

        private void WorkerDoWork(Object sender, DoWorkEventArgs e)
        {

            isWorking = true;

            // Get the time in milliseconds
            AppSettingsReader asr = new AppSettingsReader();
            int workTimeMs = 60 * 1000 * (int) asr.GetValue("PomodoroPeriod", typeof(int));

            int timer = 0;
            while (timer < workTimeMs && isWorking)
            {
                BackgroundWorker worker = (sender as BackgroundWorker);
                System.Threading.Thread.Sleep(1000);
                timer += 1000;
            }
            
        }

        private async void WorkerDone(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Show();

            var uri = new Uri(@"beeps.wav", UriKind.Relative);

            SoundPlayer p = new SoundPlayer(@"beeps.wav");

            isWorking = false;
            isBeeping = true;

            while (isBeeping)
            {
                p.Play();
                await Task.Delay(500);
            }

        }

        private void BtnStpWork_OnClick(object sender, RoutedEventArgs e)
        {
            if (isWorking)
            {
                isWorking = false;
            }

            if (isBeeping)
            {
                isBeeping = false;
            }

            BtnStpWork.Visibility = Visibility.Hidden;
            BtnWork.Visibility = Visibility.Visible;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            notifyIcon.ShowBalloonTip(2000);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

    }
}
