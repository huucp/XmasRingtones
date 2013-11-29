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
using Microsoft.Phone.Tasks;
using XmasRingtones.ViewModel;

namespace XmasRingtones
{
    public partial class RingtoneItem : UserControl
    {
        private RingtoneItemViewModel _viewModel;
        public Ringtone Source;

        public RingtoneItem()
        {
            InitializeComponent();

            _viewModel = (RingtoneItemViewModel)DataContext;
        }

        public RingtoneItem(Ringtone ringtone, bool isFavorite = false)
            : this()
        {
            Source = ringtone;
            _viewModel.SetRingtone(ringtone.Source);
            _viewModel.Title = ringtone.Title;
            _viewModel.Icon = ringtone.Icon;
            if (isFavorite) IsFavorite = true;
        }

        private void PlayPause_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PlayingState == RingtoneItemViewModel.PlayState.Pause) OnPlayPauseButtonClick();
            PlayPause();
        }

        public void PlayPause()
        {
            _viewModel.PlayPause();
        }

        private void Favorite_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _viewModel.SetFavorite(!IsFavorite);
            OnFavoriteButonClick();
        }

        public RingtoneItemViewModel.PlayState PlayingState
        {
            get { return _viewModel.PlayingState; }
        }

        public bool IsFavorite
        {
            get { return _viewModel.IsFavorite; }
            set
            {
                _viewModel.SetFavorite(value);
            }
        }

        #region event

        public delegate void PlayPauseButtonOnClickEventHandler();

        public event PlayPauseButtonOnClickEventHandler PlayPauseButtonClick;

        private void OnPlayPauseButtonClick()
        {
            PlayPauseButtonOnClickEventHandler handler = PlayPauseButtonClick;
            if (handler != null) handler();
        }


        public delegate void FavoriteButtonClickEventHandler(object sender);

        public event FavoriteButtonClickEventHandler FavoriteButonClick;

        private void OnFavoriteButonClick()
        {
            FavoriteButtonClickEventHandler handler = FavoriteButonClick;
            if (handler != null) handler(this);
        }

        private void SetRingtone_OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var saveRingtoneChooser = new SaveRingtoneTask();
            saveRingtoneChooser.Completed += saveRingtoneChooser_Completed;
            saveRingtoneChooser.Source = new Uri("appdata:" + Source.Source);


            saveRingtoneChooser.DisplayName = Source.Title;

            saveRingtoneChooser.Show();
        }

        private void saveRingtoneChooser_Completed(object sender, TaskEventArgs e)
        {
            switch (e.TaskResult)
            {
                //Logic for when the ringtone was saved successfully
                case TaskResult.OK:
                    // MessageBox.Show("Ringtone saved.");
                    break;

                //Logic for when the task was cancelled by the user
                case TaskResult.Cancel:
                    // MessageBox.Show("Save cancelled.");
                    break;

                //Logic for when the ringtone could not be saved
                case TaskResult.None:
                    MessageBox.Show("Ringtone could not be saved.");
                    break;
            }
        }

        #endregion
    }
}
