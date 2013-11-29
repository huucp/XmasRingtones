using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace XmasRingtones.ViewModel
{
    public class RingtoneItemViewModel : ViewModelBase
    {
        private string _icon = string.Empty;
        public string Icon
        {
            get { return _icon; }
            set
            {
                if (_icon == value) return;
                _icon = value;
                NotifyPropertyChanged("Icon");
            }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private string _playPauseIcon = null;
        public string PlayPauseIcon
        {
            get { return _playPauseIcon; }
            set
            {
                if (_playPauseIcon == value) return;
                _playPauseIcon = value;
                NotifyPropertyChanged("PlayPauseIcon");
            }
        }

        private string _favoriteIcon = null;
        public string FavoriteIcon
        {
            get { return _favoriteIcon; }
            set
            {
                if (_favoriteIcon == value) return;
                _favoriteIcon = value;
                NotifyPropertyChanged("FavoriteIcon");
            }
        }

        public enum PlayState
        {
            Play, Pause
        }

        public PlayState PlayingState = PlayState.Pause;

        public void PlayPause()
        {
            if (PlayingState == PlayState.Pause)
            {
                PlayingState = PlayState.Play;
                Play();
            }
            else
            {
                PlayingState = PlayState.Pause;
                Pause();
            }
        }

        private void Play()
        {
            var uri = new Uri(_ringtone, UriKind.Relative);
            FrameworkDispatcher.Update();
            MediaPlayer.Play(Song.FromUri(_ringtone, uri));
            PlayPauseIcon = ImageSource.PauseIcon;
        }

        private void Pause()
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }
            PlayPauseIcon = ImageSource.PlayIcon;
        }

        
        private string _ringtone = string.Empty;
        public void SetRingtone(string ringtone)
        {
            _ringtone = ringtone;
        }

        public bool IsFavorite = false;
        public void SetFavorite(bool isFavorite)
        {
            IsFavorite = isFavorite;
            FavoriteIcon = IsFavorite ? ImageSource.FavoriteGlowIcon : ImageSource.FavoriteIcon;
        }
    }
}
