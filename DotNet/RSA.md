```csharp
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

const int KEY_SIZE = 2048;
var DEFAULT_HASH_ALGO = HashAlgorithmName.SHA512;
var DEFAULT_SIG_PADDING = RSASignaturePadding.Pss;
var DEFAULT_PADDING = RSAEncryptionPadding.OaepSHA512;

var keys = CreateKeyAsXML();

Console.WriteLine($"\nPublic: {keys.PublicKey}\n");
Console.WriteLine($"\nPrivate: {keys.PrivateKey}\n");

byte[] message = Encoding.UTF8.GetBytes("Hello world!");

byte[] encrypted = Encrypt(keys.PublicKey, message);
Console.WriteLine($"\nEncrypted: {Convert.ToBase64String(encrypted)}\n");
byte[] decrypted = Decrypt(keys.PrivateKey, encrypted);
Console.WriteLine($"\nDecrypted: {Encoding.UTF8.GetString(decrypted)}\n");

byte[] signature = CreateSignature(keys.PrivateKey, message);
Console.WriteLine($"\nSignature: {Convert.ToBase64String(signature)}");

bool isVerified = VerifySignature(keys.PublicKey, message, signature);
Console.WriteLine($"\nVerified: {isVerified}\n");

// Mess with the message
var badMessage = message.ToArray();
badMessage[3] = 34;
isVerified = VerifySignature(keys.PublicKey, badMessage, signature);
Console.WriteLine($"Verified: {isVerified}\n");

bool VerifySignature(string publicKeyXML, byte[] message, byte[] signature)
{
    using var rsa = RSA.Create(KEY_SIZE);
    rsa.FromXmlString(publicKeyXML);
    return rsa.VerifyData(message, signature, DEFAULT_HASH_ALGO, DEFAULT_SIG_PADDING);
}

byte[] CreateSignature(string privateKeyXML, byte[] plainText)
{
    using var rsa = RSA.Create(KEY_SIZE);
    rsa.FromXmlString(privateKeyXML);
    return rsa.SignData(plainText, DEFAULT_HASH_ALGO, DEFAULT_SIG_PADDING);
}

byte[] Encrypt(string publicKeyXML, byte[] plainText)
{
    using var rsa = RSA.Create(KEY_SIZE);
    rsa.FromXmlString(publicKeyXML);
    return rsa.Encrypt(plainText, DEFAULT_PADDING);
}

byte[] Decrypt(string privateKeyXML, byte[] encrypted)
{
    using var rsa = RSA.Create(KEY_SIZE);
    rsa.FromXmlString(privateKeyXML);
    return rsa.Decrypt(encrypted, DEFAULT_PADDING);
}

(string PublicKey, string PrivateKey) CreateKeyAsXML()
{
    using var rsa = RSA.Create(KEY_SIZE);
    return (
        rsa.ToXmlString(includePrivateParameters: false),
        rsa.ToXmlString(includePrivateParameters: true)
    );
}
```