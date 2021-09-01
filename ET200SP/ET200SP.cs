using S7.Net;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;

namespace ET200SP
{
    class ET200SP
    {
        private Plc m_plc;
        private string m_plcIP;
        public ET200SP(string plcIP)
        {
            m_plcIP = plcIP;
            m_plc = new Plc(CpuType.S71500, plcIP, 0, 1);           
        }
        public const string barcodeAddress = "xxx.xxx.xxx.xxx";
        public bool plcConnect()
        {
           
            try
            {
                m_plc.Open();
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            if (!m_plc.IsConnected)
            {
                return false;
            }
            return true;
        }
        public bool plcRead(string address)
        {
            bool iRes =false;
 
            while (!iRes)
            {
                Thread.Sleep(200);
                var res = m_plc.Read(address);
                iRes = (bool)res;
            }

            Console.WriteLine("Address %s contain value: %d", address, iRes);
            
            return true;
    }
        public bool plcWriteString(int DBnumber, int offset, string value)
        {
            byte[] byteArr = ToByteArray(value);
            m_plc.WriteBytes(DataType.DataBlock, 1, 16, byteArr);
            int err = Marshal.GetLastWin32Error();
            if (err > 0)
                Console.WriteLine("Write string failed. error code: %d", err);
            else
                Console.WriteLine("Write string successfull");
            return true;
        }
        private byte[] ToByteArray(string value)
        {
            char[] charArr = value.ToCharArray();
            byte[] byteArr = new byte[2 + value.Length];
            byteArr[0] = byteArr[1] = (byte)value.Length;
            for (int i = 0; i < charArr.Length; i++)
            {
                byteArr[i + 2] = (byte)Asc(charArr[i].ToString());
            }
            return byteArr;
        }

        private int Asc(string s)
        {
            byte[] b = System.Text.Encoding.ASCII.GetBytes(s);
            if (b.Length > 0)
                return b[0];
            return 0;
        }
        public bool write()
        {
            m_plc.Write("DB1.DBX0.0", false);
            return true;
        }

    }

}
