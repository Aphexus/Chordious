﻿// 
// ObservableDiagram.cs
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

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ObservableDiagram : ObservableObject
    {
        public string SvgText
        {
            get
            {
                return Diagram.ToImageMarkup(ImageMarkupType.SVG);
            }
        }

        public object ImageObject
        {
            get
            {
                return _imageObject;
            }
            private set
            {
                _imageObject = value;
                RaisePropertyChanged("ImageObject");
            }
        }
        private object _imageObject;

        public int Height
        {
            get
            {
                return (int)Diagram.TotalHeight();
            }
        }
        
        public int Width
        {
            get
            {
                return (int)Diagram.TotalWidth();
            }
        }

        internal Diagram Diagram
        {
            get
            {
                return _diagram;
            }
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                _diagram = value;
                RaisePropertyChanged("SvgText");
                RaisePropertyChanged("Height");
                RaisePropertyChanged("Width");
                ImageObject = AppViewModel.Instance.SvgTextToImage(SvgText);
            }
        }
        private Diagram _diagram;

        public ObservableDiagram(Diagram diagram) : base()
        {
            if (null == diagram)
            {
                throw new ArgumentNullException("diagram");
            }

            Diagram = diagram;
        }
    }
}
