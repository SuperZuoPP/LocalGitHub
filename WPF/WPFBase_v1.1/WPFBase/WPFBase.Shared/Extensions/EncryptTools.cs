using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WPFBase.Shared.Extensions
{
    public class EncryptTools
    {
        // <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strPwd">被加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public static string GetMD5(string strPwd)
        {
            //实例化一个md5对象
            MD5 md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(strPwd));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, string key)
        {
            string ketstr = key.Substring(0, 16);
            if (string.IsNullOrEmpty(toEncrypt)) return null;
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(ketstr);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(ketstr);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged() { Key = keyArray, IV = ivArray, Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
        public static string Decrypt(string toDecrypt, string key)
        {
            if (string.IsNullOrEmpty(toDecrypt)) return null;
            string ketstr = key.Substring(0, 16);
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(ketstr);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(ketstr);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged() { Key = keyArray, IV = ivArray, Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            List<byte> blist = new List<byte>();
            blist.AddRange(resultArray);
            for (int i = blist.Count - 1; i >= 0; i--)
            {
                if (blist[i] == 0)
                {
                    blist.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
            return UTF8Encoding.UTF8.GetString(blist.ToArray());
        }
    }
}
