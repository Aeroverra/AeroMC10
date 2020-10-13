using Binarysharp.MemoryManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AeroMC10
{
    class Program
    {

        static Process process;
        static VAMemory vam;
        static IntPtr baseAddress;
        static void Main(string[] args)
        {
            Console.Title = "AeroMC10 by Aeroverra";
            Console.WriteLine("Welcome to AeroMC10 by Aeroverra");

            process = Process.GetProcessesByName("Minecraft.Windows").FirstOrDefault();
            vam = new VAMemory("Minecraft.Windows");
            Console.WriteLine($"Minecraft Found!");
            baseAddress = process.MainModule.BaseAddress;
            Console.WriteLine();
            while (true)
            {
                var output = "";
                Console.WriteLine("--- Menu ---");
                Console.WriteLine("[1] Toggle InstaBreak");
                Console.WriteLine("[2] Toggle Fly");
                Console.WriteLine("[3] Set Anvil Cost To 1");
                var input = Console.ReadLine();
                int option = 0;
                int.TryParse(input, out option);
                switch (option)
                {
                    case 0:
                        output = "Invalid Input";
                        break;
                    case 1:
                        ToggleInstaBreak();
                        output = "InstaBreak Toggled";
                        break;
                    case 2:
                        ToggleFly();
                        output = "Fly Toggled";
                        break;
                    case 3:
                        SetAnvilCost();
                        output = "Anvil Cost Set To 1.";
                        break;
                }
                Console.Clear();
                Console.WriteLine(output);
                Console.WriteLine();
            }






        }
        static bool AnvilIsOn = false;
        public static void SetAnvilCost()
        {
            if (AnvilIsOn)
            {
                AnvilIsOn = false;
                return;
            }
            int staticPointerOffset = 0x036A0278;
            int[] offsets = new int[] { 0x0, 0x18, 0x80, 0x5A8, 0x50, 0x20, 0xA8 };


            Console.WriteLine("Setting Anvil Cost to 1 for 20 seconds. You may need to click the side of the interface to update the price.");
            for (int x = 0; x <= 40; x++)
            {
                var pointer = FindPointer(baseAddress, staticPointerOffset, offsets);
                var value = vam.ReadInt32(pointer);
                vam.WriteInt32(pointer, 1);
                Thread.Sleep(500);
            }
        }




        public static void ToggleFly()
        {

            int staticPointerOffset = 0x03655728;
            int[] offsets = new int[] { 0xA8, 0x20, 0x48, 0x10, 0x58, 0x0, 0x8B8 };

            var pointer = FindPointer(baseAddress, staticPointerOffset, offsets);

            while (true)
            {
                var p8v2 = vam.ReadInt32(pointer);
                Console.WriteLine($"p8v: {p8v2}");
                vam.WriteInt32(pointer, 1);
                Thread.Sleep(100);
            }


        }


        public static void ToggleInstaBreak()
        {
            int staticPointerOffset = 0x3683F48;
            int[] offsets = new int[] { 0xD0, 0x58, 0xB8, 0x158, 0x140, 0x1C84 };

            var pointer = FindPointer(baseAddress, staticPointerOffset, offsets);

            var value = vam.ReadInt32(pointer);
            if (value == 1)
            {
                vam.WriteInt32(pointer, 0);
            }
            else
            {
                vam.WriteInt32(pointer, 1);
            }

        }
        public static IntPtr FindPointer(IntPtr applicationBaseAddress, int baseAddressOffset, int[] offsets)
        {
            //Get the start address by adding the static offset to the base minecraft memory adress
            var pointer = IntPtr.Add(applicationBaseAddress, baseAddressOffset);

            //Read the first value
            pointer = (IntPtr)vam.ReadInt64(pointer);

            //Itterate through the offsets and add them to the values read to get the next one
            for (int x = 0; x < offsets.Count(); x++)
            {
                var offset = offsets[x];
                pointer = IntPtr.Add(pointer, offset);

                //Only set this if its not the last offset. the last offset is the value we are looking to manipulate
                if (x != offsets.Count() - 1)
                {
                    pointer = (IntPtr)vam.ReadInt64(pointer);
                }

            }
            return pointer;
        }

    }
}
