using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCP_Server
{
    internal class Program
    {   
        /// <summary>
        /// 接收消息的容器
        /// </summary>
        static byte[] dataBuffer = new byte[1024];
        
        public static void Main(string[] args)
        {
            StartServerAsync();
            Console.ReadKey();
        }

        static void StartServerAsync()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            
            IPAddress ipAddress = IPAddress.Parse("192.168.0.108");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 1234);
            
            //注意！
            //客户端是socket.connect()  服务端是socket.bind();
            serverSocket.Bind(ipEndPoint);
            //backlog代表监听的客户端最大容量  0：无限制
            serverSocket.Listen(10);

//            //socket.accept()  接收连接过来的客户端， 会返回客户端socket(同步的方式)
//            Socket clientSocket = serverSocket.Accept();
//            
//            //向客户端发送消息
//            string msg = "hello client! connect to server success";
//            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
//            clientSocket.Send(data);

            //socket.beginAccept()是异步接收客户端信息
            serverSocket.BeginAccept(AcceptCallback, serverSocket);
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            Socket serverSocket = ar.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(ar);
            
            //向客户端发送消息
            string msg = "hello client! connect to server success";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
            clientSocket.Send(data);
                
            //socket.BeginReceive()是异步接收消息  socket.Receive()是同步接收消息
            clientSocket.BeginReceive(dataBuffer, 0, 1024,
                SocketFlags.None, ReceiveCallback, clientSocket);
            
            serverSocket.BeginAccept(AcceptCallback, serverSocket);
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar);
                //正常退出不关闭客户端会一直发空消息
                if (count == 0)
                {
                    clientSocket.Close();
                    return;
                }
                string msg = Encoding.UTF8.GetString(dataBuffer, 0, count);
                Console.WriteLine("接收到客户端的消息： " + msg);
                clientSocket.BeginReceive(dataBuffer, 0, 1024,
                    SocketFlags.None, ReceiveCallback, clientSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }
            }
        }

        static void StartServerSync()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            
            IPAddress ipAddress = IPAddress.Parse("192.168.2.177");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 1234);
            
            //注意！
            //客户端是socket.connect()  服务端是socket.bind();
            serverSocket.Bind(ipEndPoint);
            //backlog代表监听的客户端最大容量  0：无限制
            serverSocket.Listen(10);

            //socket.accept()  接收连接过来的客户端， 会返回客户端socket
            Socket clientSocket = serverSocket.Accept();
            
            //向客户端发送消息
            string msg = "hello client! connect to server success";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
            clientSocket.Send(data);
            
            //接收客户端的消息
            byte[] dataBuffer = new byte[1024];
            //会返回接收到下消息的长度
            int count = clientSocket.Receive(dataBuffer);
            string text = System.Text.Encoding.UTF8.GetString(dataBuffer, 0, count);
            Console.WriteLine(text);

            Console.ReadKey();
            serverSocket.Close();
            clientSocket.Close();
        }
    }
}