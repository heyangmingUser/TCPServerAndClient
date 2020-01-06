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
            //什么是Socket,是计算机之前进行通信的一种约定或方式
            //客户端指定计算机之间通信的协议,套接字类型规定了可以使用什么样的协议，寻址方案
            Socket skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //将套接字绑定IP地址
            skt.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.212"), 2000));//前面的局域网内ip,后面是端口号，随便起，在本机不被占用就行
            //套接字监听，参数挂起套接字最大的长度
            skt.Listen(100);
            while (true)//不断轮询，接收客户端发送的信息
            {
                Socket acceptskt = skt.Accept();//接收客户端信息
                acceptskt.ReceiveTimeout = 3000;//接收超时时间
                List<byte> data = new List<byte>();
                byte[] buffer = new byte[1024];
                int length = 0;
                try
                {
                    //接收到的消息，来自客户端
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
                //服务端发送给客户端的信息
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
