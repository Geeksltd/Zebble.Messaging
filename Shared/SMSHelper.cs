using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zebble.Device
{
    internal static partial class SMSHelper
    {
        public static Task ComposeAsync()
            => ComposeAsync(null);

        public static Task ComposeAsync(SMS message)
        {
            if (!IsComposeSupported)
                throw new System.Exception("Sms is not supported");

            if (message == null)
                message = new SMS();

            if (message?.Receiver == null)
                message.Receiver = new List<string>();

            return PlatformComposeAsync(message);
        }
    }
}
