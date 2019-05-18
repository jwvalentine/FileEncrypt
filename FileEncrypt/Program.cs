using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace FileEncrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath;
            string input;
            string key = "dfoishdfiophonsidfoishdfiophonsi"; // 32 characters -- This should be stored in Vault A
            string iv = "dfoishdfiophonsi"; // 16 characters -- This should be stored in Vault B

            Console.WriteLine("Specifiy the exact path, filename, and extension that you wish to edit:\n(e.g. C:\\EncryptMe\\file.txt)");
            filePath = Console.ReadLine();

            while (true)
            {
                Console.WriteLine("e) Encrypt");
                Console.WriteLine("d) Decrypt");
                Console.WriteLine("x) Exit");
                input = Console.ReadLine();

                if (input == "e")
                {
                    EncryptFile(filePath, key, iv);
                }
                else if (input == "d")
                {
                    DecryptFile(filePath, key, iv);
                }
                else
                {
                    break;
                }
            }

        } // End of Main

        static void EncryptFile(string filePath, string key, string iv)
        {
            byte[] plainContent = File.ReadAllBytes(filePath);
            using (var AES = new AesCryptoServiceProvider())
            {
                AES.BlockSize = 128;
                AES.KeySize = 256;
                AES.IV = ASCIIEncoding.ASCII.GetBytes(iv);
                AES.Key = ASCIIEncoding.ASCII.GetBytes(key);
                AES.Padding = PaddingMode.PKCS7;
                AES.Mode = CipherMode.CBC;

                using (var ms = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write);

                    cryptoStream.Write(plainContent, 0, plainContent.Length);
                    cryptoStream.FlushFinalBlock();
                    File.WriteAllBytes(filePath, ms.ToArray());
                    Console.WriteLine("Encrypted Successfully!" + filePath);
                }
            }
        }



        static void DecryptFile(string filePath, string key, string iv)
        {
            byte[] encrypted = File.ReadAllBytes(filePath);
            using (var AES = new AesCryptoServiceProvider())
            {
                AES.BlockSize = 128;
                AES.KeySize = 256;
                AES.IV = ASCIIEncoding.ASCII.GetBytes(iv);
                AES.Key = ASCIIEncoding.ASCII.GetBytes(key);
                AES.Padding = PaddingMode.PKCS7;
                AES.Mode = CipherMode.CBC;

                using (var ms = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write);

                    cryptoStream.Write(encrypted, 0, encrypted.Length);
                    cryptoStream.FlushFinalBlock();
                    File.WriteAllBytes(filePath, ms.ToArray());
                    Console.WriteLine("Decrypted Successfully!" + filePath);
                }
            }


        }
    } // End of Class
}  // End of Namespace
