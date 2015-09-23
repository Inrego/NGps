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

namespace NGps
{
    public class GpggaReceivedEventArgs : EventArgs
    {
        public string Identifier
        {
            get;
            private set;
        }

        public DateTime Time
        {
            get;
            private set;
        }

        public float Latitude
        {
            get;
            private set;
        }

        public float Longitude
        {
            get;
            private set;
        }

        public PositionFixType Quality
        {
            get;
            private set;
        }

        public int Satellites
        {
            get;
            private set;
        }

        public float Hdop
        {
            get;
            private set;
        }

        public float Altitude
        {
            get;
            private set;
        }

        public float GeoidalSeperation
        {
            get;
            private set;
        }

        public float Age
        {
            get;
            private set;
        }

        public int StationId
        {
            get;
            private set;
        }

        public GpggaReceivedEventArgs(string[] values)
        {
            if (String.Compare(values[0], "GPGGA", true) != 0 || values.Length != 15)
            {
                throw new ArgumentException("Invalid GPGGA sentence.", "values");
            }

            this.Identifier = values[0];

            if (!String.IsNullOrEmpty(values[2]))
            {
                this.Time = DateTime.UtcNow.Date.Add(TimeSpan.ParseExact(values[1], @"hhmmss\.fff", CultureInfo.InvariantCulture));
            }
            var numberFormat = new NumberFormatInfo {NumberDecimalSeparator = "."};
            if (!String.IsNullOrEmpty(values[2]))
            {
                this.Latitude = Single.Parse(values[2].Substring(0, 2), numberFormat) + (Single.Parse(values[2].Substring(2, values[2].Length - 2), numberFormat) / 60.0f);

                if (values[3] == "S")
                {
                    this.Latitude = -this.Latitude;
                }
            }

            if (!String.IsNullOrEmpty(values[4]))
            {
                this.Longitude = Single.Parse(values[4].Substring(0, 3), numberFormat) + (Single.Parse(values[4].Substring(3, values[4].Length - 3), numberFormat) / 60.0f);

                if (values[5] == "W")
                {
                    this.Longitude = -this.Longitude;
                }
            }

            if (!String.IsNullOrEmpty(values[6]))
            {
                this.Quality = (PositionFixType)Enum.Parse(typeof(PositionFixType), values[6]);
            }

            if (!String.IsNullOrEmpty(values[7]))
            {
                this.Satellites = Int32.Parse(values[7], numberFormat);
            }

            if (!String.IsNullOrEmpty(values[8]))
            {
                this.Hdop = Single.Parse(values[8], numberFormat);
            }

            if (!String.IsNullOrEmpty(values[9]))
            {
                this.Altitude = Single.Parse(values[9], numberFormat);
            }

            if (!String.IsNullOrEmpty(values[11]))
            {
                this.GeoidalSeperation = Single.Parse(values[11], numberFormat);
            }

            if (!String.IsNullOrEmpty(values[13]))
            {
                this.Age = Single.Parse(values[13], numberFormat);
            }

            if (!String.IsNullOrEmpty(values[14]))
            {
                this.StationId = Int32.Parse(values[14], numberFormat);
            }
        }
    }
}
