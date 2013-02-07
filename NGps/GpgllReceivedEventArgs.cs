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
    public class GpgllReceivedEventArgs : EventArgs
    {
        public string Identifier
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

        public DateTime Time
        {
            get;
            private set;
        }

        public string Status
        {
            get;
            private set;
        }

        public GpgllReceivedEventArgs(string[] values)
        {
            if (String.Compare(values[0], "GPGGL", true) != 0 || values.Length != 7)
            {
                throw new ArgumentException("Invalid GPGLL sentence.", "values");
            }

            this.Identifier = values[0];

            if (!String.IsNullOrEmpty(values[1]))
            {
                this.Latitude = Single.Parse(values[1].Substring(0, 2)) + (Single.Parse(values[1].Substring(2, values[1].Length - 2)) / 60.0f);

                if (values[2] == "S")
                {
                    this.Latitude = -this.Latitude;
                }
            }

            if (!String.IsNullOrEmpty(values[3]))
            {
                this.Longitude = Single.Parse(values[3].Substring(0, 3)) + (Single.Parse(values[3].Substring(3, values[3].Length - 3)) / 60.0f);

                if (values[4] == "W")
                {
                    this.Longitude = -this.Longitude;
                }
            }

            if (!String.IsNullOrEmpty(values[5]))
            {
                this.Time = DateTime.UtcNow.Date.Add(TimeSpan.ParseExact(values[5], @"hhmmss\.fff", CultureInfo.InvariantCulture));
            }

            if (!String.IsNullOrEmpty(values[6]))
            {
                this.Status = values[6];
            }
        }
    }
}
