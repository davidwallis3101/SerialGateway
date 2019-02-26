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
                int iter = 0;
                bool finished = false;

                while (!finished)
                {


                    //try
                    //{
                    //    sp.WriteLine(line);
                    //}
                    //catch (TimeoutException)
                    //{
                    //    Console.WriteLine("ERROR: Sending command timed out");
                    //}

                    //if (iter == 0)
                    //{
                    // Console.WriteLine("11 receive, send command");




                    //byte[] onCommandZone0 = new byte[] { 0x55, 0x04, 0xA0, 0x00, 0x07 };
                    //// start len  selsrc  one  src  crc
                    //byte[] selectSource1Zone1 = new byte[] { 0x55, 0x05, 0xA3, 0x01, 0x01, 0x02 };

                    //byte[] onCommandZone2 = new byte[] { 0x55, 0x04, 0xA0, 0x01, 0x06 };
                    //byte[] selectSource2Zone2 = new byte[] { 0x55, 0x05, 0xA3, 0x01, 0x01, 0x02 };

                    //byte[] offCommandZone1 = new byte[] { 0x55, 0x04, 0xA1, 0x00, 0x06 };
                    //SendCommand(sp, onCommandZone0);


                    int numZones = 6;

                    for (int i = 0; i < numZones; i++)
                    {
                        SendOnCommand(sp, i);
                    }

                    for (int i = 0; i < numZones; i++)
                    {
                        SendSelectSourceCommand(sp, i, i);
                    }





                    //Console.WriteLine($"Send on Command");
                    //    //System.Threading.Thread.Sleep(500);
                    //    sp.Write(onCommandZone2, 0, onCommandZone2.Length);






                    //System.Threading.Thread.Sleep(1000);
                    //Console.WriteLine($"Send off Command");

                    //System.Threading.Thread.Sleep(500);
                    //sp.Write(offCommandZone1, 0, offCommandZone1.Length);
                    //System.Threading.Thread.Sleep(1000);
                    //}


                    //iter++;
                    break;
                    //}

                    //if (finished)
                    //    break;

                    // if RATE is set to really high Arduino may fail to respond in time
                    // then on the next command you might get an old message
                    //ReadExisting will read everything from the internal buffer
                    //var existingData = sp.ReadExisting();
                    //Console.Write($"Existing Data: {existingData}\r\n");


                    //if (!existingData.Contains('\n') && !existingData.Contains('\r'))
                    //{
                    // we didn't get the response yet, let's wait for it then
                    //try
                    //{
                    //    int length = sp.BytesToRead;
                    //    byte[] buf = new byte[length];
                    //    sp.Read(buf, 0, length);
                    //    if (length > 0)
                    //    {
                    //        //Console.WriteLine("Received Data: " + BitConverter.ToString(buf, 0, buf.Length));

                    //        if (buf[0] == 0x11)
                    //        {
                    //            Console.WriteLine("11 receive, send command");
                    //            byte[] command = new byte[] { 0x55, 0x04, 0xA0, 0x00, 0x07 };
                    //            sp.Write(command, 0, command.Length);
                    //            // sp.Write(command, 0, command.Length);

                    //            int respLength = sp.BytesToRead;
                    //            byte[] respBuf = new byte[respLength];
                    //            sp.Read(respBuf, 0, respLength);

                    //            Console.WriteLine("Resp Data: " + BitConverter.ToString(respBuf, 0, respBuf.Length));

                    //            break;
                    //        }
                    //    }


                    //    //Console.WriteLine($"ReadLine: {sp.ReadLine()}");
                    //}
                    //catch (TimeoutException)
                    //{
                    //    Console.WriteLine($"ERROR: No response in {sp.ReadTimeout}ms.");
                    //}
                }
            }
        }

        private static void SendOnCommand(SerialPort sp, int Zone)
        {
            Console.WriteLine($"Send On Command Zone {Zone}");
                                               // start len  oncmd  zne   crc
            byte[] onCommand = new byte[] { 0x55, 0x04, 0xA0, (byte)Zone };

           SendCommand(sp, GenerateCRC(onCommand));
        }

        private static void SendSelectSourceCommand(SerialPort sp, int Zone, int Source)
        {
            Console.WriteLine($"Send Select Source Command {Zone}");
            // start len  oncmd  zne   crc
            byte[] sourceCommand = new byte[] { 0x55, 0x05, 0xA3, (byte)Zone , (byte)Source };

            SendCommand(sp, GenerateCRC(sourceCommand));
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
                System.Threading.Thread.Sleep(500);
                sp.Write(command, 0, command.Length);
                int length = sp.BytesToRead;
                byte[] buf = new byte[length];
                sp.Read(buf, 0, length);
                if (length > 4) // Enough to get the ACK
                {
                    //Console.WriteLine($"Len: {length} Received Data: {BitConverter.ToString(buf)}");

                    if (buf[2] == 0x95 && buf[4] == 0x01)
                    {
                        Console.WriteLine("ACK resp");
                        break;
                    }

                }
            }
        }
    }
}
