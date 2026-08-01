```csharp
using System.IO;
using System.Text;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

string host = config["Smtp:Host"];
int port = config.GetValue<int>("Smtp:Port");
string userName = config["Smtp:UserName"];
string password = config["Smtp:Password"];

string toAddress = config["ToAddress"];
string toName = config["ToName"];
string fromAddress = config["FromAddress"];
string fromName = config["FromName"];

var msg = new MimeMessage();
msg.To.Add(new MailboxAddress(toName, toAddress));
msg.From.Add(new MailboxAddress(fromName, fromAddress));
msg.Subject = "This is a test";

var bodyBuilder = new BodyBuilder();
bodyBuilder.HtmlBody = "<b>Good morning</b> captain";
byte[] attachmentBytes = Encoding.UTF8.GetBytes("hello there");
bodyBuilder.Attachments.Add("some-file.txt", 
    attachmentBytes, new ContentType("text", "plain"));

msg.Body = bodyBuilder.ToMessageBody();

// If we weren't using an attachment
// msg.Body = new TextPart(TextFormat.Html) { Text = "<b>Hello</b> world!" };

using var emailClient = new SmtpClient();

await emailClient.ConnectAsync(host, port, useSsl: true).ConfigureAwait(false);

// If you're using non-SSL/TLS, you'd have:
// await emailClient.ConnectAsync(host, port: 25, options: SecureSocketOptions.None);

await emailClient.AuthenticateAsync(userName, password).ConfigureAwait(false);
await emailClient.SendAsync(msg).ConfigureAwait(false);
await emailClient.DisconnectAsync(true).ConfigureAwait(false);
```