using System.Collections.Generic;

namespace FormulaEngine.Logic
{
    public class FunctionOutput
    {
        public string FunctionName { get; set; }

        public List<double> Values { get; set; }
    }
    public class InterpreterResponse
    {
        public List<string> Outputs { get; set; }
        
        public List<FunctionOutput> FunctionOutpus { get; set; }
        
    }
}