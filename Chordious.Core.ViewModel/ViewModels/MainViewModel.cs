﻿// 
// MainViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019 Jon Thysell <http://jonthysell.com>
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
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class MainViewModel : ViewModelBase
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
                return AppInfo.Product;
            }
        }

        public string FullProgramName
        {
            get
            {
                return AppInfo.Product + " " + AppInfo.Version;
            }
        }

        public string Description
        {
            get
            {
                string[] descLines = { AppInfo.Comments,
                                   AppInfo.Copyright
                                   };

                return string.Join(Environment.NewLine, descLines);
            }
        }

        #region LaunchWebsite

        public string LaunchWebsiteLabel
        {
            get
            {
                return Strings.MainLaunchWebsiteLabel;
            }
        }

        public string LaunchWebsiteToolTip
        {
            get
            {
                return Strings.MainLaunchWebsiteToolTip;
            }
        }

        public RelayCommand LaunchWebsite
        {
            get
            {
                return _launchWebsite ?? (_launchWebsite = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ConfirmationMessage(Strings.MainLaunchWebsitePromptMessage, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    Messenger.Default.Send(new LaunchUrlMessage(AppInfo.Website));
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, "confirmation.main.launchwebsite"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _launchWebsite;

        #endregion

        #region ShowLicense

        public string ShowLicenseLabel
        {
            get
            {
                return Strings.MainShowLicenseLabel;
            }
        }

        public string ShowLicenseToolTip
        {
            get
            {
                return Strings.MainShowLicenseToolTip;
            }
        }

        public RelayCommand ShowLicense
        {
            get
            {
                return _showLicense ?? (_showLicense = new RelayCommand(() =>
                {
                    try
                    {
                        string text = string.Join(Environment.NewLine, AppInfo.Product + " " + AppInfo.Copyright, "", AppInfo.License);
                        Messenger.Default.Send(new ChordiousMessage(text, Strings.LicenseTitle));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showLicense;

        #endregion

        #region ShowChordFinder

        public string ShowChordFinderLabel
        {
            get
            {
                return Strings.MainShowChordFinderLabel;
            }
        }

        public string ShowChordFinderToolTip
        {
            get
            {
                return Strings.MainShowChordFinderToolTip;
            }
        }

        public RelayCommand ShowChordFinder
        {
            get
            {
                return _showChordFinder ?? (_showChordFinder = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowChordFinderMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showChordFinder;

        #endregion

        #region ShowScaleFinder

        public string ShowScaleFinderLabel
        {
            get
            {
                return Strings.MainShowScaleFinderLabel;
            }
        }

        public string ShowScaleFinderToolTip
        {
            get
            {
                return Strings.MainShowScaleFinderToolTip;
            }
        }

        public RelayCommand ShowScaleFinder
        {
            get
            {
                return _showScaleFinder ?? (_showScaleFinder = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowScaleFinderMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showScaleFinder;

        #endregion

        #region Show Diagram Library

        public string ShowDiagramLibraryLabel
        {
            get
            {
                return Strings.MainShowDiagramLibraryLabel;
            }
        }

        public string ShowDiagramLibraryToolTip
        {
            get
            {
                return Strings.MainShowDiagramLibraryToolTip;
            }
        }

        public RelayCommand ShowDiagramLibrary
        {
            get
            {
                return _showDiagramLibrary ?? (_showDiagramLibrary = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowDiagramLibraryMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showDiagramLibrary;

        #endregion

        #region Show Instrument Manager

        public string ShowInstrumentManagerLabel
        {
            get
            {
                return Strings.MainShowInstrumentManagerLabel;
            }
        }

        public string ShowInstrumentManagerToolTip
        {
            get
            {
                return Strings.MainShowInstrumentManagerToolTip;
            }
        }

        public RelayCommand ShowInstrumentManager
        {
            get
            {
                return _showInstrumentManager ?? (_showInstrumentManager = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowInstrumentManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showInstrumentManager;

        #endregion

        #region Show Options

        public string ShowOptionsLabel
        {
            get
            {
                return Strings.MainShowOptionsLabel;
            }
        }

        public string ShowOptionsToolTip
        {
            get
            {
                return Strings.MainShowOptionsToolTip;
            }
        }

        public RelayCommand ShowOptions
        {
            get
            {
                return _showOptions ?? (_showOptions = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowOptionsMessage((itemsChanged) =>
                        {
                            try
                            {
                                if (itemsChanged)
                                {
                                    AppVM.SaveUserConfig();
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showOptions;

        #endregion

        #region Show Help

        public string ShowHelpLabel
        {
            get
            {
                return Strings.MainShowHelpLabel;
            }
        }

        public string ShowHelpToolTip
        {
            get
            {
                return Strings.MainShowHelpToolTip;
            }
        }

        public RelayCommand ShowHelp
        {
            get
            {
                return _showHelp ?? (_showHelp = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ConfirmationMessage(Strings.MainShowHelpPromptMessage, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    Messenger.Default.Send(new LaunchUrlMessage("http://chordious.com/help/"));
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, "confirmation.main.showhelp"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showHelp;

        #endregion

        public MainViewModel()
        {
        }

    }
}
