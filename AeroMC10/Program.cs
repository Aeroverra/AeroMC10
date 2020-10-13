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

            var pointer = FindPointer(baseAddress, staticPointerOffset, offsets);



            while (true)
            {
                var value = vam.ReadInt32(pointer);
                Console.WriteLine($"value: {value}");
                // vam.WriteInt32(p8, 1);
                Thread.Sleep(1000);
            }
        }

        public static IntPtr FindPointer(IntPtr applicationBaseAddress, int baseAddressOffset, int[] offsets )
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


        public static void SetAnvilCost2()
        {
            if (AnvilIsOn)
            {
                AnvilIsOn = false;
                return;
            }
            int[] offsets = new int[] { 0x0, 0x18, 0x80, 0x5A8, 0x50, 0x20, 0xA8 };
            // works on realms and single player
            var p1 = IntPtr.Add(baseAddress, 0x036A0278);
            var p1v = (IntPtr)vam.ReadInt64(p1);
            Console.WriteLine($"p1v: {p1v.AsHex()}");

            var p2 = IntPtr.Add(p1v, 0x0);
            var p2v = (IntPtr)vam.ReadInt64(p2);
            Console.WriteLine($"p2v: {p2v.AsHex()}");

            var p3 = IntPtr.Add(p2v, 0x18);
            var p3v = (IntPtr)vam.ReadInt64(p3);
            Console.WriteLine($"p3v: {p3v.AsHex()}");

            var p4 = IntPtr.Add(p3v, 0x80);
            var p4v = (IntPtr)vam.ReadInt64(p4);
            Console.WriteLine($"p4v: {p4v.AsHex()}");

            var p5 = IntPtr.Add(p4v, 0x5A8);
            var p5v = (IntPtr)vam.ReadInt64(p5);
            Console.WriteLine($"p5v: {p5v.AsHex()}");

            var p6 = IntPtr.Add(p5v, 0x50);
            var p6v = (IntPtr)vam.ReadInt64(p6);
            Console.WriteLine($"p6v: {p6v.AsHex()}");

            var p7 = IntPtr.Add(p6v, 0x20);
            var p7v = (IntPtr)vam.ReadInt64(p7);
            Console.WriteLine($"p7v: {p7v.AsHex()}");

            var p8 = IntPtr.Add(p7v, 0xA8);
            var p8v = vam.ReadInt32(p8);
            Console.WriteLine($"p8v: {p8v}");
            while (true)
            {
                var p8v2 = vam.ReadInt32(p8);
                Console.WriteLine($"p8v: {p8v2}");
                vam.WriteInt32(p8, 1);
                Thread.Sleep(1000);
            }





        }
        public static void ToggleFly()
        {
            // works on realms and single player
            var p1 = IntPtr.Add(baseAddress, 0x03655728);
            var p1v = (IntPtr)vam.ReadInt64(p1);
            Console.WriteLine($"p1v: {p1v.AsHex()}");

            var p2 = IntPtr.Add(p1v, 0xA8);
            var p2v = (IntPtr)vam.ReadInt64(p2);
            Console.WriteLine($"p2v: {p2v.AsHex()}");

            var p3 = IntPtr.Add(p2v, 0x20);
            var p3v = (IntPtr)vam.ReadInt64(p3);
            Console.WriteLine($"p3v: {p3v.AsHex()}");

            var p4 = IntPtr.Add(p3v, 0x48);
            var p4v = (IntPtr)vam.ReadInt64(p4);
            Console.WriteLine($"p4v: {p4v.AsHex()}");

            var p5 = IntPtr.Add(p4v, 0x10);
            var p5v = (IntPtr)vam.ReadInt64(p5);
            Console.WriteLine($"p5v: {p5v.AsHex()}");

            var p6 = IntPtr.Add(p5v, 0x58);
            var p6v = (IntPtr)vam.ReadInt64(p6);
            Console.WriteLine($"p6v: {p6v.AsHex()}");

            var p7 = IntPtr.Add(p6v, 0x0);
            var p7v = (IntPtr)vam.ReadInt64(p7);
            Console.WriteLine($"p7v: {p7v.AsHex()}");

            var p8 = IntPtr.Add(p7v, 0x8B8);
            var p8v = vam.ReadInt32(p8);
            Console.WriteLine($"p8v: {p8v}");

            while (true)
            {
                var p8v2 = vam.ReadInt32(p8);
                Console.WriteLine($"p8v: {p8v2}");
                vam.WriteInt32(p8, 1);
                Thread.Sleep(100);
            }


        }


        public static void ToggleInstaBreak()
        {
            var personalModePointer = new IntPtr(0x29EA64C1EC4);
            var personalModeValue = vam.ReadInt32(personalModePointer);
            Console.WriteLine($"Personal Mode: {personalModeValue}");

            var p1 = IntPtr.Add(baseAddress, 0x3683F48);
            var p1v = (IntPtr)vam.ReadInt64(p1);
            Console.WriteLine($"p1v: {p1v.AsHex()}");

            var p2 = IntPtr.Add(p1v, 0xD0);
            var p2v = (IntPtr)vam.ReadInt64(p2);
            Console.WriteLine($"p2v: {p2v.AsHex()}");

            var p3 = IntPtr.Add(p2v, 0x58);
            var p3v = (IntPtr)vam.ReadInt64(p3);
            Console.WriteLine($"p3v: {p3v.AsHex()}");

            var p4 = IntPtr.Add(p3v, 0xB8);
            var p4v = (IntPtr)vam.ReadInt64(p4);
            Console.WriteLine($"p4v: {p4v.AsHex()}");

            var p5 = IntPtr.Add(p4v, 0x158);
            var p5v = (IntPtr)vam.ReadInt64(p5);
            Console.WriteLine($"p5v: {p5v.AsHex()}");

            var p6 = IntPtr.Add(p5v, 0x140);
            var p6v = (IntPtr)vam.ReadInt64(p6);
            Console.WriteLine($"p6v: {p6v.AsHex()}");

            var p7 = IntPtr.Add(p6v, 0x1C84);
            var p7v = vam.ReadInt32(p7);
            Console.WriteLine($"p7v: {p7v}");
            if (p7v == 1)
            {
                vam.WriteInt32(p7, 0);
            }
            else
            {
                vam.WriteInt32(p7, 1);
            }

        }
    }
}
