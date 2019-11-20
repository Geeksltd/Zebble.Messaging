namespace Zebble.Device
{
    using System;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Calls;
    using Windows.ApplicationModel.Email;
    using Windows.Storage;
    using Windows.Storage.Streams;

    partial class Messaging
    {
        public static bool CanSendEmail => true;
        public static bool CanSendEmailAttachments => true;
        public static bool CanSendEmailBodyAsHtml => true;
        public static bool CanMakePhoneCall => !PhoneCallManager.IsCallActive;
        public static bool CanSendSMS => true;

        static async Task DoSendEmail(EmailMessage email)
        {
            var emailMessage = new Windows.ApplicationModel.Email.EmailMessage { Body = email.Message };

            email.Attachments.Do(async (a) =>
            {
                RandomAccessStreamReference stream;
                if (a.FilePath.IsUrl())
                    stream = RandomAccessStreamReference.CreateFromUri(new Uri(a.FilePath));
                else stream = RandomAccessStreamReference.CreateFromFile(await StorageFile.GetFileFromPathAsync(a.FilePath));
                var attachmentFile = new Windows.ApplicationModel.Email.EmailAttachment(a.FileName, stream);
                emailMessage.Attachments.Add(attachmentFile);
            });

            email.Recipients.Do(r => emailMessage.To.Add(new EmailRecipient(r)));
            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }

        static Task DoMakePhoneCall(string number, string name)
        {
            PhoneCallManager.ShowPhoneCallUI(number, name.OrEmpty());
            return Task.CompletedTask;
        }
    }
}