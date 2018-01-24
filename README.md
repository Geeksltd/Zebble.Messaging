[logo]: https://raw.githubusercontent.com/Geeksltd/Zebble.Messaging/master/Shared/NuGet/Icon.png "Zebble.Messaging"


## Zebble.Messaging

![logo]

A Zebble plugin to make external communications in Zebble applications.


[![NuGet](https://img.shields.io/nuget/v/Zebble.Messaging.svg?label=NuGet)](https://www.nuget.org/packages/Zebble.Messaging/)

> With this plugin you can make call or send a message and email on Android, IOS and UWP platforms.

<br>


### Setup
* Available on NuGet: [https://www.nuget.org/packages/Zebble.Messaging/](https://www.nuget.org/packages/Zebble.Messaging/)
* Install in your platform client projects.
* Available for iOS, Android and UWP.
<br>


### Api Usage

You can call `Zebble.Device.Messaging` from any project to gain access to APIs.

##### Making a phone call:
```csharp
if (Device.Messaging.CanMakePhoneCall)
{
    await Device.Messaging.PhoneCall("01234 5678 90");
}
```
##### Sending an Email:
```csharp
if (Device.Messaging.CanSendEmail)
{
    await Device.Messaging.SendEmail("someone@email.com", "my subject", "my body...");
}
```
You can alternatively use another method overload to add more details such as Cc, Bcc, attachment, etc.
```csharp
Device.Messaging.SendEmail(new Device.EmailMessage { ... });
```
##### Sending an SMS
```csharp
if (Device.Messaging.CanSendSms)
{
    await Device.Messaging.SendSms("0789 654 123", "my message");
}
```
<br>


### Properties
| Property     | Type         | Android | iOS | Windows |
| :----------- | :----------- | :------ | :-- | :------ |
| CanSendEmail           | bool          | x       | x   | x       |
| CanMakePhoneCall           | bool          | x       | x   | x       |
| CanSendSMS           | bool          | x       | x   | x       |
| CanSendEmailAttachments           | bool          | x       | x   | x       |
| CanSendEmailBodyAsHtml           | bool          | x       | x   | x       |

<br>

### Methods
| Method       | Return Type  | Parameters                          | Android | iOS | Windows |
| :----------- | :----------- | :-----------                        | :------ | :-- | :------ |
| PhoneCall         | Task | number -> string<br> name -> string<br> errorAction-> OnError| x       | x   | x       |
| SendEmail         | Task | to -> string<br> subject -> string<br> message -> string<br> errorAction -> OnError| x       | x   | x       |
| SendEmail         | Task | message -> EmailMessage<br> errorAction -> OnError| x       | x   | x       |
| SendSms         | Task | receiver -> string<br> messageText -> string<br> errorAction -> OnError| x       | x   | x       |
| SendSms         | Task | receiver -> List<string&gt;<br> messageText -> string<br> errorAction -> OnError| x       | x   | x       |
| SendSms         | Task | message -> SMS<br> errorAction -> OnError| x       | x   | x       |
