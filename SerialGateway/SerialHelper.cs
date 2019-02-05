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
                sp.DataBits = 8;
                sp.Parity = Parity.None;
                sp.StopBits = StopBits.One;

                sp.ReadTimeout = 1000;
                sp.WriteTimeout = 1000;
                sp.Open();

                bool finished = false;

                while (!finished)
                {
                    //string line = Console.ReadLine();
                    //if (line == "!q")
                    //    break;

                    //try
                    //{
                    //    sp.WriteLine(line);
                    //}
                    //catch (TimeoutException)
                    //{
                    //    Console.WriteLine("ERROR: Sending command timed out");
                    //}

                    if (finished)
                        break;

                    // if RATE is set to really high Arduino may fail to respond in time
                    // then on the next command you might get an old message
                    // ReadExisting will read everything from the internal buffer
                    var existingData = sp.ReadExisting();
                    Console.Write($"Existing Data: {existingData}");


                    //if (!existingData.Contains('\n') && !existingData.Contains('\r'))
                    //{
                        // we didn't get the response yet, let's wait for it then
                        try
                        {
                            int length = sp.BytesToRead;
                            byte[] buf = new byte[length];

                            sp.Read(buf, 0, length);
                            Console.WriteLine("Received Data: " + buf);
                            Console.WriteLine("Received Data 2: " + BitConverter.ToString(buf, 0, buf.Length));

                            //Console.WriteLine($"ReadLine: {sp.ReadLine()}");
                        }
                        catch (TimeoutException)
                        {
                            Console.WriteLine($"ERROR: No response in {sp.ReadTimeout}ms.");
                        }
                    //}
                }
            }
        }

    }
}
