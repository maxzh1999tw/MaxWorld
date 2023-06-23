using System.Security.Cryptography;
using System.Text;

namespace MaxWorld.Web.Utilities
{
    public static class Crypto
    {
        public static string Sha512Encode(string rawData)
        {
            //將輸入字串轉換為位元組並計算哈希數據   
            byte[] bytes = SHA512.HashData(Encoding.UTF8.GetBytes(rawData));

            // 將位元組轉換為字串
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
