using System.IO.Ports;
using System.Management;

//Disable all warnings here
#pragma warning disable 1416, 8600, 8603, 8604

namespace CMH_Lahore.Classes
{
    class GSMComPort
    {
        public string Port { get; set; }
        public string Vendor { get; set; }

        public GSMComPort(string _Port, string _Vendor)
        {
            Port = _Port;
            Vendor = _Vendor;
        }

        override
        public string ToString()
        {
            return $"{Port} = {Vendor} ";
        }
    }
    class Unit
    {
        private SerialPort Port = null;

        public Unit()
        {
            Port = new SerialPort();
        }

        public GSMComPort Getportfromos()
        {
            List<GSMComPort> GSMComPort = new List<GSMComPort>();
            ConnectionOptions options = new();
            {
                options.Impersonation = ImpersonationLevel.Impersonate;
                options.EnablePrivileges = true;
            }

            string connString = $@"\\{Environment.MachineName}\root\cimv2";
            ManagementScope scope = new(connString, options);
            scope.Connect();

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_POTSModem");
            ManagementObjectSearcher search = new(scope, query);
            ManagementObjectCollection collection = search.Get();

            foreach (ManagementObject obj in collection)
            {
                string portName = obj["AttachedTo"].ToString();
                string portDescription = obj["Description"].ToString();

                if (portName != "" && portDescription != "")
                {
                    GSMComPort com = new GSMComPort(portName, portDescription);
                    GSMComPort.Add(com);
                }
            }
            if (GSMComPort.Count > 0)
            {
                return GSMComPort[0];
            }
            return null;
        }

        public bool Connetionstatus()
        {
            if (Port.IsOpen)
            {
                return true;
            }
            return false;
        }

        public bool Connect()
        {
            if (!Port.IsOpen)
            {
                GSMComPort com = Getportfromos();
                if (com != null)
                {
                    try
                    {
                        Port.PortName = com.Port;
                        Port.NewLine = Environment.NewLine;
                        Port.Open();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            return Port.IsOpen;
        }

        public void Disconnect()
        {
            if (Port.IsOpen)
            {
                Port.Close();
                Port.Dispose();
            }
        }

        public void Read()
        {
            Console.WriteLine("Reading..");

            Port.WriteLine("AT+ CMGF=1"); // Set mode to Text(1) or PDU(0)
            Thread.Sleep(1000); // Give a second to write
            Port.WriteLine("AT+ CPMS=\"SM\""); // Set storage to SIM(SM)
            Thread.Sleep(1000);
            Port.WriteLine("AT+ CMGL=\"ALL\""); // What category to read ALL, REC READ, or REC UNREAD
            Thread.Sleep(1000);
            Port.Write("\r");
            Thread.Sleep(1000);

            string response = Port.ReadExisting();

            if (response.EndsWith("\r\nOK\r\n"))
            {
                Console.WriteLine(response);
                // add more code here to manipulate reponse string.
            }
            else
            {
                // add more code here to handle error.
                Console.WriteLine(response);
            }

        }

        public void Send(string receiver, string message)
        {
            Console.WriteLine("Sending..");

            Port.WriteLine("AT+ CMGF=1"); // Set mode to Text(1) or PDU(0)
            Thread.Sleep(1000);
            Port.WriteLine($"AT+ CMGS=\"{receiver}\"");
            Thread.Sleep(1000);
            Port.WriteLine(message + char.ConvertFromUtf32(26));
            Thread.Sleep(5000);

            string response = Port.ReadExisting();

            if (response.EndsWith("\r\nOK\r\n") && response.Contains("+CMGS:")) // IF CMGS IS MISSING IT MEANS THE MESSAGE WAS NOT SENT!
            {
                Console.WriteLine(response);
                // add more code here to manipulate reponse string.
            }
            else
            {
                // add more code here to handle error.
                Console.WriteLine(response);
            }
        }
        //static void Main(string[] args)
        //{
        //    Unit unit = new Unit();
        //    unit.Connect();
        //    unit.Send("+923174120910", "Hello World!");
        //    unit.Disconnect();
        //}
    }
}
