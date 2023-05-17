using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GameOfLife
{
    class AdWindow : Window
    {
        private readonly DispatcherTimer adTimer;
        private int imgNmb;     // the number of the image currently shown
        private string link;    // the URL where the currently shown ad leads to
        private readonly Dictionary<int, ImageBrush> images = new Dictionary<int, ImageBrush>();
        private const string Link = "http://example.com";


        public AdWindow(Window owner)
        {
            Random rnd = new Random();
            Owner = owner;
            Width = 350;
            Height = 100;
            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.ToolWindow;
            Title = "Support us by clicking the ads";
            Cursor = Cursors.Hand;
            ShowActivated = false;
            MouseDown += OnClick;
            imgNmb = rnd.Next(1, 3);
            FillImages();
            ChangeAds(this, new EventArgs());

            // Run the timer that changes the ad's image 
            adTimer = new DispatcherTimer();
            adTimer.Interval = TimeSpan.FromSeconds(3);
            adTimer.Tick += ChangeAds;
            adTimer.Start();
        }

        private void FillImages()
        {
            AddImage(1, "ad1.jpg");
            AddImage(2, "ad2.jpg");
            AddImage(3, "ad3.jpg");
        }

        private void AddImage(int number, string name)
        {
            ImageBrush myBrush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(name, UriKind.Relative))
            };
            images.Add(number, myBrush);
        }

        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(link);
            Close();
        }
        
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        } 

        public void Unsubscribe()
        {
            adTimer.Tick -= ChangeAds;
        }

        private void ChangeAds(object sender, EventArgs eventArgs)
        {
            Background = images[imgNmb];
            switch (imgNmb)
            {
                case 1:
                    link = Link;
                    imgNmb++;
                    break;
                case 2:
                    link = Link;
                    imgNmb++;
                    break;
                case 3:
                    link = Link;
                    imgNmb = 1;
                    break;
            }
            
        }
    }
}