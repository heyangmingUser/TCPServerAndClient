using Amqp.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i<=10; i++) 
            {
                Client();
            }
        }
        /// <summary>
        /// Tcp数据传输客户端
        /// </summary>
        public static void Client() 
        {
            
            string result = string.Empty;
            //什么是Socket,是计算机之前进行通信的一种约定或方式
            //客户端指定计算机之间通信的协议,套接字类型规定了可以使用什么样的协议，寻址方案
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //通过Ip连接
            clientSocket.Connect("192.168.1.186", 2000);
            //发送的数据
            string senddata = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + new Random((int)DateTime.Now.Ticks).Next().ToString();
            clientSocket.Send(Encoding.Default.GetBytes(senddata));
            Console.WriteLine("我告诉服务器:" + senddata);
            clientSocket.ReceiveTimeout = 3000;
            List<byte> data = new List<byte>();
            byte[] buffer = new byte[1024];
            int length = 0;
            try
            {
                while ((length = clientSocket.Receive(buffer)) > 0)
                {
                    for (int j = 0; j < length; j++)
                    {
                        data.Add(buffer[j]);
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
                result = Encoding.Default.GetString(data.ToArray(), 0, data.Count);
            }
            Console.WriteLine("服务器对我说：" + result);
        }
    }
}
