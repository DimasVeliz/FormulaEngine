using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FormulaEngine.WebAPI.Models;
using FormulaEngine.Logic;

namespace FormulaEngine.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InterpreterController : ControllerBase
    {
        public InterpreterMPrograms Interpreter { get; }
        private readonly ILogger<InterpreterController> _logger;


        public InterpreterController(ILogger<InterpreterController> logger)
        {
            _logger = logger;
            this.Interpreter=new InterpreterMPrograms();
        }

        [HttpGet]
        public IEnumerable<InterpreterResponse> Get()
        {
            return new List<InterpreterResponse>();
        }

        [HttpPost]
        public InterpreterResponse Post([FromBody] MLSourceprogram lines)
        {
            var convertedLines = Utilities.ConvertLines(lines);
            var scanner = new SourceScanner(convertedLines);
            var lexer = new Lexer(scanner);

            var sourceCodeParsed = new Parser(lexer, new SymbolTable());

            var answer = Interpreter.Execute(sourceCodeParsed.ParseProgram());

            InterpreterResponse response = new InterpreterResponse()
            {
                Outputs = new List<string>(answer.Outputs),
                FunctionOutpus= new List<FunctionOutput>(answer.FunctionOutpus)
            };

            return response;
        }
    }
}
