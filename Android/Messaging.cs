namespace Zebble.Device
{
    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Telephony;
    using Android.Text;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    partial class Messaging
    {
        public static bool CanSendEmail
        {
            get
            {
                var mgr = Application.Context.PackageManager;
                var emailIntent = new Intent(Intent.ActionSend);
                emailIntent.SetType("message/rfc822");
                return emailIntent.ResolveActivity(mgr) != null;
            }
        }

        public static bool CanMakePhoneCall => (new Intent(Intent.ActionDial)).ResolveActivity(Application.Context.PackageManager) != null;

        public static bool CanSendSMS => true;
        public static bool CanSendEmailAttachments => true;
        public static bool CanSendEmailBodyAsHtml => true;

        static Task DoSendEmail(EmailMessage email)
        {
            var intentAction = Intent.ActionSend;
            if (email.Attachments?.Count() > 1)
                intentAction = Intent.ActionSendMultiple;

            var emailIntent = new Intent(intentAction);
            emailIntent.SetType("message/rfc822");

            if (email.Recipients.Any()) emailIntent.PutExtra(Intent.ExtraEmail, email.Recipients.ToArray());

            if (email.RecipientsCc.Any()) emailIntent.PutExtra(Intent.ExtraCc, email.RecipientsCc.ToArray());

            if (email.RecipientsBcc.Any()) emailIntent.PutExtra(Intent.ExtraBcc, email.RecipientsBcc.ToArray());

            emailIntent.PutExtra(Intent.ExtraSubject, email.Subject);

            if (email.IsHtml) emailIntent.PutExtra(Intent.ExtraText, Html.FromHtml(email.Message));
            else emailIntent.PutExtra(Intent.ExtraText, email.Message);

            if (email.Attachments?.Any() == true)
            {
                var uris = new List<IParcelable>();
                email.Attachments.Do(async (a) =>
                {
                    var path = a.FilePath;

                    if (a.FilePath.IsUrl())
                    {
                        if (IO.ExternalStorageFolder.LacksValue())
                            throw new Exception("File attachement is not supported on your device. You need to add a SD card to enable this feature.");

                        path = Path.Combine(IO.ExternalStorageFolder, a.FileName);
                        var downloaded = await Network.Download(new Uri(a.FilePath), path, OnError.Alert);
                    }

                    uris.Add(Android.Net.Uri.Parse("file://" + path));
                });

                if (uris.Count > 1) emailIntent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
                else emailIntent.PutExtra(Intent.ExtraStream, uris[0]);
            }

              ((Activity)UIRuntime.NativeRootScreen).StartActivity(emailIntent);

            return Task.CompletedTask;
        }

        static Task DoMakePhoneCall(string number, string _)
        {
            ((Activity)UIRuntime.NativeRootScreen).StartActivity(
                new Intent(Intent.ActionDial, Android.Net.Uri.Parse("tel:" + PhoneNumberUtils.FormatNumber(number))));

            return Task.CompletedTask;
        }
    }
}