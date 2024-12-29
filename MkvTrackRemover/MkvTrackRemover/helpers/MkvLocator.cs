using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace MkvTrackRemover.helpers
{
    internal class MkvLocator
    {
        private static readonly string? DefaultMkvMergeLocation = GetDefaultMkvPropedit();
        private static readonly string MkvDirectorySetting = "MkvDirectory";
        internal static string MkvDirectory = string.Empty;
        internal static string MkvMergePath => Path.Combine(MkvDirectory, "mkvmerge.exe");
        internal static string MkvInfoPath => Path.Combine(MkvDirectory, "mkvinfo.exe");

        private static string? GetDefaultMkvPropedit()
        {
            RegistryKey? key = Registry.LocalMachine.OpenSubKey(@"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\MKVtoolnix") ?? Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\MKVtoolnix");

            if (key == null)
            {
                return null;
            }

            string? value = key.GetValue("UninstallString")?.ToString();
            if(value == null)
            {
                return null;
            }

            string? directory = Directory.GetParent(value)?.FullName;
            if(directory == null)
            {
                return null;
            }

            string mergePath = Path.Combine(directory, "mkvmerge.exe");
            string infoPath  = Path.Combine(directory, "mkvinfo.exe");
            return File.Exists(mergePath) && File.Exists(infoPath) ? directory : null;
        }

        internal static async void SetMvkDirectory()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string? currentDirectory = localSettings.Values[MkvDirectorySetting] as string;
            if(currentDirectory == null)
            {
                currentDirectory = DefaultMkvMergeLocation ?? await GetMkvDirectory();
                localSettings.Values[MkvDirectorySetting] = currentDirectory;
            }

            MkvDirectory = currentDirectory;
            
        }

        private async static Task<string> GetMkvDirectory()
        {
            TextBox directoryTextBox = new();
            ContentDialog dialog = new()
            {
                Content = directoryTextBox,
                Title = "Enter MKV merge and info directory",
                PrimaryButtonText = "Ok",
            };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return directoryTextBox.Text;
            }
            else
            {
                Application.Current.Exit();
                return string.Empty;
            }
        }
    }
}
