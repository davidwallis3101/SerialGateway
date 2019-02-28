using System;
using System.Text;
using System.IO.Ports;
using System.Linq;

namespace SerialGateway
{
    class SerialHelper
    {
        public static void GetAvailablePorts()
        {
            var ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                Console.WriteLine(port);
            }
        }

        public static void DoSomething(string portName)
        {
            using (var sp = new SerialPort(portName))
            {
                sp.Encoding = Encoding.UTF8;
                sp.BaudRate = 57600;
                sp.Handshake = Handshake.XOnXOff;
                //sp.Handshake = Handshake.None;
                sp.DataBits = 8;
                sp.Parity = Parity.None;
                sp.StopBits = StopBits.One;

                //sp.ReadTimeout = 1000;
                //sp.WriteTimeout = 1000;
                sp.Open();

                    int numZones = 6;

                for (int i = 0; i < numZones; i++)
                {
                    SendOffCommand(sp, i);
                }

                for (int i = 0; i < numZones; i++)
                {
                    SendOnCommand(sp, i);
                }

                for (int i = 0; i < numZones; i++)
                {
                    SendSelectSourceCommand(sp, i, i);
                }
                  
 
            }
        }

        private static void SendOffCommand(SerialPort sp, int Zone)
        {
            Console.WriteLine($"Send Off Command Zone {Zone}");
            SendCommand(sp, GenerateCRC(new byte[] { 0x55, 0x04, 0xA1, (byte)Zone }));
        }


        private static void SendOnCommand(SerialPort sp, int Zone)
        {
            Console.WriteLine($"Send On Command Zone {Zone}");
           SendCommand(sp, GenerateCRC(new byte[] { 0x55, 0x04, 0xA0, (byte)Zone }));
        }

        private static void SendSelectSourceCommand(SerialPort sp, int Zone, int Source)
        {
            Console.WriteLine($"Send Select Source Command {Zone}");
            SendCommand(sp, GenerateCRC(new byte[] { 0x55, 0x05, 0xA3, (byte)Zone, (byte)Source }));
        }

        public static byte[] GenerateCRC(byte[] command)
        {
            var total = command.Aggregate(default(byte), (current, b) => (byte)(current + b));

            var crc = (byte)(256 - total);

            // Append CRC byte to command
            var commandWithCRC = new byte[command.Length + 1];
            command.CopyTo(commandWithCRC, 0);
            commandWithCRC[commandWithCRC.Length - 1] = crc;

            return commandWithCRC;
        }


        private static void SendCommand(SerialPort sp, byte[] command)
        {
            int retryCount = 10;
            for (int i = 0; i < retryCount; i++)
            {
                Console.WriteLine($"Sending Command: {BitConverter.ToString(command)}");
                System.Threading.Thread.Sleep(250);
                sp.Write(command, 0, command.Length);
                int length = sp.BytesToRead;
                byte[] buf = new byte[length];
                sp.Read(buf, 0, length);
                Console.WriteLine($"Length {length} CommandLen: {command.Length}");
                Console.WriteLine(BitConverter.ToString(buf));
                if (length > 3)
                {
                    if (buf[2] == 0x95 && buf[4] == 0x01)
                    {
                        Console.WriteLine("ACK resp");
                        break;
                    }
                    else
                    {
                        // err
                        Console.WriteLine(BitConverter.ToString(buf));
                    }
                }
            }
        }
    }
}
