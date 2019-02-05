using System;
using System.IO.Ports;

namespace SerialGateway
{
    class Program
    {
        static void Main()
        {
            //SerialHelper.GetAvailablePorts();
            SerialHelper.SendCommand("/dev/ttyUSB0");
            TcpHelper.StartServer(5678);
            TcpHelper.Listen();
        }
    }
}
