using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            skt.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.186"), 2000));//前面的局域网内ip,后面是端口号，随便起，在本机不被占用就行
            skt.Listen(100);
            while (true)//不断轮询，接收客户端发送的信息
            {
                Socket acceptskt = skt.Accept();//接收客户端信息
                acceptskt.ReceiveTimeout = 3000;//
                List<byte> data = new List<byte>();
                byte[] buffer = new byte[1024];
                int length = 0;
                try
                {
                    while ((length = acceptskt.Receive(buffer)) > 0)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            data.Add(buffer[i]);
                        }
                        if (length < buffer.Length)
                        {
                            break;
                        }
                    }
                }
                catch { }
                if (data.Count > 0)
                {
                    Console.WriteLine(Encoding.Default.GetString(data.ToArray(), 0, data.Count));
                }
                acceptskt.Send(Encoding.Default.GetBytes("我收到你的信了"));
                if (acceptskt.Connected)
                {
                    acceptskt.Shutdown(SocketShutdown.Both);
                }
                acceptskt.Close();

            }
        }
    }
}
