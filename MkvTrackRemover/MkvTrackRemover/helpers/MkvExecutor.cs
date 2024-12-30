using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MkvTrackRemover.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MkvTrackRemover.helpers
{
    internal class MkvExecutor
    {
        private readonly Action<int> StartProgressBar;
        private readonly Action UpdateProgressBar;
        private readonly Action<string> UpdateOutput;
        private readonly MessageHelper MessageHelper;

        public MkvExecutor(Action<int> startProgressBar, Action updateProgressBar, Action<string> updateOutput, MessageHelper messageHelper)
        {
            UpdateProgressBar = updateProgressBar;
            StartProgressBar = startProgressBar;
            MessageHelper = messageHelper;
            UpdateOutput = updateOutput;
        }

        internal async Task Execute(string directory, HashSet<string> audioConfig, HashSet<string> subtitleConfig)
        {
            try
            {
                string[] fileNames = Directory.GetFiles(directory, "*.mkv");
                StartProgressBar(fileNames.Length);
                bool skipDialog = false;
                foreach (string fileName in fileNames)
                {
                    string output = await ExecuteCommand($"-J \"{fileName}\"");
                    IMkvInfo? info = JsonSerializer.Deserialize<IMkvInfo>(output);
                    if(info != null)
                    {
                        HashSet<int> keepAudioTracks = new();
                        HashSet<int> keepSubtitleTracks = new();
                        foreach (ITrack track in info.tracks)
                        {
                            if(track.@type == ETrackType.Audio)
                            {
                                if(audioConfig.Contains(track.properties.language))
                                {
                                    keepAudioTracks.Add(track.id);
                                }
                            }
                            else if (track.@type == ETrackType.Subtitles)
                            {
                                if(subtitleConfig.Contains(track.properties.language))
                                {
                                    keepSubtitleTracks.Add(track.id);
                                }
                            }
                        }
                        if (!skipDialog && (keepSubtitleTracks.Count == 0 || keepAudioTracks.Count == 0))
                        {
                            ContentDialogResult result = await MessageHelper.ShowYesNo($"Subtitle or audio track for file \"{fileName}\" is empty!\nContinue?");
                            if(result != ContentDialogResult.Primary) {
                                return;
                            }
                            skipDialog = true;
                        }
                        await KeepTracks(fileName, keepAudioTracks, keepSubtitleTracks);
                    }
                    UpdateProgressBar();
                }
            }
            catch (Exception ex)
            {
                await MessageHelper.ShowError(ex.Message);
            }
        }

        private async Task<string> KeepTracks(string fileName, HashSet<int> audioTracks, HashSet<int> subtitleTracks)
        {
            string? directory = Path.GetDirectoryName(fileName);
            if (directory == null)
            {
                return string.Empty;
            }
            string destination = Path.Combine(directory, "remuxed", Path.GetFileName(fileName));
            string subtitleString = subtitleTracks.Count == 0 ? "--no-subtitles" : $"--subtitle-tracks {string.Join(",", subtitleTracks)}";
            string audioTrackString = $"--audio-tracks {string.Join(",", audioTracks)}";
            return await ExecuteCommand($" -o \"{destination}\" {audioTrackString} {subtitleString} \"{fileName}\"", true);
        }

        private async Task<string> ExecuteCommand(string command, bool writeAsync = false)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = MkvLocator.MkvMergePath,
                    Arguments = command,
                    StandardOutputEncoding = Encoding.UTF8,
                },
            };
            
            process.Start();
            StringBuilder output = new StringBuilder();
            while (!process.StandardOutput.EndOfStream)
            {
                string? line = await process.StandardOutput.ReadLineAsync();
                if (line != null)
                {
                    if(writeAsync)
                    {
                        UpdateOutput(line);
                    }
                    output.AppendLine(line);
                }
            }
            process.WaitForExit();
            return output.ToString();
        }
    }
}
