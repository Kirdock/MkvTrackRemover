// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MkvTrackRemover.helpers;
using MkvTrackRemover.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Dispatching;
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MkvTrackRemover
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly HashSet<string> SelectedAudioTracks = new();
        private readonly HashSet<string> SelectedSubtitleTracks = new();
        private MkvExecutor MkvExecutor;
        private MessageHelper MessageHelper;
        private readonly DispatcherQueue DispatcherQueue = DispatcherQueue.GetForCurrentThread();

        public MainWindow()
        {
            this.InitializeComponent();
            if (Content is FrameworkElement fe)
            {
                fe.Loaded += (ss, ee) => OnLoaded();
            }
            ConsumeArgs();
        }

        private void ConsumeArgs()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                directoryInput.Text = args[1];
            }
            
        }

        private void OnLoaded()
        {
            MessageHelper = new MessageHelper(DispatcherQueue, Content.XamlRoot);
            MkvExecutor = new MkvExecutor(StartProgressBar, UpdateProgressBar, UpdateOutput, MessageHelper );
            MkvLocator.SetMvkDirectory();
            infoBar.Message = MkvLocator.MkvDirectory;
            InitDefaultValues();
        }

        private void InitDefaultValues()
        {
            CheckBox[] audioCheckboxes = new CheckBox[] { keepGermanAudioCheckbox, keepJapanesenAudioCheckbox };
            CheckBox[] subtitleCheckboxes = new CheckBox[] { keepGermanSubtitleCheckbox};

            foreach(var audioCheckbox in audioCheckboxes)
            {
                keepAudioCheckbox_Checked(audioCheckbox);
            }
            foreach(var subtitleCheckbox in subtitleCheckboxes)
            {
                keepSubtitleCheckbox_Checked(subtitleCheckbox);
            }
        }

        private async void run_Click(object sender, RoutedEventArgs e)
        {
            string directory = directoryInput.Text;
            if(directory == string.Empty)
            {
                directoryInputStatus.Message = "Please enter a directory";
            } else if(!Directory.Exists(directory)) {
                directoryInputStatus.Message = "The specified directory does not exist";
                directoryInputStatus.IsOpen = true;
            }
            await Task.Run(async () => {
                await MkvExecutor.Execute(directory, SelectedAudioTracks, SelectedSubtitleTracks);
            });
        }

        internal void StartProgressBar(int maximum)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                progressBar.Maximum = maximum;
                progressBar.Value = 0;
            });
        }

        internal void UpdateProgressBar()
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                progressBar.Value++;
            });
        }

        internal void UpdateOutput(string message)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                AddOutputLine(message);
            });
        }

        private void AddOutputLine(string message)
        {
            Paragraph paragraph = new();
            Run run = new()
            {
                Text = message
            };

            paragraph.Inlines.Add(run);
            outputTextBox.Blocks.Add(paragraph);

        }

        private void HandleTrack(object sender, HashSet<string> selectedLanguages)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Tag == null)
            {
                return;
            }
            string value = (string)checkBox.Tag;

            if (checkBox.IsChecked.GetValueOrDefault())
            {
                selectedLanguages.Add(value);
            }
            else
            {
                selectedLanguages.Remove(value);
            }
        }

        private void keepAudioCheckbox_Checked(object sender, RoutedEventArgs? e = null)
        {
            HandleTrack(sender, SelectedAudioTracks);
        }

        private void keepSubtitleCheckbox_Checked(object sender, RoutedEventArgs? e = null)
        {
            HandleTrack(sender, SelectedSubtitleTracks);
        }
    }
}
