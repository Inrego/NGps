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
    public class GprmcReceivedEventArgs : EventArgs
    {
        public string Identifier
        {
            get;
            private set;
        }

        public DateTime DateTime
        {
            get;
            private set;
        }

        public string Status
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

        public float KnotsSpeed
        {
            get;
            private set;
        }

        public float TrueBearing
        {
            get;
            private set;
        }

        public float MagneticVariation
        {
            get;
            private set;
        }

        public string FixType
        {
            get;
            private set;
        }

        public GprmcReceivedEventArgs(string[] values)
        {
            if (String.Compare(values[0], "GPRMC", true) != 0 || values.Length != 13)
            {
                throw new ArgumentException("Invalid GPRMC sentence.", "values");
            }

            this.Identifier = values[0];
            this.DateTime = DateTime.ParseExact(values[9], "ddMMyy", CultureInfo.InvariantCulture).Add(TimeSpan.ParseExact(values[1], @"hhmmss\.fff", CultureInfo.InvariantCulture));
            this.Status = values[2];

            if (!String.IsNullOrEmpty(values[3]))
            {
                this.Latitude = Single.Parse(values[3].Substring(0, 2)) + (Single.Parse(values[3].Substring(2, values[3].Length - 2)) / 60.0f);

                if (values[4] == "S")
                {
                    this.Latitude = -this.Latitude;
                }
            }

            if (!String.IsNullOrEmpty(values[5]))
            {
                this.Longitude = Single.Parse(values[5].Substring(0, 3)) + (Single.Parse(values[5].Substring(3, values[5].Length - 3)) / 60.0f);

                if (values[6] == "W")
                {
                    this.Longitude = -this.Longitude;
                }
            }

            this.KnotsSpeed = Single.Parse(values[7]);
            this.TrueBearing = Single.Parse(values[8]);

            if (!String.IsNullOrEmpty(values[10]))
            {
                this.MagneticVariation = Single.Parse(values[10]);

                if (values[11] == "W")
                {
                    this.MagneticVariation = -this.MagneticVariation;
                }
            }

            this.FixType = values[12];
        }
    }
}
