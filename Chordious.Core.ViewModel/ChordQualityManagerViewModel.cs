﻿// 
// ChordQualityManagerViewModel.cs
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
using System.Collections.Generic;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ChordQualityManagerViewModel : NamedIntervalManagerViewModel
    {
        public override string Title
        {
            get
            {
                return "Chord Qualities";
            }
        }

        public override RelayCommand AddNamedInterval
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowChordQualityEditorMessage>(new ShowChordQualityEditorMessage(true, (name, abbreviation, intervals) =>
                        {
                            try
                            {
                                NamedInterval ni = null;

                                AppVM.UserConfig.ChordQualities.Add(name, abbreviation, intervals);

                                Refresh(ni);
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
                });
            }
        }

        public override RelayCommand EditNamedInterval
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowChordQualityEditorMessage>(new ShowChordQualityEditorMessage(false, (name, abbreviation, intervals) =>
                        {
                            try
                            {
                                SelectedNamedInterval.NamedInterval.Name = name;
                                ((ChordQuality)(SelectedNamedInterval.NamedInterval)).Abbreviation = abbreviation;
                                SelectedNamedInterval.NamedInterval.Intervals = intervals;
                                Refresh(SelectedNamedInterval.NamedInterval);
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, SelectedNamedInterval.Name, ((ChordQuality)(SelectedNamedInterval.NamedInterval)).Abbreviation, SelectedNamedInterval.Intervals));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return NamedIntervalIsSelected && SelectedNamedInterval.CanEdit;
                });
            }
        }

        public ChordQualityManagerViewModel() : base(AppViewModel.Instance.GetChordQualities, AppViewModel.Instance.UserConfig.ChordQualities.Remove) { }
    }
}
