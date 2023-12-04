using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNG
{
    static class TypeChecker
    {
        static bool DoTypesMatch(InputConnector a, OutputConnector b) {
            if (a is ExecutionInputConnector && b is ExecutionOutputConnector) return true;
            return false;
        }
    }
}
