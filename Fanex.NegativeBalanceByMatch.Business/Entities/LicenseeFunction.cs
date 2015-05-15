using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.NegativeBalanceByMatch.Business.Entities
{
    public class LicenseeFunction
    {
        public string LicenseeName { get; set; }

        public Function Function { get; set; }
    }

    public class Function
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
