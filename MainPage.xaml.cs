using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using GoogleAds;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Media;
using XmasRingtones.ViewModel;

namespace XmasRingtones
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool _hasRingtonePlaying = false;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            LoadAllRingtone();
            LoadFavorite();

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            timer.Tick += CheckMediaPlayerState;
            timer.Start();
        }

        private void CheckMediaPlayerState(object sender, EventArgs e)
        {
            if (!_hasRingtonePlaying && MediaPlayer.State == MediaState.Playing)
            {
                _hasRingtonePlaying = true;
                return;
            }
            if (_hasRingtonePlaying && MediaPlayer.State != MediaState.Playing)
            {
                PauseAll();
            }
        }

        private const string FavoriteListSetting = "FavoriteList";

        private List<Ringtone> _allRingtones = new List<Ringtone>();

        private void LoadAllRingtone()
        {
            _allRingtones = Ultility.GetAllRingtone();
            foreach (var ringtone in _allRingtones)
            {
                var ringtoneItem = new RingtoneItem(ringtone);
                ringtoneItem.PlayPauseButtonClick += RingtoneItemPlayPauseButtonClick;
                ringtoneItem.FavoriteButonClick += RingtoneItemFavoriteButonClick;
                RingtoneListBox.Items.Add(ringtoneItem);
            }

        }

        private void LoadFavorite()
        {
            FavoriteListBox.Items.Clear();
            if (GlobalVariables.FavoriteRingtones.Count == 0)
            {
                string favoriteList;
                if (AppSettings.TryGetSetting(FavoriteListSetting, out favoriteList))
                {
                    var split = Regex.Split(favoriteList, ";");
                    if (split.Any())
                    {
                        foreach (var s in split)
                        {
                            for (int index = 0; index < _allRingtones.Count; index++)
                            {
                                var rt = _allRingtones[index];
                                if (rt.Source == s)
                                {
                                    GlobalVariables.FavoriteRingtones.Add(rt);
                                    var rtItem = (RingtoneItem)RingtoneListBox.Items[index];
                                    rtItem.IsFavorite = true;
                                }
                            }
                        }
                    }
                }
            }
            foreach (var ringtone in GlobalVariables.FavoriteRingtones)
            {
                var ringtoneItem = new RingtoneItem(ringtone, isFavorite: true);
                ringtoneItem.PlayPauseButtonClick += RingtoneItemPlayPauseButtonClick;
                ringtoneItem.FavoriteButonClick += RingtoneItemFavoriteButonClick;
                FavoriteListBox.Items.Add(ringtoneItem);
            }
        }

        private void RingtoneItemFavoriteButonClick(object sender)
        {
            var item = (RingtoneItem)sender;
            if (item.IsFavorite)
            {
                if (!GlobalVariables.FavoriteRingtones.Contains(item.Source))
                {
                    GlobalVariables.FavoriteRingtones.Add(item.Source);
                }
            }
            else
            {
                if (GlobalVariables.FavoriteRingtones.Contains(item.Source))
                {
                    GlobalVariables.FavoriteRingtones.Remove(item.Source);
                }
            }
            SaveFavorite();
            LoadFavorite();
            if (PivotContainer.SelectedIndex == 1)
            {
                CheckFavoriteInRingtoneListBox();
            }
        }

        private void SaveFavorite()
        {
            string v = string.Empty;
            foreach (var ringtone in GlobalVariables.FavoriteRingtones)
            {
                v += ringtone.Source + ";";
            }
            v = v.Remove(v.Count() - 1);
            AppSettings.StoreSetting(FavoriteListSetting, v);
        }

        private void CheckFavoriteInRingtoneListBox()
        {
            RingtoneItem item;
            foreach (var ringtoneItem in RingtoneListBox.Items)
            {
                item = (RingtoneItem)ringtoneItem;
                item.IsFavorite = GlobalVariables.FavoriteRingtones.Contains(item.Source);
            }
        }

        private void RingtoneItemPlayPauseButtonClick()
        {
            PauseAll();
        }

        private void PauseAll()
        {
            _hasRingtonePlaying = false;            
            RingtoneItem ringtoneItem;
            foreach (var item in RingtoneListBox.Items)
            {
                ringtoneItem = (RingtoneItem)item;
                if (ringtoneItem.PlayingState == RingtoneItemViewModel.PlayState.Play)
                {
                    ringtoneItem.PlayPause();
                }
            }
            foreach (var item in FavoriteListBox.Items)
            {
                ringtoneItem = (RingtoneItem)item;
                if (ringtoneItem.PlayingState == RingtoneItemViewModel.PlayState.Play)
                {
                    ringtoneItem.PlayPause();
                }
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

        private void PivotContainer_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PauseAll();
        }
    }
}