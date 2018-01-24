namespace Zebble.Device
{
    using System;
    using System.Collections.Generic;

    public partial class EmailMessage
    {
        public bool IsHtml;
        public string Message;
        public string Subject;
        public List<string> Recipients = new List<string>();
        public List<string> RecipientsBcc = new List<string>();
        public List<string> RecipientsCc = new List<string>();
        public List<EmailAttachment> Attachments;

        internal EmailMessage() { Subject = string.Empty; Message = string.Empty; }

        public EmailMessage(string to) : this() { if (!string.IsNullOrWhiteSpace(to)) Recipients.Add(to); }

        public EmailMessage(string to = null, string subject = null, string message = null) : this(to)
        {
            Subject = subject ?? string.Empty;
            Message = message ?? string.Empty;
        }
    }
}
