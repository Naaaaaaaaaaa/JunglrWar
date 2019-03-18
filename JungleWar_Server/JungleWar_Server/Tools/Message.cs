using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;


namespace JungleWar_Server.Tools
{
    public class Message
    {
        private byte[] data = new byte[1024];
        private int startIndex = 0;//我们存取了多少字节的数据在data数组内
 
//        public void AddCount(int count)
//        {
//            startIndex += count;
//        }
 
        public byte[] Data
        {
            get { return data; }
        }
 
        public int StartIndex
        {
            get { return startIndex; }
        }
 
        public int RemainIndex
        {
            get { return data.Length - startIndex; }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        public void ReadData(int newDataAmount)
        {
            startIndex += newDataAmount;
            
            while (true)
            {
                if (startIndex <= 4) return; //如果接收到的消息长度小于4，说明接收到的消息不完整，连前四位的长度都没有接收完整
                int count = BitConverter.ToInt32(data, 0);
                if ((startIndex -4) >= count)//存取的数据长度大于等于一条完整信息的长度
                {
                    string s = Encoding.UTF8.GetString(data, 4, count);
                    Console.WriteLine("解析完成一条数据： " + s);
                    
                    //解析完数据，移除没用的数据
                    Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                    startIndex -= (count + 4);
                }
                else
                {
                    break;
                }
            }
        }

        public static byte[] PackData(string data)
        {
            byte[] sendData = Encoding.UTF8.GetBytes(data);
            int length = sendData.Length;
            byte[] lengthData = BitConverter.GetBytes(length);
            byte[] neWBytes = lengthData.Concat(sendData).ToArray();
            return neWBytes;
        }
    }
}