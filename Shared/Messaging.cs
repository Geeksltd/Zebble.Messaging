namespace Zebble.Device
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static partial class Messaging
    {
        /// <summary>Send an email using the default email application on the device.</summary>
        ///  /// <param name="to">Email recipient</param>
        /// <param name="subject">Email subject</param>
        /// <param name="message">Email message body.</param>  
        public static Task SendEmail(string to, string subject, string message, OnError errorAction = OnError.Alert)
        {
            return SendEmail(new EmailMessage(to, subject, message), errorAction);
        }

        /// <summary>Send an email using the default email application on the device.</summary>
        public static async Task SendEmail(EmailMessage message, OnError errorAction = OnError.Alert)
        {
            if (!CanSendEmail)
            {
                await errorAction.Apply("Your device does not support sending emails.");
            }
            else
            {
                try { await DoSendEmail(message); }
                catch (Exception ex) { await errorAction.Apply("Failed to send email: " + ex.Message); }
            }
        }

        /// <summary>
        /// Make a phone call using the default dialer UI on the device.
        /// </summary>
        /// <param name="number">Number to phone</param>
        public static async Task PhoneCall(string number, string name = null, OnError errorAction = OnError.Alert)
        {
            if (!await Device.Permission.PhoneCall.IsRequestGranted())
            {
                await errorAction.Apply("Permission was denied to make a phone call.");
            }
            else
            {
                try { await DoMakePhoneCall(number, name); }
                catch (Exception ex)
                {
                    await errorAction.Apply("Failed to call number " + number + ". " + ex.Message);
                }
            }
        }

        public static Task SendSMS(string receiver, string messageText, OnError errorAction = OnError.Alert)
        {
            return SendSMS(new SMS { Receiver = new List<string> { receiver }, MessageText = messageText }, errorAction);
        }

        public static Task SendSMS(List<string> receiver, string messageText, OnError errorAction = OnError.Alert)
        {
            return SendSMS(new SMS { Receiver = receiver, MessageText = messageText }, errorAction);
        }

        public static async Task SendSMS(SMS message, OnError errorAction = OnError.Alert)
        {
            if (!CanSendSMS)
            {
                await errorAction.Apply("Your device does not support sending SMS.");
            }
            else
            {
                try { await DoSendSMS(message); }
                catch (Exception ex) { await errorAction.Apply("Failed to send SMS: " + ex.Message); }
            }
        }
    }
}