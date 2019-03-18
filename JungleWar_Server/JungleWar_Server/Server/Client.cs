using System;
using System.Net.Sockets;
using JungleWar_Server.Tools;

namespace JungleWar_Server.Server
{
    public class Client
    {
        private Socket _ClientSocket;
        private MyServer _Server;
        private Message Msg = new Message();
        
        public Client()
        {
        }

        public Client(Socket client, MyServer server)
        {
            _ClientSocket = client;
            _Server = server;
        }

        public void Start()
        {
            if(_ClientSocket == null || _ClientSocket.Connected == false) return;
            _ClientSocket.BeginReceive(Msg.Data, Msg.StartIndex, Msg.RemainIndex, SocketFlags.None, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                if (_ClientSocket == null || _ClientSocket.Connected == false) return;
                int count = _ClientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }
                //TODO 处理接收到的消息
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }
        }

        /// <summary>
        /// 关闭客户端连接
        /// </summary>
        public void Close()
        {
            if(_ClientSocket != null) _ClientSocket.Close();
            _Server.RemoveClient(this);
        }
    }
}