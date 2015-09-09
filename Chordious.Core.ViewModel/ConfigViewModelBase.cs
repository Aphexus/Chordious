﻿// 
// ConfigViewModelBase.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015 Jon Thysell <http://jonthysell.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public abstract class ConfigViewModelBase : ViewModelBase
    {
        public AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public abstract string Title { get; }

        public bool IsIdle
        {
            get
            {
                return _isIdle;
            }
            protected set
            {
                _isIdle = value;
                RaisePropertyChanged("IsIdle");
            }
        }
        private bool _isIdle;

        public bool IncludeSettings
        {
            get
            {
                return _includeSettings;
            }
            set
            {
                _includeSettings = value;
                RaisePropertyChanged("IncludeSettings");
            }
        }
        private bool _includeSettings;

        public bool IncludeStyles
        {
            get
            {
                return _includeStyles;
            }
            set
            {
                _includeStyles = value;
                RaisePropertyChanged("IncludeStyles");
            }
        }
        private bool _includeStyles;

        public bool IncludeInstruments
        {
            get
            {
                return _includeInstruments;
            }
            set
            {
                _includeInstruments = value;
                RaisePropertyChanged("IncludeInstruments");
            }
        }
        private bool _includeInstruments;

        public bool IncludeChordQualities
        {
            get
            {
                return _includeChordQualities;
            }
            set
            {
                _includeChordQualities = value;
                RaisePropertyChanged("IncludeChordQualities");
            }
        }
        private bool _includeChordQualities;

        public bool IncludeScales
        {
            get
            {
                return _includeScales;
            }
            set
            {
                _includeScales = value;
                RaisePropertyChanged("IncludeScales");
            }
        }
        private bool _includeScales;

        public bool IncludeLibrary
        {
            get
            {
                return _includeLibrary;
            }
            set
            {
                _includeLibrary = value;
                RaisePropertyChanged("IncludeLibrary");
            }
        }
        private bool _includeLibrary;

        public RelayCommand Cancel
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        OnRequestClose();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public event Action RequestClose;

        public ConfigViewModelBase()
        {
            IncludeSettings = true;
            IncludeStyles = true;
            IncludeInstruments = true;
            IncludeChordQualities = true;
            IncludeScales = true;
            IncludeLibrary = true;
            IsIdle = true;
        }

        protected void OnRequestClose()
        {
            if (null != RequestClose)
            {
                RequestClose();
            }
        }

        protected ConfigParts GetConfigParts()
        {
            ConfigParts configParts = ConfigParts.None;

            configParts |= IncludeSettings ? ConfigParts.Settings : ConfigParts.None;
            configParts |= IncludeStyles ? ConfigParts.Styles : ConfigParts.None;
            configParts |= IncludeInstruments ? ConfigParts.Instruments : ConfigParts.None;
            configParts |= IncludeChordQualities ? ConfigParts.Qualities : ConfigParts.None;
            configParts |= IncludeScales ? ConfigParts.Scales : ConfigParts.None;
            configParts |= IncludeLibrary ? ConfigParts.Library : ConfigParts.None;

            return configParts;
        }
    }
}