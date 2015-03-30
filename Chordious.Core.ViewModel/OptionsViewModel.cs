﻿// 
// OptionsViewModel.cs
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
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class OptionsViewModel : ViewModelBase
    {
        public AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public string Title
        {
            get
            {
                string title = "Options";
                if (Dirty)
                {
                    title += "*";
                }
                return title;
            }
        }

        public string UserConfigPath
        {
            get
            {
                return AppVM.UserConfigPath;
            }
        }

        #region ChordFinder Defaults

        internal ChordFinderOptions ChordFinderOptionsBuffer { get; private set; }

        public int ChordFinderNumFrets
        {
            get
            {
                return ChordFinderOptionsBuffer.NumFrets;
            }
            set
            {
                try
                {
                    ChordFinderOptionsBuffer.NumFrets = value;
                    Dirty = true;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("ChordFinderNumFrets");
                    RaisePropertyChanged("ChordFinderMaxReach");
                }
            }
        }

        public int ChordFinderMaxFret
        {
            get
            {
                return ChordFinderOptionsBuffer.MaxFret;
            }
            set
            {
                try
                {
                    ChordFinderOptionsBuffer.MaxFret = value;
                    Dirty = true;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("ChordFinderMaxFret");
                }
            }
        }

        public int ChordFinderMaxReach
        {
            get
            {
                return ChordFinderOptionsBuffer.MaxReach;
            }
            set
            {
                try
                {
                    ChordFinderOptionsBuffer.MaxReach = value;
                    Dirty = true;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("ChordFinderNumFrets");
                    RaisePropertyChanged("ChordFinderMaxReach");
                }
            }
        }

        public bool ChordFinderAllowOpenStrings
        {
            get
            {
                return ChordFinderOptionsBuffer.AllowOpenStrings;
            }
            set
            {
                ChordFinderOptionsBuffer.AllowOpenStrings = value;
                Dirty = true;
                RaisePropertyChanged("ChordFinderAllowOpenStrings");
            }
        }

        public bool ChordFinderAllowMutedStrings
        {
            get
            {
                return ChordFinderOptionsBuffer.AllowMutedStrings;
            }
            set
            {
                ChordFinderOptionsBuffer.AllowMutedStrings = value;
                Dirty = true;
                RaisePropertyChanged("ChordFinderAllowMutedStrings");
            }
        }

        public bool ChordFinderAllowRootlessChords
        {
            get
            {
                return ChordFinderOptionsBuffer.AllowRootlessChords;
            }
            set
            {
                ChordFinderOptionsBuffer.AllowRootlessChords = value;
                Dirty = true;
                RaisePropertyChanged("ChordFinderAllowRootlessChords");
            }
        }

        internal ChordFinderStyle ChordFinderStyleBuffer { get; private set; }

        public bool ChordFinderAddTitle
        {
            get
            {
                return ChordFinderStyleBuffer.AddTitle;
            }
            set
            {
                ChordFinderStyleBuffer.AddTitle = value;
                Dirty = true;
                RaisePropertyChanged("ChordFinderAddTitle");
            }
        }

        public bool ChordFinderMirrorResults
        {
            get
            {
                return ChordFinderStyleBuffer.MirrorResults;
            }
            set
            {
                ChordFinderStyleBuffer.MirrorResults = value;
                Dirty = true;
                RaisePropertyChanged("ChordFinderMirrorResults");
            }
        }

        public int ChordFinderSelectedBarreTypeOptionIndex
        {
            get
            {
                return (int)ChordFinderStyleBuffer.BarreTypeOption;
            }
            set
            {
                ChordFinderStyleBuffer.BarreTypeOption = (BarreTypeOption)(value);
                Dirty = true;
                RaisePropertyChanged("ChordFinderSelectedBarreTypeOptionIndex");
            }
        }

        public ObservableCollection<string> ChordFinderBarreTypeOptions
        {
            get
            {
                return GetBarreTypeOptions();
            }
        }

        public bool ChordFinderAddRootNotes
        {
            get
            {
                return ChordFinderStyleBuffer.AddRootNotes;
            }
            set
            {
                ChordFinderStyleBuffer.AddRootNotes = value;
                Dirty = true;
                RaisePropertyChanged("ChordFinderAddRootNotes");
            }
        }

        public int ChordFinderSelectedMarkTextOptionIndex
        {
            get
            {
                return (int)ChordFinderStyleBuffer.MarkTextOption;
            }
            set
            {
                ChordFinderStyleBuffer.MarkTextOption = (MarkTextOption)(value);
                Dirty = true;
                RaisePropertyChanged("ChordFinderSelectedMarkTextOptionIndex");
            }
        }

        public ObservableCollection<string> ChordFinderMarkTextOptions
        {
            get
            {
                return GetMarkTextOptions();
            }
        }

        public bool ChordFinderAddBottomMarks
        {
            get
            {
                return ChordFinderStyleBuffer.AddBottomMarks;
            }
            set
            {
                ChordFinderStyleBuffer.AddBottomMarks = value;
                Dirty = true;
                RaisePropertyChanged("ChordFinderAddBottomMarks");
            }
        }

        public int ChordFinderSelectedBottomMarkTextOptionIndex
        {
            get
            {
                return (int)ChordFinderStyleBuffer.BottomMarkTextOption;
            }
            set
            {
                ChordFinderStyleBuffer.BottomMarkTextOption = (MarkTextOption)(value);
                Dirty = true;
                RaisePropertyChanged("ChordFinderSelectedBottomMarkTextOptionIndex");
            }
        }

        public ObservableCollection<string> ChordFinderBottomMarkTextOptions
        {
            get
            {
                return GetMarkTextOptions();
            }
        }

        #endregion

        #region ScaleFinder Defaults

        internal ScaleFinderOptions ScaleFinderOptionsBuffer { get; private set; }

        public int ScaleFinderNumFrets
        {
            get
            {
                return ScaleFinderOptionsBuffer.NumFrets;
            }
            set
            {
                try
                {
                    ScaleFinderOptionsBuffer.NumFrets = value;
                    Dirty = true;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("ScaleFinderNumFrets");
                    RaisePropertyChanged("ScaleFinderMaxReach");
                }
            }
        }

        public int ScaleFinderMaxFret
        {
            get
            {
                return ScaleFinderOptionsBuffer.MaxFret;
            }
            set
            {
                try
                {
                    ScaleFinderOptionsBuffer.MaxFret = value;
                    Dirty = true;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("ScaleFinderMaxFret");
                }
            }
        }

        public int ScaleFinderMaxReach
        {
            get
            {
                return ScaleFinderOptionsBuffer.MaxReach;
            }
            set
            {
                try
                {
                    ScaleFinderOptionsBuffer.MaxReach = value;
                    Dirty = true;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("ScaleFinderNumFrets");
                    RaisePropertyChanged("ScaleFinderMaxReach");
                }
            }
        }

        public bool ScaleFinderAllowOpenStrings
        {
            get
            {
                return ScaleFinderOptionsBuffer.AllowOpenStrings;
            }
            set
            {
                ScaleFinderOptionsBuffer.AllowOpenStrings = value;
                Dirty = true;
                RaisePropertyChanged("ScaleFinderAllowOpenStrings");
            }
        }

        public bool ScaleFinderAllowMutedStrings
        {
            get
            {
                return ScaleFinderOptionsBuffer.AllowMutedStrings;
            }
            set
            {
                ScaleFinderOptionsBuffer.AllowMutedStrings = value;
                Dirty = true;
                RaisePropertyChanged("ScaleFinderAllowMutedStrings");
            }
        }

        internal ScaleFinderStyle ScaleFinderStyleBuffer { get; private set; }

        public bool ScaleFinderAddTitle
        {
            get
            {
                return ScaleFinderStyleBuffer.AddTitle;
            }
            set
            {
                ScaleFinderStyleBuffer.AddTitle = value;
                Dirty = true;
                RaisePropertyChanged("ScaleFinderAddTitle");
            }
        }

        public bool ScaleFinderMirrorResults
        {
            get
            {
                return ScaleFinderStyleBuffer.MirrorResults;
            }
            set
            {
                ScaleFinderStyleBuffer.MirrorResults = value;
                Dirty = true;
                RaisePropertyChanged("ScaleFinderMirrorResults");
            }
        }

        public bool ScaleFinderAddRootNotes
        {
            get
            {
                return ScaleFinderStyleBuffer.AddRootNotes;
            }
            set
            {
                ScaleFinderStyleBuffer.AddRootNotes = value;
                Dirty = true;
                RaisePropertyChanged("ScaleFinderAddRootNotes");
            }
        }

        public int ScaleFinderSelectedMarkTextOptionIndex
        {
            get
            {
                return (int)ScaleFinderStyleBuffer.MarkTextOption;
            }
            set
            {
                ScaleFinderStyleBuffer.MarkTextOption = (MarkTextOption)(value);
                Dirty = true;
                RaisePropertyChanged("ScaleFinderSelectedMarkTextOptionIndex");
            }
        }

        public ObservableCollection<string> ScaleFinderMarkTextOptions
        {
            get
            {
                return GetMarkTextOptions();
            }
        }

        #endregion

        public RelayCommand ShowAdvancedSettings
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage("Careful! You can cause problems with Chordious if you don't know what you're doing in here. Do you want to continue?", (confirmed) =>
                        {
                            if (confirmed)
                            {
                                Messenger.Default.Send<ShowAdvancedDataMessage>(new ShowAdvancedDataMessage(SettingsBuffer, "", (itemsChanged) =>
                                {
                                    if (itemsChanged)
                                    {
                                        Dirty = true;
                                        RefreshProperties();
                                    }
                                }));
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ResetUserSettings
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage("This will revert all of your user settings to the defaults. This cannot be undone. Do you want to continue?", (confirmed) =>
                        {
                            if (confirmed)
                            {
                                ClearUserSettings();
                                RefreshProperties();
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ShowAdvancedStyle
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage("Careful! You can cause problems with Chordious if you don't know what you're doing in here. Do you want to continue?", (confirmed) =>
                        {
                            if (confirmed)
                            {
                                Messenger.Default.Send<ShowAdvancedDataMessage>(new ShowAdvancedDataMessage(StyleBuffer, "", (itemsChanged) =>
                                {
                                    if (itemsChanged)
                                    {
                                        Dirty = true;
                                        RefreshProperties();
                                    }
                                }));
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ResetUserStyles
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage("This will revert your base user styles to the defaults. This cannot be undone. Do you want to continue?", (confirmed) =>
                        {
                            if (confirmed)
                            {
                                ClearUserStyles();
                                RefreshProperties();
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ShowInstrumentManager
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowInstrumentManagerMessage>(new ShowInstrumentManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ShowChordQualityManager
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowChordQualityManagerMessage>(new ShowChordQualityManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ShowScaleManager
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowScaleManagerMessage>(new ShowScaleManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand Apply
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        ApplyChanges();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return Dirty;
                });
            }
        }

        public RelayCommand Accept
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        ApplyChangesOnClose = true;
                        if (null != RequestClose)
                        {
                            RequestClose();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand Cancel
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        ApplyChangesOnClose = false;
                        if (null != RequestClose)
                        {
                            RequestClose();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public event Action RequestClose;

        public bool ApplyChangesOnClose
        {
            get
            {
                return _applyChangesOnClose;
            }
            private set
            {
                _applyChangesOnClose = value;
                RaisePropertyChanged("ApplyChangesOnClose");
            }
        }
        private bool _applyChangesOnClose = false;

        public bool Dirty
        {
            get
            {
                return _dirty;
            }
            private set
            {
                _dirty = value;
                RaisePropertyChanged("Dirty");
                RaisePropertyChanged("Title");
                RaisePropertyChanged("Apply");
            }
        }
        private bool _dirty = false;

        public bool ItemsChanged
        {
            get
            {
                return _itemsChanged;
            }
            private set
            {
                _itemsChanged = value;
                RaisePropertyChanged("ItemsChanged");
            }
        }
        private bool _itemsChanged = false;

        internal ChordiousSettings SettingsBuffer { get; private set; }
        internal DiagramStyle StyleBuffer { get; private set; }

        public OptionsViewModel()
        {
            SettingsBuffer = new ChordiousSettings(AppVM.UserConfig.ChordiousSettings, "Options");
            StyleBuffer = new DiagramStyle(AppVM.UserConfig.DiagramStyle, "Options");

            ChordFinderOptionsBuffer = new ChordFinderOptions(AppVM.UserConfig, SettingsBuffer);
            ChordFinderStyleBuffer = new ChordFinderStyle(AppVM.UserConfig, SettingsBuffer, StyleBuffer);

            ScaleFinderOptionsBuffer = new ScaleFinderOptions(AppVM.UserConfig, SettingsBuffer);
            ScaleFinderStyleBuffer = new ScaleFinderStyle(AppVM.UserConfig, SettingsBuffer, StyleBuffer);
        }

        public bool ProcessClose()
        {
            if (ApplyChangesOnClose)
            {
                ApplyChanges();
            }
            return ItemsChanged;
        }

        protected string GetSetting(string key)
        {
            return SettingsBuffer.Get(key);
        }

        protected void SetSetting(string key, object value)
        {
            SettingsBuffer.Set(key, value);
            Dirty = true;
        }

        protected string GetStyle(string key)
        {
            return StyleBuffer.Get(key);
        }

        protected void SetStyle(string key, object value)
        {
            StyleBuffer.Set(key, value);
            Dirty = true;
        }

        private void ApplyChanges()
        {
            SettingsBuffer.SetParent();
            SettingsBuffer.Clear();

            StyleBuffer.SetParent();
            StyleBuffer.Clear();

            Dirty = false;
            ItemsChanged = true;
        }

        private void ClearUserSettings()
        {
            SettingsBuffer.Clear();
            SettingsBuffer.Parent.Clear();

            Dirty = false;
            ItemsChanged = true;
        }

        private void ClearUserStyles()
        {
            StyleBuffer.Clear();
            StyleBuffer.Parent.Clear();

            Dirty = false;
            ItemsChanged = true;
        }

        private static ObservableCollection<string> GetBarreTypeOptions()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("None");
            collection.Add("Partial");
            collection.Add("Full");

            return collection;
        }

        private static ObservableCollection<string> GetMarkTextOptions()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("None");
            collection.Add("Show Both");
            collection.Add("Prefer Flats");
            collection.Add("Prefer Sharps");

            return collection;
        }

        public virtual void RefreshProperties()
        {
            // refresh all properties
        }
    }
}
