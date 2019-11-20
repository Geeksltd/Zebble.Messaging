namespace Zebble.Device
{
    using System.Collections.Generic;
    using System.Linq;

    public partial class SMS
    {
        public List<string> Receiver;
        public string MessageText;

        public SMS()
        {
            MessageText = string.Empty;
            Receiver = new List<string>();
        }

        public SMS(string body, string recipient) : base()
        {
            MessageText = body;
            if (!string.IsNullOrWhiteSpace(recipient))
                Receiver.Add(recipient);
        }

        public SMS(string body, IEnumerable<string> recipients) : base()
        {
            MessageText = body;
            if (recipients != null)
            {
                Receiver.AddRange(recipients.Where(x => !string.IsNullOrWhiteSpace(x)));
            }
        }
    }



}
