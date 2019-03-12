# JunglrWar

1，同步方式
	Accept Connect
	Receive
2，异步方式
	BeginAccept EndAccept Connect
	BeginReceive EndReceive


1，字符串转成字节数组 System.Text.Encoding.UTF8.GetBytes(GetString)
2，Int32转成字节数组BitConverter.GetBytes(ToXX)


粘包：当一次性传送到服务器的数据太多时，tcp会把多条数据组合到一起变成一条数据
            for (int i = 0; i < 100; i++)
            {
                clientSocket.Send(Encoding.UTF8.GetBytes(i.ToString()));
            }
