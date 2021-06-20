using System.Linq;
using System.Collections.Generic;
using FormulaEngine.WebAPI.Models;

namespace FormulaEngine.WebAPI
{
    class Utilities
    {
        public static List<string> ConvertLines(MLSourceprogram program)
        {
            return program.lines;
        }
    }
}