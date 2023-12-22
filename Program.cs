using System;
using System.Security.Cryptography;
using System.Text;

namespace AES

{
    class Program
    {
        static void Main(string[] args)
        {
            // AES key of 128 bits (16 bytes)
            string key = "0123456789ABCDEF";

            // Initialization Vector (IV) of 128 bits (16 bytes)
            string iv = "FEDCBA9876543210";

            Console.WriteLine("Enter the text you want to encrypt:");
            // Read user input for the original text to be encrypted
            string originalText = Console.ReadLine();

            Console.WriteLine("Original Text:  " + originalText);

            // Encrypt the text
            string encryptedText = EncryptAES(originalText, key, iv);
            Console.WriteLine("Encrypted Text: " + encryptedText);

            // Decrypt the text
            string decryptedText = DecryptAES(encryptedText, key, iv);
            Console.WriteLine("Decrypted Text: " + decryptedText);
        }

        static string EncryptAES(string plainText, string key, string iv)
        {
            // Create a new instance of the AES algorithm
            using (Aes aesAlg = Aes.Create())
            {
                // Set the key for AES encryption by converting the string key to bytes
                aesAlg.Key = Encoding.UTF8.GetBytes(key);

                // Set the Initialization Vector (IV) for AES encryption by converting the string IV to bytes
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                // Create an AES encryptor using the specified key and IV
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create a memory stream to store the encrypted data
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    // Create a CryptoStream to perform the encryption
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        // Create a StreamWriter to write the plaintext into the CryptoStream
                        using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                        {
                            // Write the plaintext into the CryptoStream, which will encrypt it
                            swEncrypt.Write(plainText);
                        }
                    }

                    // Convert the encrypted data in the memory stream to a base64-encoded string
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        static string DecryptAES(string cipherText, string key, string iv)
        {
            // Create a new instance of the AES algorithm
            using (Aes aesAlg = Aes.Create())
            {
                // Set the key for AES decryption by converting the string key to bytes
                aesAlg.Key = Encoding.UTF8.GetBytes(key);

                // Set the Initialization Vector (IV) for AES decryption by converting the string IV to bytes
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                // Create an AES decryptor using the specified key and IV
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create a memory stream to store the decrypted data, initializing it with the base64-decoded cipherText
                using (var msDecrypt = new System.IO.MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    // Create a CryptoStream to perform the decryption
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        // Create a StreamReader to read the decrypted data from the CryptoStream
                        using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                        {
                            // Read the decrypted data from the StreamReader and return it as a string
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
