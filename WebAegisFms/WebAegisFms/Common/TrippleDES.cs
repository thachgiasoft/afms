using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    /// <summary>
    /// Class for triple DES Cryptogrgrpy.
    /// </summary>
    //Nilesh@20100516 : Converted class to static to remove "Static holder types should not have constructors" error
    public static class TrippleDES
    {

        public static string Encrypt(string s)
        {
            return Encrypt(s, "sangitla");
        }

        public static string Decrypt(string s)
        {
            return Decrypt(s, "sangitla");
        }

        /// <summary>
        /// Encrypts provided string parameter
        /// </summary>
        public static string Encrypt(string s, string EncrKey)
        {
            if (EncrKey == null && EncrKey == "")
            {
                throw new Exception("Invalid encryption key provided");
            }

            if (s == null || s.Length == 0) return string.Empty;

            string result = string.Empty;

            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(s);

                TripleDESCryptoServiceProvider des =
                    new TripleDESCryptoServiceProvider();

                MD5CryptoServiceProvider MD5 =
                    new MD5CryptoServiceProvider();

                des.Key =
                    MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(EncrKey));

                des.IV = new byte[8] { 240, 3, 45, 29, 0, 76, 173, 59 };
                result = Convert.ToBase64String(
                    des.CreateEncryptor().TransformFinalBlock(
                        buffer, 0, buffer.Length));
            }
            catch
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// Decrypts provided string parameter
        /// </summary>
        public static string Decrypt(string s, string DecrKey)
        {
            if (DecrKey == null && DecrKey == "")
            {
                throw new Exception("Invalid decryption key provided");
            }

            if (s == null || s.Length == 0) return string.Empty;

            string result = string.Empty;

            try
            {
                byte[] buffer = Convert.FromBase64String(s);

                TripleDESCryptoServiceProvider des =
                    new TripleDESCryptoServiceProvider();

                MD5CryptoServiceProvider MD5 =
                    new MD5CryptoServiceProvider();

                des.Key =
                    MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(DecrKey));

                des.IV = new byte[8] { 240, 3, 45, 29, 0, 76, 173, 59 };

                result = Encoding.ASCII.GetString(
                    des.CreateDecryptor().TransformFinalBlock(
                    buffer, 0, buffer.Length));
            }
            catch
            {
                result = "";
            }

            return result;
        }

        // File Encryption Decryption

        public static bool EncryptFile(string inputFile, string outputFile)
        {
            return EncryptFile(inputFile, outputFile, "sangitla");
        }

        public static bool DecryptFile(string inputFile, string outputFile)
        {
            return DecryptFile(inputFile, outputFile, "sangitla");
        }

        public static bool EncryptFile(string inputFile, string outputFile, string encrKey)
        {

            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(encrKey);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                TripleDESCryptoServiceProvider tDes = new TripleDESCryptoServiceProvider();

                MD5CryptoServiceProvider MD5 =
                new MD5CryptoServiceProvider();

                key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(encrKey));


                CryptoStream cs = new CryptoStream(fsCrypt,
                    tDes.CreateEncryptor(key, new byte[8] { 240, 3, 45, 29, 0, 76, 173, 59 }),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);
                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();

                return true;
            }
            catch (Exception)
            {
                //throw ex;
                //Console.WriteLine("Encryption failed!", "Error");
                return false;
            }
        }

        public static bool DecryptFile(string inputFile, string outputFile, string encrKey)
        {
            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(encrKey);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                TripleDESCryptoServiceProvider tDes = new TripleDESCryptoServiceProvider();

                MD5CryptoServiceProvider MD5 =
                new MD5CryptoServiceProvider();

                key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(encrKey));

                CryptoStream cs = new CryptoStream(fsCrypt,
                   tDes.CreateDecryptor(key, new byte[8] { 240, 3, 45, 29, 0, 76, 173, 59 }),
                   CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
