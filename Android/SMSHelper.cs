using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zebble.Device
{
    internal static partial class SMSHelper
    {
        static readonly string smsRecipientSeparator = ";";

        internal static bool IsComposeSupported
            => PlatformHelper.IsIntentSupported(CreateIntent(null, new List<string> { "0000000000" }));

        static Task PlatformComposeAsync(SMS message)
        {
            var intent = CreateIntent(message?.MessageText, message?.Receiver)
                .SetFlags(ActivityFlags.ClearTop)
                .SetFlags(ActivityFlags.NewTask);

            Application.Context.StartActivity(intent);

            return Task.FromResult(true);
        }

        static Intent CreateIntent(string body, List<string> recipients)
        {
            Intent intent = null;

            body = body ?? string.Empty;

            if (OS.IsAtLeast(BuildVersionCodes.Kitkat) && recipients.All(x => !x.HasValue()))
            {
                var packageName = Telephony.Sms.GetDefaultSmsPackage(Application.Context);
                if (!string.IsNullOrWhiteSpace(packageName))
                {
                    intent = new Intent(Intent.ActionSend);
                    intent.SetType("text/plain");
                    intent.PutExtra(Intent.ExtraText, body);
                    intent.SetPackage(packageName);

                    return intent;
                }
            }

            // Fall back to normal send
            intent = new Intent(Intent.ActionView);

            if (body.HasValue())
                intent.PutExtra("sms_body", body);

            var recipienturi = string.Join(smsRecipientSeparator, recipients.Select(r => Android.Net.Uri.Encode(r)));

            var uri = Android.Net.Uri.Parse($"smsto:{recipienturi}");
            intent.SetData(uri);

            return intent;
        }
    }
    internal static class PlatformHelper
    {
        internal static bool IsIntentSupported(Intent intent)
        {
            var manager = Application.Context.ApplicationContext.PackageManager;
            var activities = manager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return activities.Any();
        }
    }
}