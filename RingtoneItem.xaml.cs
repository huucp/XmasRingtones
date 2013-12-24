using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        public void PlayPause()
        {
            _viewModel.PlayPause();
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

        private bool _playPauseDown = false;
        private void PlayPause_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _playPauseDown = true;            
        }

        private void PlayPause_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        { 
            if (_playPauseDown)
            {
                _playPauseDown = false;
                if (PlayingState == RingtoneItemViewModel.PlayState.Pause) OnPlayPauseButtonClick();
                PlayPause();
            }
        }

        private bool _favoriteDown = false;
        private void Favorite_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _favoriteDown = true;
        }

        private void Favorite_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_favoriteDown)
            {
                _favoriteDown = false;
                _viewModel.SetFavorite(!IsFavorite);
                OnFavoriteButonClick();
            }
        }

        private bool _setRingtoneDown = false;
        private void SetRingtone_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _setRingtoneDown = true;
        }

        private void SetRingtone_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(_setRingtoneDown)
            {
                _setRingtoneDown = false;
                var saveRingtoneChooser = new SaveRingtoneTask();
                saveRingtoneChooser.Completed += saveRingtoneChooser_Completed;
                saveRingtoneChooser.Source = new Uri("appdata:" + Source.Source);
                saveRingtoneChooser.DisplayName = Source.Title;
                saveRingtoneChooser.Show();
            }
        }
    }
}
