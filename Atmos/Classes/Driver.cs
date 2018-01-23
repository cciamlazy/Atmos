using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Atmos.Classes
{
    public enum LEDCommand
    {
        On,
        Off,
        Brightness,
        SetLED,
        SetAll
    }

    public static class Driver
    {
        static SerialPort currentPort;
        static bool portFound;

        static SerialPort mainPort;
        static string detectedPort;

        static bool initialized = false;
        static bool connected = false;

        public static bool Initialize()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    currentPort = new SerialPort(port, 9600);
                    if (DetectArduino())
                    {
                        portFound = true;
                        detectedPort = port;
                        break;
                    }
                    else
                    {
                        portFound = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error initializing: " + e.Message);
            }
            initialized = portFound;
            return portFound;
        }

        /// Automatic detection provided by Richard210363 and can be found on https://playground.arduino.cc/Csharp/SerialCommsCSharp
        private static bool DetectArduino()
        {
            try
            {
                //The below setting are for the Hello handshake
                byte[] buffer = new byte[5];
                buffer[0] = Convert.ToByte(16);
                buffer[1] = Convert.ToByte(128);
                buffer[2] = Convert.ToByte(0);
                buffer[3] = Convert.ToByte(0);
                buffer[4] = Convert.ToByte(4);
                int intReturnASCII = 0;
                char charReturnValue = (Char)intReturnASCII;
                currentPort.Open();
                currentPort.Write(buffer, 0, 5);
                Thread.Sleep(1000);
                int count = currentPort.BytesToRead;
                string returnMessage = "";
                while (count > 0)
                {
                    intReturnASCII = currentPort.ReadByte();
                    returnMessage += Convert.ToChar(intReturnASCII);
                    count--;
                }
                //ComPort.name = returnMessage;
                currentPort.Close();
                if (returnMessage.Contains("HELLO FROM ARDUINO"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error detecting (this is nothing to worry about): " + e.Message);
                return false;
            }
        }

        public static bool Connect()
        {
            if(!initialized)
                if (!Initialize())
                    return false;

            try
            {
                mainPort = new SerialPort(detectedPort, 9600);
                connected = true;
            }
            catch (Exception e)
            {
                connected = false;
                Console.WriteLine("Error connecting: " + e.Message);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer">First should be 8, second should be (int)LEDCommand</param>
        /// <returns></returns>
        public static async Task<string> SendData(byte[] buffer)
        {
            string returnMessage = "";
            if(!connected)
            {
                Console.WriteLine("Error: Not connected to Arduino");
                returnMessage = "Failed";
                return returnMessage;
            }

            try
            {
                if (!mainPort.IsOpen)
                    mainPort.Open();

                mainPort.Write(buffer, 0, buffer.Length);
                await Task.Delay(500);
                //Thread.Sleep(100);
                int count = currentPort.BytesToRead;
                while (count > 0)
                {
                    returnMessage += Convert.ToChar(currentPort.ReadByte());
                    count--;
                }
                mainPort.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to send data - " + buffer.ToString() + ": " + e.Message);
                returnMessage = "Failed";
            }

            return returnMessage;
        }

        /// <summary>
        /// Turn on and off the leds
        /// </summary>
        /// <param name="state">True = On, False = Off</param>
        /// <returns>If the process was sent successfully</returns>
        public static async Task<bool> ToggleLED(bool state)
        {
            byte[] buffer = new byte[2];
            buffer[0] = Convert.ToByte(8);
            buffer[1] = Convert.ToByte((int)(state ? LEDCommand.On : LEDCommand.Off));
            string message = await SendData(buffer);
            if (message == "Failed")
                return false;
            return true;
        }
    }
}
