﻿// 
// ChordFinderViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016 Jon Thysell <http://jonthysell.com>
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
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ChordFinderViewModel : ViewModelBase, IIdle
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
                return Strings.ChordFinderTitle;
            }
        }

        public bool IsIdle
        {
            get
            {
                return _isIdle;
            }
            private set
            {
                _isIdle = value;
                RaisePropertyChanged("IsIdle");
                RaisePropertyChanged("SearchAsync");
                RaisePropertyChanged("CancelSearch");
            }
        }
        private bool _isIdle = true;

        #region Options

        #region Instruments

        public string SelectedInstrumentLabel
        {
            get
            {
                return Strings.FinderSelectedInstrumentLabel;
            }
        }

        public string SelectedInstrumentToolTip
        {
            get
            {
                return Strings.FinderSelectedInstrumentToolTip;
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
                    Tunings = null;

                    if (null != value)
                    {
                        Tunings = SelectedInstrument.GetTunings();
                        if (null != Tunings && Tunings.Count > 0)
                        {
                            SelectedTuning = Tunings[0];
                            Options.SetTarget(SelectedInstrument.Instrument, SelectedTuning.Tuning);
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
                    RaisePropertyChanged("SearchAsync");
                    RaisePropertyChanged("SetAsDefaults");
                }
            }
        }
        private ObservableInstrument _instrument;

        public ObservableCollection<ObservableInstrument> Instruments
        {
            get
            {
                return _instruments;
            }
            private set
            {
                _instruments = value;
                RaisePropertyChanged("Instruments");
            }
        }
        private ObservableCollection<ObservableInstrument> _instruments;

        #endregion

        #region Tunings

        public string SelectedTuningLabel
        {
            get
            {
                return Strings.FinderSelectedTuningLabel;
            }
        }

        public string SelectedTuningToolTip
        {
            get
            {
                return Strings.FinderSelectedTuningToolTip;
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
                    if (null != value)
                    {
                        Options.SetTarget(SelectedInstrument.Instrument, SelectedTuning.Tuning);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("SelectedTuning");
                    RaisePropertyChanged("SearchAsync");
                    RaisePropertyChanged("SetAsDefaults");
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

        #endregion

        public string ShowInstrumentManagerLabel
        {
            get
            {
                return Strings.ShowInstrumentManagerLabel;
            }
        }

        public string ShowInstrumentManagerToolTip
        {
            get
            {
                return Strings.ShowInstrumentManagerToolTip;
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
                        Messenger.Default.Send<ShowInstrumentManagerMessage>(new ShowInstrumentManagerMessage(() =>
                        {
                            RefreshInstruments(null != SelectedInstrument ? SelectedInstrument.Instrument : null, null != SelectedTuning ? SelectedTuning.Tuning : null);
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        #region RootNote

        public string SelectedRootNoteLabel
        {
            get
            {
                return Strings.FinderSelectedRootNoteLabel;
            }
        }

        public string SelectedRootNoteToolTip
        {
            get
            {
                return Strings.FinderSelectedRootNoteToolTip;
            }
        }

        public string SelectedRootNote
        {
            get
            {
                return NoteUtils.ToString(Options.RootNote);
            }
            set
            {
                try
                {
                    Options.RootNote = NoteUtils.ParseNote(value);
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("SelectedRootNote");
                }
            }
        }

        public ObservableCollection<string> RootNotes
        {
            get
            {
                return ObservableEnums.GetNotes();
            }
        }

        #endregion

        #region ChordQuality

        public string SelectedChordQualityLabel
        {
            get
            {
                return Strings.ChordFinderOptionsSelectedChordQualityLabel;
            }
        }

        public string SelectedChordQualityToolTip
        {
            get
            {
                return Strings.ChordFinderOptionsSelectedChordQualityToolTip;
            }
        }

        public ObservableChordQuality SelectedChordQuality
        {
            get
            {
                return _chordQuality;
            }
            set
            {
                try
                {
                    _chordQuality = value;
                    if (null != value)
                    {
                        Options.SetTarget(Options.RootNote, SelectedChordQuality.ChordQuality);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("SelectedChordQuality");
                    RaisePropertyChanged("SearchAsync");
                    RaisePropertyChanged("SetAsDefaults");
                }
            }
        }
        private ObservableChordQuality _chordQuality;

        public ObservableCollection<ObservableChordQuality> ChordQualities
        {
            get
            {
                return _chordQualities;
            }
            private set
            {
                _chordQualities = value;
                RaisePropertyChanged("ChordQualities");
            }
        }
        private ObservableCollection<ObservableChordQuality> _chordQualities;

        public string ShowChordQualityManagerLabel
        {
            get
            {
                return Strings.ShowChordQualityManagerLabel;
            }
        }

        public string ShowChordQualityManagerToolTip
        {
            get
            {
                return Strings.ShowChordQualityManagerToolTip;
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
                        Messenger.Default.Send<ShowChordQualityManagerMessage>(new ShowChordQualityManagerMessage(() =>
                            {
                                RefreshChordQualities(null != SelectedChordQuality ? SelectedChordQuality.ChordQuality : null);
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

        public string NumFretsLabel
        {
            get
            {
                return Strings.FinderOptionsNumFretsLabel;
            }
        }

        public string NumFretsToolTip
        {
            get
            {
                return Strings.FinderOptionsNumFretsToolTip;
            }
        }

        public int NumFrets
        {
            get
            {
                return Options.NumFrets;
            }
            set
            {
                try
                {
                    Options.NumFrets = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("NumFrets");
                    RaisePropertyChanged("MaxReach");
                }
            }
        }

        public string MaxReachLabel
        {
            get
            {
                return Strings.FinderOptionsMaxReachLabel;
            }
        }

        public string MaxReachToolTip
        {
            get
            {
                return Strings.FinderOptionsMaxReachToolTip;
            }
        }

        public int MaxReach
        {
            get
            {
                return Options.MaxReach;
            }
            set
            {
                try
                {
                    Options.MaxReach = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("MaxReach");
                    RaisePropertyChanged("NumFrets");
                }
            }
        }

        public string MaxFretLabel
        {
            get
            {
                return Strings.FinderOptionsMaxFretLabel;
            }
        }

        public string MaxFretToolTip
        {
            get
            {
                return Strings.FinderOptionsMaxFretToolTip;
            }
        }

        public int MaxFret
        {
            get
            {
                return Options.MaxFret;
            }
            set
            {
                try
                {
                    Options.MaxFret = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("MaxFret");
                }
            }
        }

        public string AllowOpenStringsLabel
        {
            get
            {
                return Strings.FinderOptionsAllowOpenStringsLabel;
            }
        }

        public string AllowOpenStringsToolTip
        {
            get
            {
                return Strings.FinderOptionsAllowOpenStringsToolTip;
            }
        }

        public bool AllowOpenStrings
        {
            get
            {
                return Options.AllowOpenStrings;
            }
            set
            {
                Options.AllowOpenStrings = value;
                RaisePropertyChanged("AllowOpenStrings");
            }
        }

        public string AllowMutedStringsLabel
        {
            get
            {
                return Strings.FinderOptionsAllowMutedStringsLabel;
            }
        }

        public string AllowMutedStringsToolTip
        {
            get
            {
                return Strings.FinderOptionsAllowMutedStringsToolTip;
            }
        }

        public bool AllowMutedStrings
        {
            get
            {
                return Options.AllowMutedStrings;
            }
            set
            {
                Options.AllowMutedStrings = value;
                RaisePropertyChanged("AllowMutedStrings");
            }
        }

        public string AllowRootlessChordsLabel
        {
            get
            {
                return Strings.ChordFinderOptionsAllowRootlessChordsLabel;
            }
        }

        public string AllowRootlessChordsToolTip
        {
            get
            {
                return Strings.ChordFinderOptionsAllowRootlessChordsToolTip;
            }
        }

        public bool AllowRootlessChords
        {
            get
            {
                return Options.AllowRootlessChords;
            }
            set
            {
                Options.AllowRootlessChords = value;
                RaisePropertyChanged("AllowRootlessChords");
            }
        }

        #endregion

        #region Styles

        public string AddTitleLabel
        {
            get
            {
                return Strings.FinderOptionsAddTitleLabel;
            }
        }

        public string AddTitleToolTip
        {
            get
            {
                return Strings.FinderOptionsAddTitleToolTip;
            }
        }

        public bool AddTitle
        {
            get
            {
                return Style.AddTitle;
            }
            set
            {
                Style.AddTitle = value;
                RaisePropertyChanged("AddTitle");
            }
        }

        public string MirrorResultsLabel
        {
            get
            {
                return Strings.FinderOptionsMirrorResultsLabel;
            }
        }

        public string MirrorResultsToolTip
        {
            get
            {
                return Strings.FinderOptionsMirrorResultsToolTip;
            }
        }

        public bool MirrorResults
        {
            get
            {
                return Style.MirrorResults;
            }
            set
            {
                Style.MirrorResults = value;
                RaisePropertyChanged("MirrorResults");
            }
        }

        public string SelectedBarreTypeOptionLabel
        {
            get
            {
                return Strings.ChordFinderOptionsBarreTypeLabel;
            }
        }

        public string SelectedBarreTypeOptionToolTip
        {
            get
            {
                return Strings.ChordFinderOptionsBarreTypeToolTip;
            }
        }

        public int SelectedBarreTypeOptionIndex
        {
            get
            {
                return (int)Style.BarreTypeOption;
            }
            set
            {
                Style.BarreTypeOption = (BarreTypeOption)(value);
                RaisePropertyChanged("SelectedBarreTypeOptionIndex");
            }
        }

        public ObservableCollection<string> BarreTypeOptions
        {
            get
            {
                return ObservableEnums.GetBarreTypeOptions();
            }
        }

        public string AddRootNotesLabel
        {
            get
            {
                return Strings.FinderOptionsAddRootNotesLabel;
            }
        }

        public string AddRootNotesToolTip
        {
            get
            {
                return Strings.FinderOptionsAddRootNotesToolTip;
            }
        }

        public bool AddRootNotes
        {
            get
            {
                return Style.AddRootNotes;
            }
            set
            {
                Style.AddRootNotes = value;
                RaisePropertyChanged("AddRootNotes");
            }
        }

        public string SelectedMarkTextOptionLabel
        {
            get
            {
                return Strings.FinderOptionsMarkTextLabel;
            }
        }

        public string SelectedMarkTextOptionToolTip
        {
            get
            {
                return Strings.FinderOptionsMarkTextToolTip;
            }
        }

        public int SelectedMarkTextOptionIndex
        {
            get
            {
                return (int)Style.MarkTextOption;
            }
            set
            {
                Style.MarkTextOption = (MarkTextOption)(value);
                RaisePropertyChanged("SelectedMarkTextOptionIndex");
            }
        }

        public ObservableCollection<string> MarkTextOptions
        {
            get
            {
                return ObservableEnums.GetMarkTextOptions();
            }
        }

        public string AddBottomMarksLabel
        {
            get
            {
                return Strings.ChordFinderOptionsAddBottomMarksLabel;
            }
        }

        public string AddBottomMarksToolTip
        {
            get
            {
                return Strings.ChordFinderOptionsAddBottomMarksToolTip;
            }
        }

        public bool AddBottomMarks
        {
            get
            {
                return Style.AddBottomMarks;
            }
            set
            {
                Style.AddBottomMarks = value;
                RaisePropertyChanged("AddBottomMarks");
            }
        }

        public int SelectedBottomMarkTextOptionIndex
        {
            get
            {
                return (int)Style.BottomMarkTextOption;
            }
            set
            {
                Style.BottomMarkTextOption = (MarkTextOption)(value);
                RaisePropertyChanged("SelectedBottomMarkTextOptionIndex");
            }
        }

        public string SelectedBottomMarkTextOptionLabel
        {
            get
            {
                return Strings.ChordFinderOptionsBottomMarkTextLabel;
            }
        }

        public string SelectedBottomMarkTextOptionToolTip
        {
            get
            {
                return Strings.ChordFinderOptionsBottomMarkTextToolTip;
            }
        }

        public ObservableCollection<string> BottomMarkTextOptions
        {
            get
            {
                return ObservableEnums.GetMarkTextOptions();
            }
        }

        #endregion

        public string SetAsDefaultsLabel
        {
            get
            {
                return Strings.FinderOptionsSetAsDefaultsLabel;
            }
        }

        public string SetAsDefaultsToolTip
        {
            get
            {
                return Strings.FinderOptionsSetAsDefaultsToolTip;
            }
        }

        public RelayCommand SetAsDefaults
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.FinderOptionsSetAsDefaultsPromptMessage, (confirmed) =>
                        {
                            if (confirmed)
                            {
                                Options.Settings.SetParent();
                                Style.Settings.SetParent();
                                RefreshSettings();
                            }
                        }, "confirmation.chordfinder.setasdefaults"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanSearch();
                });
            }
        }

        public string ResetToDefaultsLabel
        {
            get
            {
                return Strings.FinderOptionsResetToDefaultsLabel;
            }
        }

        public string ResetToDefaultsToolTip
        {
            get
            {
                return Strings.FinderOptionsResetToDefaultsToolTip;
            }
        }

        public RelayCommand ResetToDefaults
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.FinderOptionsResetToDefaultsPromptMessage, (confirmed) =>
                        {
                            if (confirmed)
                            {
                                Options.Settings.Clear();
                                Style.Settings.Clear();
                                RefreshSettings();
                            }
                        }, "confirmation.chordfinder.resettodefaults"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public string SearchAsyncLabel
        {
            get
            {
                return Strings.FinderSearchLabel;
            }
        }

        public string SearchAsyncToolTip
        {
            get
            {
                return Strings.FinderSearchToolTip;
            }
        }

        public RelayCommand SearchAsync
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    _searchAsyncCancellationTokenSource = new CancellationTokenSource();

                    try
                    {
                        IsIdle = false;

                        Results.Clear();
                        SelectedResults.Clear();

                        ChordFinderResultSet results = await FindChordsAsync(_searchAsyncCancellationTokenSource.Token);

                        if (null != results)
                        {
                            if (results.Count == 0 && !_searchAsyncCancellationTokenSource.IsCancellationRequested)
                            {
                                Messenger.Default.Send<ChordiousMessage>(new ChordiousMessage(Strings.ChordFinderNoResultsMessage));
                            }
                            else
                            {
                                for (int i = 0; i < results.Count; i++)
                                {
                                    if (_searchAsyncCancellationTokenSource.IsCancellationRequested)
                                    {
                                        break;
                                    }
                                    Results.Add(await RenderChordAsync(results.ResultAt(i)));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                    finally
                    {
                        _searchAsyncCancellationTokenSource = null;
                        IsIdle = true;
                    }
                }, () =>
                {
                    return CanSearch();
                });
            }
        }

        private CancellationTokenSource _searchAsyncCancellationTokenSource;

        public RelayCommand CancelSearch
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        if (null != _searchAsyncCancellationTokenSource)
                        {
                            _searchAsyncCancellationTokenSource.Cancel();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return (null != _searchAsyncCancellationTokenSource);
                });
            }
        }

        public string SaveSelectedLabel
        {
            get
            {
                return Strings.FinderSaveSelectedLabel;
            }
        }

        public string SaveSelectedToolTip
        {
            get
            {
                return Strings.FinderSaveSelectedToolTip;
            }
        }

        public RelayCommand SaveSelected
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowDiagramCollectionSelectorMessage>(new ShowDiagramCollectionSelectorMessage((name, newCollection) =>
                        {
                            DiagramLibrary library = AppVM.UserConfig.DiagramLibrary;
                            DiagramCollection targetCollection = library.Get(name);

                            foreach (ObservableDiagram od in SelectedResults)
                            {
                                targetCollection.Add(od.Diagram);
                            }

                            LastDiagramCollectionName = name.Trim();
                        }, LastDiagramCollectionName));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedResults.Count > 0;
                });
            }
        }

        private static string LastDiagramCollectionName = "";

        public ObservableCollection<ObservableDiagram> SelectedResults
        {
            get
            {
                return _selectedResults;
            }
            private set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }

                _selectedResults = value;
                RaisePropertyChanged("SelectedResults");
            }
        }
        private ObservableCollection<ObservableDiagram> _selectedResults;

        private void SelectedResults_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("SaveSelected");
        }

        public ObservableCollection<ObservableDiagram> Results
        {
            get
            {
                return _results;
            }
            private set
            {
                _results = value;
                RaisePropertyChanged("Results");
            }
        }
        public ObservableCollection<ObservableDiagram> _results;

        internal ChordFinderOptions Options { get; private set; }
        internal ChordFinderStyle Style { get; private set; }

        public ChordFinderViewModel()
        {
            Options = new ChordFinderOptions(AppVM.UserConfig);
            Style = new ChordFinderStyle(AppVM.UserConfig);

            RefreshInstruments(Options.Instrument, Options.Tuning);

            RefreshChordQualities(Options.ChordQuality);

            Results = new ObservableCollection<ObservableDiagram>();
            SelectedResults = new ObservableCollection<ObservableDiagram>();

            SelectedResults.CollectionChanged += SelectedResults_CollectionChanged;
        }

        private void RefreshInstruments(IInstrument selectedInstrument = null, ITuning selectedTuning = null)
        {
            Instruments = AppVM.GetInstruments();
            SelectedInstrument = null;

            if (null != selectedInstrument && null != Instruments)
            {
                foreach (ObservableInstrument oi in Instruments)
                {
                    if (oi.Instrument == selectedInstrument)
                    {
                        SelectedInstrument = oi;
                        break;
                    }
                }
            }

            if (null != SelectedInstrument)
            {
                Tunings = SelectedInstrument.GetTunings();
                SelectedTuning = null;

                if (null != selectedTuning && null != Tunings)
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

        private void RefreshChordQualities(IChordQuality selectedChordQuality = null)
        {
            ChordQualities = AppVM.GetChordQualities();
            SelectedChordQuality = null;

            if (null != selectedChordQuality && null != ChordQualities)
            {
                foreach (ObservableChordQuality ocq in ChordQualities)
                {
                    if (ocq.ChordQuality == selectedChordQuality)
                    {
                        SelectedChordQuality = ocq;
                        break;
                    }
                }
            }
        }

        private void RefreshSettings()
        {
            RefreshInstruments(Options.Instrument, Options.Tuning);
            RefreshChordQualities(Options.ChordQuality);

            RaisePropertyChanged("SelectedRootNote");
            RaisePropertyChanged("NumFrets");
            RaisePropertyChanged("MaxReach");
            RaisePropertyChanged("MaxFret");
            RaisePropertyChanged("AllowOpenStrings");
            RaisePropertyChanged("AllowMutedStrings");
            RaisePropertyChanged("AllowRootlessChords");
            RaisePropertyChanged("AddTitle");
            RaisePropertyChanged("MirrorResults");
            RaisePropertyChanged("SelectedBarreTypeOptionIndex");
            RaisePropertyChanged("AddRootNotes");
            RaisePropertyChanged("SelectedMarkTextOptionIndex");
            RaisePropertyChanged("AddBottomMarks");
            RaisePropertyChanged("SelectedBottomMarkTextOptionIndex");
        }

        private Task<ChordFinderResultSet> FindChordsAsync(CancellationToken cancelToken)
        {
            return Task<ChordFinderResultSet>.Factory.StartNew(() =>
            {
                ChordFinderResultSet results = null;

                try
                {
                    Task<ChordFinderResultSet> task = ChordFinder.FindChordsAsync(Options, cancelToken);
                    task.Wait();

                    results = task.Result;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                
                return results;
            });
        }

        private Task<ObservableDiagram> RenderChordAsync(IChordFinderResult result)
        {            
            return Task<ObservableDiagram>.Factory.StartNew(() =>
            {
                ObservableDiagram od = null;
                AppVM.DoOnUIThread(() =>
                    {
                        try
                        {
                            od = new ObservableDiagram(result.ToDiagram(Style));
                            od.PostEditCallback = (changed) =>
                            {
                                if (changed)
                                {
                                    od.Refresh();
                                }
                            };
                        }
                        catch (Exception ex)
                        {
                            ExceptionUtils.HandleException(ex);
                        }
                    });
                return od;
            });
        }

        private bool CanSearch()
        {
            return IsIdle && (null != SelectedInstrument) && (null != SelectedTuning) && (null != SelectedChordQuality);
        }
    }
}
