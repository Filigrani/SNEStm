using HidApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            if (HIDDevice != null)
            {
                if (GamePadsManager.HasChange())
                {
                    try
                    {
                        ReadOnlySpan<byte> SendData = GamePadsManager.GetInputs();

                        // Use Write instead of SendFeatureReport
                        HIDDevice.Write(SendData);

                        ReadOnlySpan<byte> Data = HIDDevice.Read(SendData.Length);
                        if (Data.Length > 0)
                        {
                            s_DebugText = $"Sent {BitConverter.ToString(SendData.ToArray())}\nSTM  {BitConverter.ToString(Data.ToArray())}";
                        }
                    }
                    catch (HidApi.HidException ex)
                    {
                        Console.WriteLine($"HID Error: {ex.Message}");
                        HIDDevice = null;
                    }
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
                if(deviceInfo.VendorId == 0xcafe)
                {
                    HIDDevice = new Device(deviceInfo.Path);
                    HIDDevice.SetNonBlocking(false);
                    s_Connected = true;
                    break;
                }
            }
        }
    }
}
