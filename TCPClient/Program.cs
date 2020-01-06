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
            clientSocket.Connect("192.168.1.212", 2000);
            //模拟的数据
            string senddata = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + new Random((int)DateTime.Now.Ticks).Next().ToString();
            //发送数据
            clientSocket.Send(Encoding.Default.GetBytes(senddata));
            Console.WriteLine("我告诉服务器:" + senddata);
            //接收超时时间
            clientSocket.ReceiveTimeout = 3000;
            //list集合
            List<byte> data = new List<byte>();
            //创建缓存区,
            byte[] buffer = new byte[1024];
            int length = 0;
            try
            {
                //接收到的字节长度,来自服务器
                while ((length = clientSocket.Receive(buffer)) > 0)
                {
                    //当字节长度小于总的长度时，将他添加到集合中
                    for (int j = 0; j < length; j++)
                    {

                        data.Add(buffer[j]);
                    }
                    //当长度小于缓存的长度时，跳出循环
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
