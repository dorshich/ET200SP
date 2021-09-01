using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace ET200SP
{
    class Client
    {
        /// The .net wrapper around WinSock sockets.
        TcpClient _client;
        /// Buffer to store incoming messages from the server.
        byte[] _buffer = new byte[46];


        public Client()
        {
            _client = new TcpClient();
        }
        public void connect()
        {
            // Connect to the remote server. The IP address and port # could be
            // picked up from a settings file.
            _client.Connect("192.168.60.241", 2000);

            // Start reading the socket and receive any incoming messages
            _client.GetStream().BeginRead(_buffer, 0, _buffer.Length, Server_MessageReceived, null);
        }
            
        public void Server_MessageReceived(IAsyncResult ar)
        {
            if (ar.IsCompleted)
            {
                // End the stream read
                var bytesIn = _client.GetStream().EndRead(ar);
                if (bytesIn > 0)
                {
                    // Create a string from the received data. For this server 
                    // our data is in the form of a simple string, but it could be
                    // binary data or a JSON object. Payload is your choice.
                    var tmp = new byte[bytesIn];
                    Array.Copy(_buffer, 0, tmp, 0, bytesIn);
                    var str = Encoding.ASCII.GetString(tmp );
                    Console.WriteLine("Recived form server: {0:G}\n",str);
                    // etPLC.plcWriteString(1, 16, "eyal");
                    Program.m_etPLC.plcWriteString(1, 14, str);
                }

                // Clear the buffer and start listening again
                Array.Clear(_buffer, 0, _buffer.Length);
                _client.GetStream().BeginRead(_buffer, 0, _buffer.Length, Server_MessageReceived, null);
            }
        }

        public void write()
        {
            // Encode the message and send it out to the server.
            var msg = Encoding.ASCII.GetBytes("Hello C++");
            _client.GetStream().Write(msg, 0, msg.Length);
        }
    
    }
}
