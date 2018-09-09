using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace QTrans.WebAPI.Helpers
{
    public class EncryptDecryptHelper
    {
        public static string key = ConfigurationManager.AppSettings["EncryptionKey"];

        public static string Encrypt(string Encryptval)
        {
            byte[] SrctArray;
            byte[] EnctArray = UTF8Encoding.UTF8.GetBytes(Encryptval);
            SrctArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objcrpt = new MD5CryptoServiceProvider();
            SrctArray = objcrpt.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            objcrpt.Clear();
            objt.Key = SrctArray;
            objt.Mode = CipherMode.ECB;
            objt.Padding = PaddingMode.PKCS7;
            ICryptoTransform crptotrns = objt.CreateEncryptor();
            byte[] resArray = crptotrns.TransformFinalBlock(EnctArray, 0, EnctArray.Length);
            objt.Clear();
            return Convert.ToBase64String(resArray, 0, resArray.Length);
        }

        public static string Decrypt(string DecryptText)
        {
            byte[] SrctArray;
            byte[] DrctArray = Convert.FromBase64String(DecryptText);
            SrctArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objmdcript = new MD5CryptoServiceProvider();
            SrctArray = objmdcript.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            objmdcript.Clear();
            objt.Key = SrctArray;
            objt.Mode = CipherMode.ECB;
            objt.Padding = PaddingMode.PKCS7;
            ICryptoTransform crptotrns = objt.CreateDecryptor();
            byte[] resArray = crptotrns.TransformFinalBlock(DrctArray, 0, DrctArray.Length);
            objt.Clear();
            return UTF8Encoding.UTF8.GetString(resArray);
        }

        public static string ReverseString(string value)
        {
            char[] charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string SplitAndCombine(string value)
        {
            string[] splitArray = value.Split('@');
            return splitArray[0] + "|" + splitArray[1];
        }

        public static string ComputePasswordHash(string TextToBeEncrypted, string saltText)
        {
            if (TextToBeEncrypted == string.Empty || TextToBeEncrypted == null)
            {
                return TextToBeEncrypted;
            }
            saltText = ConfigurationManager.AppSettings["EncryptionKey"] + "_" + saltText;
            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            byte[] PlainText = Encoding.Unicode.GetBytes(TextToBeEncrypted);
            byte[] Salt = Encoding.ASCII.GetBytes(saltText.Length.ToString());
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(saltText, Salt);

            //Creates a symmetric encryptor object.
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream();

            //Defines a stream that links data streams to cryptographic transformations
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(PlainText, 0, PlainText.Length);

            //Writes the final state and clears the buffer
            cryptoStream.FlushFinalBlock();
            byte[] CipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string EncryptedData = Convert.ToBase64String(CipherBytes);

            return EncryptedData;
        }
        public static string DecryptPassword(string TextToBeDecrypted, string saltText)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            string DecryptedData;

            try
            {
                saltText = ConfigurationManager.AppSettings["EncryptionKey"] + "_" + saltText;

                byte[] EncryptedData = Convert.FromBase64String(TextToBeDecrypted.Replace(' ', '+'));
                byte[] Salt = Encoding.ASCII.GetBytes(saltText.Length.ToString());

                //Making of the key for decryption
                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(saltText, Salt);

                //Creates a symmetric Rijndael decryptor object.
                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(EncryptedData);

                //Defines the cryptographics stream for decryption.THe stream contains decrpted data
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
                byte[] PlainText = new byte[EncryptedData.Length];
                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
                memoryStream.Close();
                cryptoStream.Close();

                //Converting to string
                DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            }
            catch
            {
                DecryptedData = TextToBeDecrypted;
            }

            return DecryptedData;
        }
    }
}