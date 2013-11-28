using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GoogleAds;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace XmasRingtones
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            var ringtones = Ultility.GetAllRingtone();
            foreach (var ringtone in ringtones)
            {
                var ringtoneItem = new RingtoneItem(ringtone);
                RingtoneListBox.Items.Add(ringtoneItem);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var saveRingtoneChooser = new SaveRingtoneTask();
            saveRingtoneChooser.Completed += saveRingtoneChooser_Completed;
            saveRingtoneChooser.Source = new Uri("appdata:/Ringtones/a1.mp3");


            saveRingtoneChooser.DisplayName = "My custom ringtone";

            saveRingtoneChooser.Show();
        }

        private void saveRingtoneChooser_Completed(object sender, TaskEventArgs e)
        {
            switch (e.TaskResult)
            {
                //Logic for when the ringtone was saved successfully
                case TaskResult.OK:
                    MessageBox.Show("Ringtone saved.");
                    break;

                //Logic for when the task was cancelled by the user
                case TaskResult.Cancel:
                    MessageBox.Show("Save cancelled.");
                    break;

                //Logic for when the ringtone could not be saved
                case TaskResult.None:
                    MessageBox.Show("Ringtone could not be saved.");
                    break;
            }
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {

            var bannerAd = new AdView
            {
                Format = AdFormats.SmartBanner,
                AdUnitID = "a152956acb16238"
            };
            bannerAd.ReceivedAd += OnAdReceived;
            bannerAd.FailedToReceiveAd += OnFailedToReceiveAd;
            bannerAd.SetValue(Grid.RowProperty, 1);
            LayoutRoot.Children.Add(bannerAd);
            var adRequest = new AdRequest();
            bannerAd.LoadAd(adRequest);
        }

        private void OnAdReceived(object sender, AdEventArgs e)
        {
            Debug.WriteLine("Received ad successfully");
        }

        private void OnFailedToReceiveAd(object sender, AdErrorEventArgs errorCode)
        {
            Debug.WriteLine("Failed to receive ad with error " + errorCode.ErrorCode);
        }
    }
}