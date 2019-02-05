using System;
using System.Text;
using System.IO.Ports;

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

        public static void SendCommand(string portName)
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
                        byte[] onCommand = new byte[] { 0x55, 0x04, 0xA0, 0x00, 0x07 };
                        byte[] offCommand = new byte[] { 0x55, 0x04, 0xA1, 0x00, 0x06 };
                    //for (int i = 0; i < 2; i++)
                    //{
                        Console.WriteLine($"Send on Command");
                        //System.Threading.Thread.Sleep(500);
                        sp.Write(onCommand, 0, onCommand.Length);

                        System.Threading.Thread.Sleep(1000);
                        Console.WriteLine($"Send off Command");
                        //System.Threading.Thread.Sleep(500);
                        sp.Write(offCommand, 0, offCommand.Length);
                        System.Threading.Thread.Sleep(1000);
                    //}


                    //iter++;
                    //break;
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

    }
}
