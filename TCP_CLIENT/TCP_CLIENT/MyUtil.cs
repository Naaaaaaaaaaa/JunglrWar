using System;
using System.Linq;
using System.Text;

namespace TCP_CLIENT
{
    public class MyUtil
    {
        /// <summary>
        /// 在传到服务器的数据前加上数据长度，占四位
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int count = dataBytes.Length;
            byte[] countBytes = BitConverter.GetBytes(count);
            byte[] newByte = countBytes.Concat(dataBytes).ToArray();
            return newByte;
        }
    }
}