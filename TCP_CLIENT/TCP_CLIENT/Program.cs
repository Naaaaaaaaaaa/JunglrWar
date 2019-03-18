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
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4567));
            
            byte[] data = new byte[1024];
            int count = clientSocket.Receive(data);
            string msg = System.Text.Encoding.UTF8.GetString(data, 0, count);
            Console.WriteLine(msg);

            while (true)
            {
               string s = Console.ReadLine();
               
               if (string.IsNullOrEmpty(s))
               {
                   continue;
               }

               if (s == "c")
               {
                   clientSocket.Close();
                   return;
               }

               clientSocket.Send(MyUtil.GetBytes(s));
            }
            
            //粘包：当一次性传送到服务器的数据太多时，tcp会把多条数据组合到一起变成一条数据
//            for (int i = 0; i < 100; i++)
//            {
//                clientSocket.Send(MyUtil.GetBytes(i.ToString()));
//            }

            //分包：当一个数据包太大的时候，tcp会把这条数据分成多个包
//            string s = @"匡乐局释热拉涵珙嘉了任可渐行黎杰荣刀太手里接过了森马过来看我健儿们立刻给我看
//匡乐局释热拉涵珙嘉了任可渐行黎杰荣刀太手里接过了森马过来看我健儿们立刻给我看
//匡乐局释热拉涵珙嘉了任可渐行黎杰荣刀太手里接过了森马过来看我健儿们立刻给我看";
//            clientSocket.Send(Encoding.UTF8.GetBytes(s));


//            Console.ReadKey();
//            clientSocket.Close();
        }
    }
}