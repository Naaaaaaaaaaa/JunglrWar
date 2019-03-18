using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using JungleWar_Server.Tools;

namespace JungleWar_Server.Server
{
    public class MyServer
    {
        /// <summary>
        /// 服务器socket
        /// </summary>
        private Socket _ServerSocket = null;
        /// <summary>
        /// 客户端列表
        /// </summary>
        private List<Client> _ClientList = new List<Client>();
        /// <summary>
        /// IP地址
        /// </summary>
        private string _IP = "127.0.0.1";
        /// <summary>
        /// 端口地址
        /// </summary>
        private int _Port = 4567;
    
        private IPAddress _IpAddress;
        private IPEndPoint _IpEndPoint;
    
        //消息字符串
        private string msg = "";
        /// <summary>
        /// 发送消息容器
        /// </summary>
        private byte[] data = new byte[1024];
        /// <summary>
        /// 接收消息容器
        /// </summary>
        private byte[] dataBuffer = new byte[1024];

        private Message Msg = new Message();
    
        public MyServer()
        {
            InitMyServer();
        }
    
        public MyServer(string ip, int port)
        {
            _IP = ip;
            _Port = port;
            
            InitMyServer();
        }
    
        void InitMyServer()
        {
            _ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    
            _IpAddress = IPAddress.Parse(_IP);
            _IpEndPoint = new IPEndPoint(_IpAddress, _Port);
        }
    
        /// <summary>
        /// 开启服务器
        /// </summary>
        public void StartServer()
        {
            _ServerSocket.Bind(_IpEndPoint);
            _ServerSocket.Listen(10);
            Console.WriteLine("开启服务器成功");
            
            BeginAccept();
        }
    
        /// <summary>
        /// 开始接收客户端socket
        /// </summary>
        public void BeginAccept()
        {
            Console.WriteLine("开始接收消息");
            _ServerSocket.BeginAccept(AcceptCallback, _ServerSocket);
        }
    
        /// <summary>
        /// 接收socket回调
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket serverSocket = ar.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            _ClientList.Add(client);
            
            //向客户端发送连接成功消息
            msg = "connect to server success";
            data = System.Text.Encoding.UTF8.GetBytes(msg);
            clientSocket.Send(data);
            
            //开始接收客户端socket发送的消息
            BeginReceive(serverSocket, clientSocket);
        }
    
        /// <summary>
        /// 开始接收客户端消息
        /// </summary>
        /// <param name="serverSocket">服务端</param>
        /// <param name="clientSocket">客户端</param>
        private void BeginReceive(Socket serverSocket, Socket clientSocket)
        {
            //socket.BeginReceive()是异步接收消息  socket.Receive()是同步接收消息
            clientSocket.BeginReceive(dataBuffer, 0, 1024,
                SocketFlags.None, ReceiveCallback, clientSocket);
            
            //再次开启接收socket；
            serverSocket.BeginAccept(AcceptCallback, serverSocket);
        }
    
        /// <summary>
        /// 接收消息回调
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
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
    
        /// <summary>
        /// 关闭服务器
        /// </summary>
        public void CloseServer()
        {
            _ServerSocket.Close();
        }
    
        /// <summary>
        /// 移除连接中断的client
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(Client client)
        {
            lock (_ClientList)
            {
                _ClientList.Remove(client);
    
            }
        }
    }
}