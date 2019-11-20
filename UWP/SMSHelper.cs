using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Chat;

namespace Zebble.Device
{
    internal partial class SMSHelper
    {
        internal static bool IsComposeSupported = true;
        internal static async Task PlatformComposeAsync(SMS message)
        {
            try
            {
                var sms = new ChatMessage { Body = message.MessageText, MessageKind = ChatMessageKind.Standard, MessageOperatorKind = ChatMessageOperatorKind.Sms };

                foreach (var receiver in message.Receiver)
                    sms.Recipients.Add(receiver);
                await ChatMessageManager.ShowComposeSmsMessageAsync(sms);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("SMS sending failed. It is possible that your device does not support SMS.", ex);
            }

        }
    }
}
