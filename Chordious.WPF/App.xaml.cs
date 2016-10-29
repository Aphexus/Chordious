﻿// 
// App.xaml.cs
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
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel;
using com.jonthysell.Chordious.WPF.Resources;

namespace com.jonthysell.Chordious.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public string UserConfigPath { get; private set; }

        public App(string userConfigPath = null)
        {
            MessageHandlers.RegisterMessageHandlers(this);

            UserConfigPath = !String.IsNullOrWhiteSpace(userConfigPath) ? userConfigPath : GetDefaultUserConfigPath();

            string defaultFile = GetDefaultConfigPath();
            string appFile = GetAppConfigPath();
            string userFile = UserConfigPath;

            AppViewModel.Init(Assembly.GetEntryAssembly(), () =>
            {
                return new FileStream(defaultFile, FileMode.Open, FileAccess.Read);
            }, () =>
            {
                return new FileStream(appFile, FileMode.Open, FileAccess.Read);
            }, () =>
            {
                return new FileStream(userFile, FileMode.Open);
            }, () =>
            {
                return new FileStream(userFile, FileMode.Create);
            },
            ImageUtils.SvgTextToBitmapImage, (action) =>
                {
                    Dispatcher.Invoke(action);
                }
            , this.GetFonts
            , userFile);

            if (File.Exists(defaultFile))
            {
                AppVM.LoadDefaultConfig();
            }

            if (File.Exists(appFile))
            {
                AppVM.LoadAppConfig();
            }

            if (!File.Exists(userFile))
            {
                // Makes sure that LoadUserConfig will be successful
                AppVM.SaveUserConfig();
            }

            AppVM.LoadUserConfig(HandleUserConfigLoadException);

            // Makes sure textboxes accept localized inputs
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            Exit += App_Exit;
        }

        public string GetDefaultConfigPath()
        {
            return "Chordious.Core.xml";
        }

        public string GetAppConfigPath()
        {
            return "Chordious.WPF.xml";
        }

        public string GetDefaultUserConfigPath()
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            userFolder = Path.Combine(userFolder, "Chordious");

            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            string defaultConfigPath = Path.Combine(userFolder, DefaultUserConfigFileName);

            if (!File.Exists(defaultConfigPath))
            {
                // Migrate old config if necessary
                string oldConfigPath = Path.Combine(userFolder, "Chordious.WPF.xml");
                if (File.Exists(oldConfigPath))
                {
                    File.Move(oldConfigPath, defaultConfigPath);
                }
            }

            return defaultConfigPath;
        }

        private const string DefaultUserConfigFileName = "Chordious.User.xml";

        public IEnumerable<string> GetFonts()
        {
            foreach (System.Drawing.FontFamily font in System.Drawing.FontFamily.Families)
            {
                yield return font.Name;
            }
        }

        public void HandleUserConfigLoadException(Exception ex)
        {
            string userFile = UserConfigPath;
            Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.ResetAndBackupUserConfigConfirmationMessage, (confirmed) =>
            {
                if (!confirmed) // No, exit app
                {
                    Exit -= App_Exit; // Remove the handler so we don't overwrite the corruupt config with whatever we may have loaded 
                    Shutdown(1);
                }
                else // Yes, backup and continue
                {
                    string backupFile = ResetAndBackupUserConfig(userFile);
                    string message = String.Format(Strings.ResetAndBackupUserConfigBackupFileMessageFormat, backupFile);
                    ExceptionUtils.HandleException(new Exception(message, ex));
                }
            }));
        }

        private string ResetAndBackupUserConfig(string userFile)
        {
            if (String.IsNullOrWhiteSpace(userFile))
            {
                throw new ArgumentNullException("userFile");
            }

            string userFolder = Path.GetDirectoryName(userFile);
            string userFileName = Path.GetFileNameWithoutExtension(userFile);
            string userFileExt = Path.GetExtension(userFile);

            string backupFile = Path.Combine(userFolder, String.Format("{0}.{1:yyyy.MM.dd.HH.mm.ss}{2}", userFileName, DateTime.UtcNow, userFileExt));

            File.Move(userFile, backupFile);

            AppVM.ResetUserConfig();
            AppVM.SaveUserConfig();

            return backupFile;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                AppVM.Close();
                MessageHandlers.UnregisterMessageHandlers(this);
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyHandleMouseButton), true);
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewKeyUpEvent, new RoutedEventHandler(EnterUpdatesSource), true);

            base.OnStartup(e);
        }

        private static void SelectivelyHandleMouseButton(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = (sender as TextBox);
            if (null != textBox && !textBox.IsKeyboardFocusWithin)
            {
                if (e.OriginalSource.GetType().Name == "TextBoxView")
                {
                    e.Handled = true;
                    textBox.Focus();
                }
            }
        }

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (e.OriginalSource as TextBox);
            if (null != textBox)
            {
                textBox.SelectAll();
            }
        }

        private static void EnterUpdatesSource(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (e.OriginalSource as TextBox);
            if (null != textBox && !textBox.IsReadOnly)
            {
                KeyEventArgs args = e as KeyEventArgs;
                if (null != args && args.Key == Key.Enter)
                {
                    textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                }
            }
        }
    }
}
