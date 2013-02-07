#region The MIT License

/*
 * The MIT License
 * Source: http://www.opensource.org/licenses/mit-license.php
 * 
 * Copyright (c) 2013 EmitCode Corporation
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the "Software"), to deal in the Software without
 * restriction, including without limitation to the rights to use, copy, modify, merge, publish
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS NOR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES, NOR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT, OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALING IN THE SOFTWARE.
*/

#endregion

using System;
using NGps;

namespace Nmea.TestEnviroment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NmeaReceiver receiver = new NmeaReceiver();
            receiver.GpggaReceived += OnGpggaReceived;
            receiver.GpgllReceived += OnGpgllReceived;
            receiver.GpgsaReceived += OnGpgsaReceived;
            receiver.GpgsvReceived += OnGpgsaReceived;
            receiver.GprmcReceived += OnGprmcReceived;
            receiver.GpvtgReceived += OnGpvtgReceived;

            receiver.Start();

            Console.ReadLine();
        }

        private static void OnGpggaReceived(object sender, GpggaReceivedEventArgs e)
        {
            Console.WriteLine("GPGGA message received.");
        }

        private static void OnGpgllReceived(object sender, GpgllReceivedEventArgs e)
        {
            Console.WriteLine("GPGLL message received.");
        }

        private static void OnGpgsaReceived(object sender, GpgsaReceivedEventArgs e)
        {
            Console.WriteLine("GPGSA message received.");
        }

        private static void OnGpgsaReceived(object sender, GpgsvReceivedEventArgs e)
        {
            Console.WriteLine("GPGSV message received.");
        }

        private static void OnGprmcReceived(object sender, GprmcReceivedEventArgs e)
        {
            Console.WriteLine("GPRMC message received.");
        }

        private static void OnGpvtgReceived(object sender, GpvtgReceivedEventArgs e)
        {
            Console.WriteLine("GPVTG message received.");
        }
    }
}
