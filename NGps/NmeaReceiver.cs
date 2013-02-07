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
using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NGps
{
    public class NmeaReceiver : IDisposable
    {
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(2);
        private static readonly char[] Comma = new char[] { ',' };
        private static readonly char[] Asterisk = new char[] { '*' };

        private static string AcquirePort()
        {
            foreach (string name in SerialPort.GetPortNames())
            {
                SerialPort port = new SerialPort(name, 4800, Parity.None, 8, StopBits.One);

                checked
                {
                    port.ReadTimeout = (int)Timeout.TotalMilliseconds;
                }

                try
                {
                    port.Open();
                }
                catch
                {
                    continue;
                }

                DateTime target = DateTime.Now + Timeout;

                while (DateTime.Now <= target)
                {
                    string message;

                    try
                    {
                        message = port.ReadLine();
                    }
                    catch (TimeoutException)
                    {
                        break;
                    }

                    message = message.Replace("$", String.Empty);

                    if (message.StartsWith("GPGGA")
                        || message.StartsWith("GPGLL")
                        || message.StartsWith("GPGSA")
                        || message.StartsWith("GPGSV")
                        || message.StartsWith("GPRMC")
                        || message.StartsWith("GPVTG"))
                    {
                        port.Close();
                        port.Dispose();

                        return name;
                    }
                }

                port.Close();
                port.Dispose();
            }

            throw new DeviceNotFoundException("Cannot locate NMEA device.");
        }

        private static bool VerifyChecksum(string sentence, string checksum)
        {
            if (checksum.Length != 2)
            {
                return false;
            }

            if ((!(checksum[0] >= '0' && checksum[0] <= '9') && !(checksum[0] >= 'A' && checksum[0] <= 'F'))
                || (!(checksum[1] >= '0' && checksum[1] <= '9') && !(checksum[1] >= 'A' && checksum[1] <= 'F')))
            {
                return false;
            }

            byte calculatedChecksum = 0;

            foreach (byte value in Encoding.ASCII.GetBytes(sentence))
            {
                calculatedChecksum ^= value;
            }

            return calculatedChecksum == Byte.Parse(checksum, NumberStyles.HexNumber);
        }

        private readonly SerialPort port;

        private readonly CancellationTokenSource tokenSource;
        private readonly Task worker;

        private volatile int listening;

        public event EventHandler<GpggaReceivedEventArgs> GpggaReceived;
        public event EventHandler<GpgllReceivedEventArgs> GpgllReceived;
        public event EventHandler<GpgsaReceivedEventArgs> GpgsaReceived;
        public event EventHandler<GpgsvReceivedEventArgs> GpgsvReceived;
        public event EventHandler<GprmcReceivedEventArgs> GprmcReceived;
        public event EventHandler<GpvtgReceivedEventArgs> GpvtgReceived;

        public NmeaReceiver()
            : this(AcquirePort())
        { }

        public NmeaReceiver(string portName)
            : this(portName, 4800, Parity.None, 8, StopBits.One)
        { }

        public NmeaReceiver(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
            : this(new SerialPort(portName, baudRate, parity, dataBits, stopBits))
        { }

        public NmeaReceiver(SerialPort port)
        {
            this.port = port;
            this.port.NewLine = "\r\n";

            this.tokenSource = new CancellationTokenSource();
            this.worker = new Task(this.Read, this.tokenSource.Token);

            this.listening = 0;
        }

        ~NmeaReceiver()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this.listening == 1)
            {
                this.Stop();
            }

            this.port.Dispose();
        }

        private void OnGpggaReceived(GpggaReceivedEventArgs args)
        {
            EventHandler<GpggaReceivedEventArgs> callback = this.GpggaReceived;

            if (Object.ReferenceEquals(callback, null))
            {
                return;
            }

            callback(this, args);
        }

        private void OnGpgllReceived(GpgllReceivedEventArgs args)
        {
            EventHandler<GpgllReceivedEventArgs> callback = this.GpgllReceived;

            if (Object.ReferenceEquals(callback, null))
            {
                return;
            }

            callback(this, args);
        }

        private void OnGpgsaReceived(GpgsaReceivedEventArgs args)
        {
            EventHandler<GpgsaReceivedEventArgs> callback = this.GpgsaReceived;

            if (Object.ReferenceEquals(callback, null))
            {
                return;
            }

            callback(this, args);
        }

        private void OnGpgsvReceived(GpgsvReceivedEventArgs args)
        {
            EventHandler<GpgsvReceivedEventArgs> callback = this.GpgsvReceived;

            if (Object.ReferenceEquals(callback, null))
            {
                return;
            }

            callback(this, args);
        }

        private void OnGprmcReceived(GprmcReceivedEventArgs args)
        {
            EventHandler<GprmcReceivedEventArgs> callback = this.GprmcReceived;

            if (Object.ReferenceEquals(callback, null))
            {
                return;
            }

            callback(this, args);
        }

        private void OnGpvtgReceived(GpvtgReceivedEventArgs args)
        {
            EventHandler<GpvtgReceivedEventArgs> callback = this.GpvtgReceived;

            if (Object.ReferenceEquals(callback, null))
            {
                return;
            }

            callback(this, args);
        }

        public void Read()
        {
            while (this.listening == 1)
            {
                string message;

                try
                {
                    message = this.port.ReadLine();
                }
                catch
                {
                    throw new DeviceNotFoundException("Cannot locate an NMEA device.");
                }

                message = message.Replace("$", String.Empty);

                string[] parts = message.Split(Asterisk);

                if (parts.Length < 2)
                {
                    continue;
                }

                if (!VerifyChecksum(parts[0], parts[1]))
                {
                    continue;
                }

                string[] values = parts[0].Split(Comma);

                switch (values[0])
                {
                    case "GPGGA":
                        this.OnGpggaReceived(new GpggaReceivedEventArgs(values));
                        break;

                    case "GPGLL":
                        this.OnGpgllReceived(new GpgllReceivedEventArgs(values));
                        break;

                    case "GPGSA":
                        this.OnGpgsaReceived(new GpgsaReceivedEventArgs(values));
                        break;

                    case "GPGSV":
                        this.OnGpgsvReceived(new GpgsvReceivedEventArgs(values));
                        break;

                    case "GPRMC":
                        this.OnGprmcReceived(new GprmcReceivedEventArgs(values));
                        break;

                    case "GPVTG":
                        this.OnGpvtgReceived(new GpvtgReceivedEventArgs(values));
                        break;
                }
            }
        }

        public void Start()
        {
            if (Interlocked.CompareExchange(ref this.listening, 1, 0) == 1)
            {
                throw new InvalidOperationException("The receiver is already started.");
            }

            if (this.worker.Status == TaskStatus.Running)
            {
                return;
            }

            try
            {
                this.port.Open();
            }
            catch
            {
                throw new DeviceNotFoundException("No device is connected to the port.");
            }

            this.worker.Start();
        }

        public void Stop()
        {
            if (Interlocked.CompareExchange(ref this.listening, 0, 1) == 0)
            {
                throw new InvalidOperationException("The receiver is already stopped.");
            }

            if (this.worker.Status == TaskStatus.Running)
            {
                this.tokenSource.Cancel();
                this.worker.Wait();
                this.port.Close();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
