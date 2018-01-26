namespace Zebble.Device
{
    using Foundation;
    using MessageUI;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using UIKit;

    partial class Messaging
    {
        public static bool CanSendEmail => Thread.UI.Run(() => MFMailComposeViewController.CanSendMail);
        public static bool CanSendEmailAttachments => true;
        public static bool CanSendEmailBodyAsHtml => true;
        public static bool CanSendSMS => true;
        public static MFMailComposeViewController MailController;

        public static bool CanMakePhoneCall => Thread.UI.Run(() => UIApplication.SharedApplication.CanOpenUrl(new NSUrl($"tel:{string.Empty}").AbsoluteUrl));

        static Task DoSendEmail(EmailMessage email)
        {
            return Thread.UI.Run(() =>
            {
                MailController = new MFMailComposeViewController();
                MailController.SetSubject(email.Subject);
                MailController.SetMessageBody(email.Message, email.IsHtml);
                MailController.SetToRecipients(email.Recipients.ToArray());

                if (email.RecipientsCc.Any()) MailController.SetCcRecipients(email.RecipientsCc.ToArray());

                if (email.RecipientsBcc.Any()) MailController.SetBccRecipients(email.RecipientsBcc.ToArray());

                email.Attachments.Do(a =>
                {
                    if (a.Content == null)
                    {
                        var data = a.FilePath.IsUrl() ? NSData.FromUrl(new NSUrl(a.FilePath)) : NSData.FromFile(a.FilePath);
                        MailController.AddAttachmentData(data, a.ContentType, a.FileName);
                    }
                    else MailController.AddAttachmentData(NSData.FromStream(a.Content), a.ContentType, a.FileName);
                });

                EventHandler<MFComposeResultEventArgs> handler = null;
                handler = (sender, e) =>
                {
                    MailController.Finished -= handler;

                    var uiViewController = sender as UIViewController;
                    if (uiViewController == null)
                    {
                        throw new ArgumentException("sender");
                    }

                    uiViewController.DismissViewController(animated: true, completionHandler: () => { });
                };

                MailController.Finished += handler;

                ((UIViewController)UIRuntime.NativeRootScreen).PresentViewController(MailController, animated: true, completionHandler: () => { });

                return Task.CompletedTask;
            });
        }

        static Task DoMakePhoneCall(string number, string _)
        {
            return Thread.UI.Run(async () =>
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl($"tel:{number}").AbsoluteUrl);

                await Task.CompletedTask;
            });
        }

        static Task DoSendSMS(SMS message)
        {
            return Thread.UI.Run(async () =>
            {
                foreach (var receiver in message.Receiver)
                {
                    var smsTo = NSUrl.FromString("sms:" + receiver + "");
                    UIApplication.SharedApplication.OpenUrl(smsTo);
                }

                await Task.CompletedTask;
            });
        }
    }
}