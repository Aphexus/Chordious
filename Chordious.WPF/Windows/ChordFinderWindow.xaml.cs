﻿// 
// ChordFinderWindow.xaml.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2017, 2019 Jon Thysell <http://jonthysell.com>
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

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using com.jonthysell.Chordious.Core.ViewModel;

namespace com.jonthysell.Chordious.WPF
{
    /// <summary>
    /// Interaction logic for ChordFinderWindow.xaml
    /// </summary>
    public partial class ChordFinderWindow : Window
    {
        public ChordFinderViewModel VM
        {
            get
            {
                return DataContext as ChordFinderViewModel;
            }
            private set
            {
                DataContext = value;
            }
        }

        public ChordFinderWindow(ChordFinderViewModel vm)
        {
            VM = vm;

            InitializeComponent();

            // Handle the lack of binding selected listview items in WPF
            ResultsListView.SelectionChanged += ResultsListView_SelectionChanged;

            Closing += ChordFinderWindow_Closing;
        }

        private void ChordFinderWindow_Closing(object sender, CancelEventArgs e)
        {
            VM.CancelSearch.Execute(null);
            e.Cancel = false;
        }

        void ResultsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (object item in e.AddedItems)
            {
                ObservableDiagram od = item as ObservableDiagram;
                VM.SelectedResults.Add(od);
            }

            foreach (object item in e.RemovedItems)
            {
                ObservableDiagram od = item as ObservableDiagram;
                VM.SelectedResults.Remove(od);
            }
        }

        private void DiagramImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Image image && image.DataContext is ObservableDiagram od && e.LeftButton == MouseButtonState.Pressed)
            {
                IntegrationUtils.DiagramToDragDrop(image, od);
            }
        }

        private void DiagramImage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is ObservableDiagram od)
            {
                if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed && e.ClickCount == 1 && VM.SelectedResults.Contains(od) && image != _lastClicked)
                {
                    _lastClicked = image;
                    e.Handled = true;
                }
                else
                {
                    _lastClicked = null;
                }
            }
        }

        private Image _lastClicked = null;
    }
}
