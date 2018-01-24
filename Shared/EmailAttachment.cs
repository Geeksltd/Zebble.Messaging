namespace Zebble.Device
{
    using System.IO;

    public partial class EmailAttachment
    {
        public Stream Content;
        public string FileName;
        public string ContentType;
        public string FilePath;
    }
}
