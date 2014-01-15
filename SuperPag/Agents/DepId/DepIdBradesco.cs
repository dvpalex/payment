using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Agents.DepId
{
    public class DepIdBradesco : DepId
    {
        public DepIdBradesco() { }

        public static int CalculaDigitoModulo7(string number)
        {
            return int.Parse(number) % 7;
        }

    }
}
