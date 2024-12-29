using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace MkvTrackRemover.helpers
{
    internal class MessageHelper
    {
        private readonly DispatcherQueue DispatcherQueue;
        private readonly XamlRoot Root;

        internal MessageHelper(DispatcherQueue dispatcherQueue, XamlRoot xamlRoot)
        {
            DispatcherQueue = dispatcherQueue;
            Root = xamlRoot;

        }

        async internal Task<ContentDialogResult> ShowOkMessage(string content)
        {
            ContentDialog createDialog() => new() { Content = content, Title = "Info", PrimaryButtonText = "Ok" };

            return await RunInMainWindow(createDialog);
        }

        async internal Task<ContentDialogResult> ShowError(string content)
        {
            ContentDialog createDialog() => new() { Content = content, Title = "Error", PrimaryButtonText = "Ok" };

            return await RunInMainWindow(createDialog);
        }

        async internal Task<ContentDialogResult> ShowYesNo(string content)
        {
            ContentDialog createDialog() => new () { Content = content, PrimaryButtonText = "Yes", SecondaryButtonText = "No" };

            return await RunInMainWindow(createDialog);
        }

        async internal Task<ContentDialogResult> RunInMainWindow(Func<ContentDialog> dialogFunc)
        {
            ContentDialogResult result = ContentDialogResult.None;

            await DispatcherQueue.EnqueueAsync(async () =>
            {
                var dialog = dialogFunc();
                dialog.XamlRoot = Root;
                result = await dialog.ShowAsync();
            });
            return result;
        }
    }
}
