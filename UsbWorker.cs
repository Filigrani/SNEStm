using HidApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNEStm
{
    public static class UsbWorker
    {
        public static Device HIDDevice = null;
        private static bool s_Connected = false;
        public static string s_DebugText = string.Empty;
        
        
        public static void Update()
        {
            if(HIDDevice != null)
            {
                HIDDevice.SendFeatureReport(GamePadsManager.GetInputs());
                ReadOnlySpan<byte> Data = HIDDevice.Read(64);
                if(Data.Length > 0)
                {
                    s_DebugText = BitConverter.ToString(Data.ToArray());
                }
            }
            else
            {
                Connect();
            }
        }


        public static void Connect()
        {
            foreach (var deviceInfo in Hid.Enumerate())
            {
                using var device = deviceInfo.ConnectToDevice();

                if(deviceInfo.VendorId == 0xcafe)
                {
                    HIDDevice = device;
                    HIDDevice.SetNonBlocking(false);
                    s_Connected = true;
                    break;
                }
            }
        }
    }
}
