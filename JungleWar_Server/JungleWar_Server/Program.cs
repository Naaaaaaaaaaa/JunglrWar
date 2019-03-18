using System;
using JungleWar_Server.Server;

namespace JungleWar_Server
{
    internal class Program
    {
        /// <summary>
        /// 服务器
        /// </summary>
        private static MyServer _Server;
        public static void Main(string[] args)
        {
           _Server = new MyServer();
           _Server.StartServer();

           Console.ReadKey();
        }
    }
}