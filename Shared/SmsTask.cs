namespace Zebble.Device
{
    public abstract partial class SmsTask
    {
        public bool CanSendSms;
        public abstract void SendSms(string recipient = null, string message = null);
    }
}
