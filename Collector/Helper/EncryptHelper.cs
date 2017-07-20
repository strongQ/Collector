using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Collector.Helper
{
    public class EncryptHelper
    {
        //缓冲区大小
        static int bufferSize = 128 * 1024;
        //密钥salt  防止“字典攻击”
        static byte[] salt = { 134, 216, 7, 36, 88, 164, 91, 227, 174, 76, 191, 197, 192, 154, 200, 248 };
        //初始化向量
        static byte[] iv = { 134, 216, 7, 36, 88, 164, 91, 227, 174, 76, 191, 197, 192, 154, 200, 248 };

        //初始化并返回对称加密算法
        static SymmetricAlgorithm CreateRijndael(string password, byte[] salt)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt, "SHA256", 1000);
            SymmetricAlgorithm sma = Rijndael.Create();
            sma.KeySize = 256;
            sma.Key = pdb.GetBytes(32);
            sma.Padding = PaddingMode.PKCS7;
            return sma;
        }
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        /// <param name="password"></param>
        public static void EncryptFile(string inFile, string outFile, string password)
        {
            using (FileStream inFileStream = File.OpenRead(inFile), outFileStream = File.Open(outFile, FileMode.OpenOrCreate))
            using (SymmetricAlgorithm algorithm = CreateRijndael(password, salt))
            {
                algorithm.IV = iv;
                using (CryptoStream cryptoStream = new CryptoStream(outFileStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] bytes = new byte[bufferSize];
                    int readSize = -1;
                    while ((readSize = inFileStream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        cryptoStream.Write(bytes, 0, readSize);
                    }
                    cryptoStream.Flush();
                }
            }
        }
        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        /// <param name="password"></param>
        public static void DecryptFile(string inFile, string outFile, string password)
        {
            using (FileStream inFileStream = File.OpenRead(inFile), outFileStream = File.OpenWrite(outFile))
            using (SymmetricAlgorithm algorithm = CreateRijndael(password, salt))
            {
                algorithm.IV = iv;
                using (CryptoStream cryptoStream = new CryptoStream(inFileStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] bytes = new byte[bufferSize];
                    int readSize = -1;
                    int numReads = (int)(inFileStream.Length / bufferSize);
                    int slack = (int)(inFileStream.Length % bufferSize);
                    for (int i = 0; i < numReads; ++i)
                    {
                        readSize = cryptoStream.Read(bytes, 0, bytes.Length);
                        outFileStream.Write(bytes, 0, readSize);
                    }
                    if (slack > 0)
                    {
                        readSize = cryptoStream.Read(bytes, 0, (int)slack);
                        outFileStream.Write(bytes, 0, readSize);
                    }
                    outFileStream.Flush();
                }
            }
        }


        #region Base64加密解密
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns></returns>
        public static string Base64Encrypt(string input)
        {
            return Base64Encrypt(input, new UTF8Encoding());
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <param name="encode">字符编码</param>
        /// <returns></returns>
        public static string Base64Encrypt(string input, Encoding encode)
        {
            return Convert.ToBase64String(encode.GetBytes(input));
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="input">需要解密的字符串</param>
        /// <returns></returns>
        public static string Base64Decrypt(string input)
        {
            return Base64Decrypt(input, new UTF8Encoding());
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="input">需要解密的字符串</param>
        /// <param name="encode">字符的编码</param>
        /// <returns></returns>
        public static string Base64Decrypt(string input, Encoding encode)
        {
            return encode.GetString(Convert.FromBase64String(input));
        }
        #endregion
 
    }
}
