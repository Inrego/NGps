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

namespace NGps
{
    public class GpgsaReceivedEventArgs : EventArgs
    {
        public string Identifier
        {
            get;
            private set;
        }

        public ModeType Mode
        {
            get;
            private set;
        }

        public QualityType Quality
        {
            get;
            private set;
        }

        public List<int> Prns
        {
            get;
            private set;
        }

        public float Pdop
        {
            get;
            private set;
        }

        public float Hdop
        {
            get;
            private set;
        }

        public float Vdop
        {
            get;
            private set;
        }

        public GpgsaReceivedEventArgs(string[] values)
        {
            if (String.Compare(values[0], "GPGSA", true) != 0 || values.Length != 18)
            {
                throw new ArgumentException("Invalid GPGSA sentence.", "values");
            }

            this.Prns = new List<int>();

            this.Identifier = values[0];

            if (!String.IsNullOrEmpty(values[1]))
            {
                switch (values[1])
                {
                    case "A":
                        this.Mode = ModeType.Automatic;
                        break;

                    case "M":
                        this.Mode = ModeType.Manual;
                        break;
                }
            }

            if (!String.IsNullOrEmpty(values[2]))
            {
                this.Quality = (QualityType)Enum.Parse(typeof(QualityType), values[2]);
            }

            for (int i = 3; i < 15; i++)
            {
                if (!String.IsNullOrEmpty(values[i]))
                {
                    this.Prns.Add(Int32.Parse(values[i]));
                }
            }

            if (!String.IsNullOrEmpty(values[15]))
            {
                this.Pdop = Single.Parse(values[15]);
            }

            if (!String.IsNullOrEmpty(values[16]))
            {
                this.Hdop = Single.Parse(values[16]);
            }

            if (!String.IsNullOrEmpty(values[17]))
            {
                this.Vdop = Single.Parse(values[17]);
            }
        }
    }
}
