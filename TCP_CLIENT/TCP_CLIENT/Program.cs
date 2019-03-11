using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCP_CLIENT
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            //注意！
            //客户端是socket.connect()  服务端是socket.bind();
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.2.177"), 1234));
            
            byte[] data = new byte[1024];
            int count = clientSocket.Receive(data);
            string msg = System.Text.Encoding.UTF8.GetString(data, 0, count);
            Console.WriteLine(msg);

            while (true)
            {
               string s = Console.ReadLine();
               if (s == "c")
               {
                   clientSocket.Close();
                   return;
               }

               clientSocket.Send(Encoding.UTF8.GetBytes(s));
            }

//            Console.ReadKey();
//            clientSocket.Close();
        }
    }
}