using MessageUI;
using System;
using System.Linq;
using System.Threading.Tasks;
using UIKit;

namespace Zebble.Device
{
    internal static partial class SMSHelper
    {
        internal static bool IsComposeSupported
            => MFMessageComposeViewController.CanSendText;

        static Task PlatformComposeAsync(SMS message)
        {
            // do this first so we can throw as early as possible
            var controller = PlatformHelper.GetCurrentViewController();

            // create the controller
            var messageController = new MFMessageComposeViewController();
            if (!string.IsNullOrWhiteSpace(message?.MessageText))
                messageController.Body = message.MessageText;

            messageController.Recipients = message?.Receiver?.ToArray() ?? new string[] { };

            // show the controller
            var tcs = new TaskCompletionSource<bool>();
            messageController.Finished += (sender, e) =>
            {
                messageController.DismissViewController(true, null);
                tcs?.TrySetResult(e.Result == MessageComposeResult.Sent);
            };
            controller.PresentViewController(messageController, true, null);

            return tcs.Task;
        }
    }

    internal static class PlatformHelper
    {
        internal static UIViewController GetCurrentViewController(bool throwIfNull = true)
        {
            UIViewController viewController = null;

            var window = UIApplication.SharedApplication.KeyWindow;

            if (window.WindowLevel == UIWindowLevel.Normal)
                viewController = window.RootViewController;

            if (viewController == null)
            {
                window = UIApplication.SharedApplication
                    .Windows
                    .OrderByDescending(w => w.WindowLevel)
                    .FirstOrDefault(w => w.RootViewController != null && w.WindowLevel == UIWindowLevel.Normal);

                if (window == null)
                    throw new InvalidOperationException("Could not find current view controller.");
                else
                    viewController = window.RootViewController;
            }

            while (viewController.PresentedViewController != null)
                viewController = viewController.PresentedViewController;

            if (throwIfNull && viewController == null)
                throw new InvalidOperationException("Could not find current view controller.");

            return viewController;
        }
    }
}