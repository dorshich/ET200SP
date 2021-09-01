using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ET200SP
{
    static class Program
    {
        public static ET200SP m_etPLC;
        static void Main(string[] args)
        {
            bool resualt = false;
            string ip = "192.168.60.213";
            m_etPLC = new ET200SP(ip);
            Client client = new Client();
            client.connect();
            do
            {
                if (m_etPLC.plcConnect())
                {
                    Console.WriteLine("Connection established successfully");
                    resualt = m_etPLC.plcRead("DB1.DBX0.0");
                    
                }
                else
                    Console.WriteLine("Connection failed!");
              
                if (resualt == true)
                {
                    m_etPLC.write();
                    client.write();
                }
            } while (m_etPLC.plcConnect());
         
        }
    }
}
    
