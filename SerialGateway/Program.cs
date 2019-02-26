using System;
using System.IO.Ports;
using System.Text;

namespace SerialGateway
{
    class Program
    {
        static void Main()
        {
            //SerialHelper.GetAvailablePorts();
            //var onCommandZone0 = new byte[] { 0x55, 0x04, 0xA0, 0x00, 0x07 };


            SerialHelper.DoSomething("COM6");
            //SerialHelper.DoSomething("/dev/ttyUSB0");
            TcpHelper.StartServer(5678);
            TcpHelper.Listen();
        }
    }
}
