﻿// 
// InstrumentManagerViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
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

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class InstrumentManagerViewModel : ViewModelBase
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
                return Strings.InstrumentManagerTitle;
            }
        }

        public bool InstrumentIsSelected
        {
            get
            {
                return (null != SelectedInstrument);
            }
        }

        public ObservableInstrument SelectedInstrument
        {
            get
            {
                return _instrument;
            }
            set
            {
                try
                {
                    _instrument = value;
                    SelectedTuning = null;

                    if (null == SelectedInstrument)
                    {
                        Tunings = null;
                    }
                    else
                    {
                        Tunings = SelectedInstrument.GetTunings();
                        if (Tunings.Count > 0)
                        {
                            SelectedTuning = Tunings[0];
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("SelectedInstrument");
                    RaisePropertyChanged("InstrumentIsSelected");
                    RaisePropertyChanged("EditInstrument");
                    RaisePropertyChanged("EditInstrumentLabel");
                    RaisePropertyChanged("DeleteInstrument");
                    RaisePropertyChanged("DeleteInstrumentLabel");
                    RaisePropertyChanged("AddTuning");
                }
            }
        }
        private ObservableInstrument _instrument;

        public string DefaultInstrumentsGroupLabel
        {
            get
            {
                return Strings.InstrumentManagerDefaultInstrumentsGroupLabel;
            }
        }

        public string DefaultInstrumentsGroupToolTip
        {
            get
            {
                return Strings.InstrumentManagerDefaultInstrumentsGroupToolTip;
            }
        }

        public int SelectedDefaultInstrumentIndex
        {
            get
            {
                return _selectedDefaultInstrumentIndex;
            }
            set
            {
                if (value < 0 || value >= DefaultInstruments.Count)
                {
                    _selectedDefaultInstrumentIndex = -1;
                }
                else
                {
                    _selectedDefaultInstrumentIndex = value;
                    SelectedInstrument = DefaultInstruments[_selectedDefaultInstrumentIndex];
                    SelectedUserInstrumentIndex = -1; // Unselect user instrument
                }
                RaisePropertyChanged("SelectedDefaultInstrumentIndex");
            }
        }
        private int _selectedDefaultInstrumentIndex = -1;

        public ObservableCollection<ObservableInstrument> DefaultInstruments
        {
            get
            {
                return _defaultInstruments;
            }
            private set
            {
                _defaultInstruments = value;
                RaisePropertyChanged("DefaultInstruments");
            }
        }
        private ObservableCollection<ObservableInstrument> _defaultInstruments;

        public string UserInstrumentsGroupLabel
        {
            get
            {
                return Strings.InstrumentManagerUserInstrumentsGroupLabel;
            }
        }

        public string UserInstrumentsGroupToolTip
        {
            get
            {
                return Strings.InstrumentManagerUserInstrumentsGroupToolTip;
            }
        }

        public int SelectedUserInstrumentIndex
        {
            get
            {
                return _selectedUserInstrumentIndex;
            }
            set
            {
                if (value < 0 || value >= UserInstruments.Count)
                {
                    _selectedUserInstrumentIndex = -1;
                }
                else
                {
                    _selectedUserInstrumentIndex = value;
                    SelectedInstrument = UserInstruments[_selectedUserInstrumentIndex];
                    SelectedDefaultInstrumentIndex = -1; // Unselect default instrument
                }
                RaisePropertyChanged("SelectedUserInstrumentIndex");
            }
        }
        private int _selectedUserInstrumentIndex = -1;

        public ObservableCollection<ObservableInstrument> UserInstruments
        {
            get
            {
                return _userInstruments;
            }
            private set
            {
                _userInstruments = value;
                RaisePropertyChanged("UserInstruments");
            }
        }
        private ObservableCollection<ObservableInstrument> _userInstruments;

        public string TuningsGroupLabel
        {
            get
            {
                return Strings.InstrumentManagerTuningsGroupLabel;
            }
        }

        public string TuningsGroupToolTip
        {
            get
            {
                return Strings.InstrumentManagerTuningsGroupToolTip;
            }
        }

        public bool TuningIsSelected
        {
            get
            {
                return (null != SelectedTuning);
            }
        }

        public ObservableTuning SelectedTuning
        {
            get
            {
                return _tuning;
            }
            set
            {
                try
                {
                    _tuning = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("SelectedTuning");
                    RaisePropertyChanged("TuningIsSelected");
                    RaisePropertyChanged("EditTuning");
                    RaisePropertyChanged("EditTuningLabel");
                    RaisePropertyChanged("DeleteTuning");
                    RaisePropertyChanged("DeleteTuningLabel");
                }
            }
        }
        private ObservableTuning _tuning;

        public ObservableCollection<ObservableTuning> Tunings
        {
            get
            {
                return _tunings;
            }
            private set
            {
                _tunings = value;
                RaisePropertyChanged("Tunings");
            }
        }
        private ObservableCollection<ObservableTuning> _tunings;

        #region AddInstrument

        public string AddInstrumentLabel
        {
            get
            {
                return Strings.NewLabel;
            }
        }

        public string AddInstrumentToolTip
        {
            get
            {
                return Strings.InstrumentManagerAddInstrumentToolTip;
            }
        }

        public RelayCommand AddInstrument
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowInstrumentEditorMessage>(new ShowInstrumentEditorMessage(true, (name, numStrings) =>
                        {
                            Instrument addedInstrument = AppVM.UserConfig.Instruments.Add(name, numStrings);
                            Refresh(addedInstrument);
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        #endregion

        #region EditInstrument

        public string EditInstrumentLabel
        {
            get
            {
                if (InstrumentIsSelected)
                {
                    return String.Format(Strings.InstrumentManagerEditInstrumentLabelFormat, SelectedInstrument.Name);
                }

                return Strings.EditLabel;
            }
        }

        public string EditInstrumentToolTip
        {
            get
            {
                return Strings.InstrumentManagerEditInstrumentToolTip;
            }
        }

        public RelayCommand EditInstrument
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowInstrumentEditorMessage>(new ShowInstrumentEditorMessage(false, (name, numStrings) =>
                        {
                            SelectedInstrument.Instrument.Name = name;
                            Refresh(SelectedInstrument.Instrument);
                        }, SelectedInstrument.Name, SelectedInstrument.NumStrings));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return InstrumentIsSelected && SelectedInstrument.CanEdit;
                });
            }
        }

        #endregion

        #region DeleteInstrument

        public string DeleteInstrumentLabel
        {
            get
            {
                if (InstrumentIsSelected)
                {
                    return String.Format(Strings.InstrumentManagerDeleteInstrumentLabelFormat, SelectedInstrument.Name);
                }

                return Strings.DeleteLabel;
            }
        }

        public string DeleteInstrumentToolTip
        {
            get
            {
                return Strings.InstrumentManagerDeleteInstrumentToolTip;
            }
        }

        public RelayCommand DeleteInstrument
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(String.Format(Strings.InstrumentManagerDeleteInstrumentPromptFormat, SelectedInstrument.Name), (confirm) =>
                        {
                            try
                            {
                                if (confirm)
                                {
                                    AppVM.UserConfig.Instruments.Remove(SelectedInstrument.Name);
                                    Refresh();
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, "confirmation.instrumentmanager.deleteinstrument"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return InstrumentIsSelected && SelectedInstrument.CanEdit;
                });
            }
        }

        #endregion

        #region AddTuning

        public string AddTuningLabel
        {
            get
            {
                return Strings.NewLabel;
            }
        }

        public string AddTuningToolTip
        {
            get
            {
                return Strings.InstrumentManagerAddTuningToolTip;
            }
        }

        public RelayCommand AddTuning
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowTuningEditorMessage>(new ShowTuningEditorMessage(SelectedInstrument, (accepted) =>
                        {
                            if (accepted)
                            {
                                Refresh(SelectedInstrument.Instrument);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return InstrumentIsSelected && SelectedInstrument.CanEdit;
                });
            }
        }

        #endregion

        #region EditTuning

        public string EditTuningLabel
        {
            get
            {
                if (TuningIsSelected)
                {
                    return String.Format(Strings.InstrumentManagerEditTuningLabelFormat, SelectedTuning.Name);
                }

                return Strings.EditLabel;
            }
        }

        public string EditTuningToolTip
        {
            get
            {
                return Strings.InstrumentManagerEditTuningToolTip;
            }
        }

        public RelayCommand EditTuning
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowTuningEditorMessage>(new ShowTuningEditorMessage(SelectedTuning, (accepted) =>
                        {
                            if (accepted)
                            {
                                Refresh(SelectedInstrument.Instrument, SelectedTuning.Tuning);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return InstrumentIsSelected && SelectedInstrument.CanEdit && TuningIsSelected && SelectedTuning.CanEdit;
                });
            }
        }

        #endregion

        #region DeleteTuning

        public string DeleteTuningLabel
        {
            get
            {
                if (TuningIsSelected)
                {
                    return String.Format(Strings.InstrumentManagerDeleteTuningLabelFormat, SelectedTuning.Name);
                }

                return Strings.DeleteLabel;
            }
        }

        public string DeleteTuningToolTip
        {
            get
            {
                return Strings.InstrumentManagerDeleteTuningToolTip;
            }
        }

        public RelayCommand DeleteTuning
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(String.Format(Strings.InstrumentManagerDeleteTuningPromptFormat, SelectedTuning.Name), (confirm) =>
                        {
                            try
                            {
                                if (confirm)
                                {
                                    SelectedInstrument.Instrument.Tunings.Remove(SelectedTuning.Tuning);
                                    Refresh(SelectedInstrument.Instrument);
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, "confirmation.instrumentmanager.deletetuning"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return InstrumentIsSelected && SelectedInstrument.CanEdit && TuningIsSelected && SelectedTuning.CanEdit;
                });
            }
        }

        #endregion

        public InstrumentManagerViewModel()
        {
            Refresh();
        }

        internal void Refresh(Instrument selectedInstrument = null, Tuning selectedTuning = null)
        {
            DefaultInstruments = AppVM.GetDefaultInstruments();
            UserInstruments = AppVM.GetUserInstruments();

            if (null == selectedInstrument)
            {
                SelectedInstrument = null;
            }
            else
            {
                foreach (ObservableInstrument oi in UserInstruments)
                {
                    if (oi.Instrument == selectedInstrument)
                    {
                        SelectedInstrument = oi;
                        break;
                    }
                }

                if (null != SelectedInstrument)
                {
                    foreach (ObservableInstrument oi in DefaultInstruments)
                    {
                        if (oi.Instrument == selectedInstrument)
                        {
                            SelectedInstrument = oi;
                            break;
                        }
                    }
                }

                if (null != SelectedInstrument && null != selectedTuning)
                {
                    foreach (ObservableTuning ot in Tunings)
                    {
                        if (ot.Tuning == selectedTuning)
                        {
                            SelectedTuning = ot;
                            break;
                        }
                    }
                }
            }
        }
    }
}
