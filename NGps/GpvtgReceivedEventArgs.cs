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

namespace NGps
{
    public class GpvtgReceivedEventArgs : EventArgs
    {
        public string Identifier
        {
            get;
            private set;
        }

        public float TrueBearing
        {
            get;
            private set;
        }

        public float MagneticBearing
        {
            get;
            private set;
        }

        public float KnotsSpeed
        {
            get;
            private set;
        }

        public float KphSpeed
        {
            get;
            private set;
        }

        public FixType Mode
        {
            get;
            private set;
        }

        public GpvtgReceivedEventArgs(string[] values)
        {
            if (String.Compare(values[0], "GPVTG", true) != 0 || values.Length != 10)
            {
                throw new ArgumentException("Invalid GPVTG sentence.", "values");
            }

            this.Identifier = values[0];

            if (!String.IsNullOrEmpty(values[1]))
            {
                this.TrueBearing = Single.Parse(values[1]);
            }

            if (!String.IsNullOrEmpty(values[3]))
            {
                this.MagneticBearing = Single.Parse(values[3]);
            }

            if (!String.IsNullOrEmpty(values[5]))
            {
                this.KnotsSpeed = Single.Parse(values[5]);
            }

            if (!String.IsNullOrEmpty(values[7]))
            {
                this.KphSpeed = Single.Parse(values[7]);
            }

            if (!String.IsNullOrEmpty(values[9]))
            {
                switch (values[12])
                {
                    case "A":
                        this.Mode = FixType.Autonomous;
                        break;

                    case "D":
                        this.Mode = FixType.Differential;
                        break;

                    case "E":
                        this.Mode = FixType.Estimated;
                        break;

                    case "S":
                        this.Mode = FixType.Simulator;
                        break;

                    default:
                        this.Mode = FixType.Invalid;
                        break;
                }
            }
        }
    }
}
