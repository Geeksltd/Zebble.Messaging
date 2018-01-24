namespace Zebble.Device
{
    public abstract partial class PhoneCallTask
    {
        public bool CanMakePhoneCall;
        public abstract void MakePhoneCall(string number, string name = null);
    }
}
