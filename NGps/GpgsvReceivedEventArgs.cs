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
using System.Collections.Generic;
using System.Globalization;

namespace NGps
{
    public class GpgsvReceivedEventArgs : EventArgs
    {
        public string Identifier
        {
            get;
            private set;
        }

        public int SentenceCount
        {
            get;
            private set;
        }

        public int SentenceId
        {
            get;
            private set;
        }

        public int SatelliteCount
        {
            get;
            private set;
        }

        public List<Satellite> Satellites
        {
            get;
            private set;
        }

        public GpgsvReceivedEventArgs(string[] values)
        {
            if (String.Compare(values[0], "GPGSV", true) != 0 || !(values.Length >= 8 && values.Length <= 20 && (values.Length % 4 == 0)))
            {
                throw new ArgumentException("Invalid GPGSV sentence.", "values");
            }

            this.Satellites = new List<Satellite>();

            this.Identifier = values[0];
            var numberFormat = new NumberFormatInfo { NumberDecimalSeparator = "." };
            if (!String.IsNullOrEmpty(values[1]))
            {
                this.SentenceCount = Int32.Parse(values[1], numberFormat);
            }

            if (!String.IsNullOrEmpty(values[2]))
            {
                this.SentenceId = Int32.Parse(values[2], numberFormat);
            }

            if (!String.IsNullOrEmpty(values[3]))
            {
                this.SatelliteCount = Int32.Parse(values[3], numberFormat);
            }

            for (int i = 4, count = 0; count < (values.Length / 4) - 1; i += 4, count++)
            {
                Satellite satellite = new Satellite();
                satellite.Prn = Int32.Parse(values[i], numberFormat);
                satellite.Elevation = Int32.Parse(values[i + 1], numberFormat);
                satellite.Azimuth = Int32.Parse(values[i + 2], numberFormat);

                if (!String.IsNullOrEmpty(values[i + 3]))
                {
                    satellite.Snr = Int32.Parse(values[i + 3], numberFormat);
                }

                this.Satellites.Add(satellite);
            }
        }
    }
}
