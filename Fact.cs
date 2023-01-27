using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngineRBS
{
    public class Fact
    {
        public char[] action { get; set; } // Antecedente
        public char consequence { get; set; } // Consecuente
    }
}
