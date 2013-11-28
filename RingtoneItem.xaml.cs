using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using XmasRingtones.ViewModel;

namespace XmasRingtones
{
    public partial class RingtoneItem : UserControl
    {
        private RingtoneItemViewModel _viewModel;

        public RingtoneItem()
        {
            InitializeComponent();

            _viewModel = (RingtoneItemViewModel)DataContext;
        }

        public RingtoneItem(Ringtone ringtone)
            : this()
        {
            _viewModel.SetRingtone(ringtone.Source);
            _viewModel.Title = ringtone.Title;
            _viewModel.Icon = ringtone.Icon;
        }

        private void PlayPause_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _viewModel.PlayPause();
        }

        private void Favorite_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _viewModel.SetFavorite();
        }
    }
}
