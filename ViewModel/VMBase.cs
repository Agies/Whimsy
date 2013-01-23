using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.UI.Core;

namespace WhimsyEarlierLiteracy.ViewModel
{
    public class VMBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (DispatcherHelper.Dispatcher != null && !DispatcherHelper.Dispatcher.HasThreadAccess)
            {
                DispatcherHelper.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                                     () => OnPropertyChanged(propertyName));
                return;
            }
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MainPageVM : VMBase
    {
        private List<LetterVM> _letters;

        public MainPageVM()
        {
            Letters = Enumerable.Range(65, 26).Select(i => new LetterVM(((char) i).ToString())).ToList();
        }

        public List<LetterVM> Letters
        {
            get { return _letters; }
            set
            {
                if (_letters == value) return;
                _letters = value;
                OnPropertyChanged("Letters");
            }
        }

        public void LoadSymbolsFor(Geoposition position)
        {
        }

        public void WasShaken()
        {
        }
    }

    public class LetterVM : VMBase
    {
        private static Random _rand;
        private string _smallImage;
        private TimeSpan _updateInterval;

        static LetterVM()
        {
            _rand = new Random();
        }

        public LetterVM(string letter)
        {
            Character = letter;
            SmallImage = "Assets/" + letter.ToUpper() + "/SmallImage.png";
            // Causes the tiles to flip at different times giving the metro look
            UpdateInterval = TimeSpan.FromSeconds(_rand.Next(8, 25));
            Symbols = new List<SymbolVM>();
        }

        public string Character { get; set; }

        public TimeSpan UpdateInterval
        {
            get { return _updateInterval; }
            set
            {
                if (_updateInterval == value) return;
                _updateInterval = value;
                OnPropertyChanged("UpdateInterval");
            }
        }

        public List<SymbolVM> Symbols { get; set; }

        public string SmallImage
        {
            get { return _smallImage; }
            set
            {
                if (_smallImage == value) return;
                _smallImage = value;
                OnPropertyChanged("SmallImage");
            }
        }
    }

    public class SymbolVM : VMBase
    {
        private string _image;
        private string _sound;

        private string _title;

        public string Image
        {
            get { return _image; }
            set
            {
                if (_image == value) return;
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public string Sound
        {
            get { return _sound; }
            set
            {
                if (_sound == value) return;
                _sound = value;
                OnPropertyChanged("Sound");
            }
        }
    }
}