using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace MPB_BLL.COMMON
{
    /// <summary>
    /// USE AES DeCode 
    /// </summary>
    public class EnDeCode: BLLBase
    {
        private static string Key = ConfigurationManager.AppSettings["EnDeCodeKey"].PadRight(32, '0');
        private static string IDNOKey = ConfigurationManager.AppSettings["QueryRegisterDtlKey"].PadRight(32, '0');
        /// <summary>
        /// QRCODE Decrypt
        /// </summary>
        /// <param name="encryptData"> QRCode </param> 
        /// <param name="result"> 解碼後資料 </param>
        /// <returns> 是否為正式票券 </returns>
        public static bool DecryptAES256(string encryptData, out string result, KeyType kt = KeyType.AMRS)
        {
            if (kt == KeyType.AMRS)
                return DecryptAES256(encryptData, Key, out result);
            else if (kt == KeyType.IDNO)
                return DecryptAES256(encryptData, IDNOKey, out result);
            else
            {
                result = "解密失敗: 無密鑰類別!";
                return false;
            }
        }

        public static bool DecryptAES256(string encryptData, string key, out string result)
        {
            try
            {
                string iv = encryptData.Substring(encryptData.Length - 16, 16);
                encryptData = encryptData.Substring(0, encryptData.Length - 16);

                byte[] encryptBytes = Convert.FromBase64String(encryptData);
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = Encoding.UTF8.GetBytes(iv);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform transform = aes.CreateDecryptor();

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                        {
                            cs.Write(encryptBytes, 0, encryptBytes.Length);
                            cs.FlushFinalBlock();
                            result = Encoding.UTF8.GetString(ms.ToArray());
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("解密失敗:" + encryptData + "\r\n" + ex.Message);
                //result = "解密失敗:" + ex.Message;
                result = "解密失敗:" + encryptData;
                return false;
            }
        }

        //public static bool DecryptAES256(string encryptData, int index, out string result)
        //{
        //    try
        //    {
        //        string iv = encryptData.Substring(encryptData.Length - 16, 16);
        //        encryptData = encryptData.Substring(0, encryptData.Length - 16);
        //        var encryptBytes = Convert.FromBase64String(encryptData);
        //        var aes = new RijndaelManaged();
        //        aes.Key = Encoding.UTF8.GetBytes(Key);
        //        aes.IV = Encoding.UTF8.GetBytes(iv);
        //        aes.Mode = CipherMode.CBC;
        //        aes.Padding = PaddingMode.PKCS7;
        //        ICryptoTransform transform = aes.CreateDecryptor();
        //        result = Encoding.UTF8.GetString(transform.TransformFinalBlock(encryptBytes, 0, encryptBytes.Length));
        //        if (result.Split('@').Length > index)
        //        {
        //            result = result.Split('@')[index];
        //            return true;
        //        }
        //        else
        //        {
        //            result = "";
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = "解密失敗:" + ex.Message;
        //        return false;
        //    }
        //}

        /// <summary>
        /// QRCode 加密
        /// </summary>
        /// <param name="source">未加密前的QRCode</param>
        /// <returns> 加密後的QRCode </returns>
        public static string EncryptAES256(string source, KeyType kt = KeyType.AMRS)
        {
            if (kt == KeyType.AMRS)
                return EncryptAES256(source, Key);
            else if (kt == KeyType.IDNO)
                return EncryptAES256(source, IDNOKey);
            else
                return "";
        }

        public static string EncryptAES256(string source, string key)
        {
            string rtn = null;
            try
            {
                string iv = RandomString(14) + "==";

                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = Encoding.UTF8.GetBytes(iv);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform transform = aes.CreateEncryptor();

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                        {
                            cs.Write(sourceBytes, 0, sourceBytes.Length);
                            cs.FlushFinalBlock();
                            rtn = Convert.ToBase64String(ms.ToArray());
                            rtn += iv;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("加密失敗:" + ex.Message);
                rtn = "";
                
                return "加密失敗:" + ex.Message;
            }
            return rtn;
        }

        public static bool EncryptAES256(string source, string key, out string rtn)
        {
            rtn = "";
            try
            {
                string iv = RandomString(14) + "==";

                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);

                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = Encoding.UTF8.GetBytes(iv);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform transform = aes.CreateEncryptor();

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                        {
                            cs.Write(sourceBytes, 0, sourceBytes.Length);
                            cs.FlushFinalBlock();
                            rtn = Convert.ToBase64String(ms.ToArray());
                            rtn += iv;
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                //MessageBox.Show("加密失敗:" + ex.Message);
                logger.Error("加密失敗:" + ex.Message);
                return false;
            }

            return true;
        }

        private static string RandomString(int size)
        {
             StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                RNGCryptoServiceProvider rngp = new RNGCryptoServiceProvider();
                byte[] rb = new byte[4];
                rngp.GetBytes(rb);
                int value = BitConverter.ToInt32(rb, 0);
                if (value < 0) value = -value;
                if (value >= 26) value = value % 26;

                ch = Convert.ToChar(Convert.ToInt32(value + 65));
                builder.Append(ch);

                if (rngp != null) rngp.Dispose();
            }

            return builder.ToString();
        }

        /// <summary>
        /// 加密方式 SHA1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncryptSHA1(string value)
        {
            HashAlgorithm hash = null;
            string result = "";
            try
            {
                hash = SHA1.Create();

                //SHA256 hash = new SHA256CryptoServiceProvider();
                var combinded = Encoding.UTF8.GetBytes(value ?? "");
                result = BitConverter.ToString(hash.ComputeHash(combinded)).ToLower().Replace("-", "");
            }
            finally
            {
                if (hash != null)
                {
                    hash.Clear();
                    hash.Dispose();
                }
            }
            return result;
        }
    }

    public enum KeyType
    {
        AMRS,
        IDNO
    }
}
