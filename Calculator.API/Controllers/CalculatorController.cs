using Calculator.Model.Common.Enums;
using Calculator.Model.DataTransferObjects;
using Calculator.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Calculator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly OperationService _operationService;
        public CalculatorController(OperationService operationService) {
            this._operationService = operationService;
        }   
        
        [HttpGet("{masterId}")]
        public IEnumerable<OperationDto> Get(long masterId)
        {
            return this._operationService.GetOperationsHistory(masterId);
        }

     
        [HttpGet("Add/{num1}/{num2}")]
        public OperationDto Add(int num1, int num2, [FromQuery] long masterId)
        {
            return this._operationService.Add(new OperationDto {
                FirstParameter = num1,
                SecondParameter = num2,
                Type = OperationType.Add,
                MasterId = masterId
            });
        }

       
        [HttpGet("Subtract/{num1}/num2")]
        public OperationDto Subtract(int num1, int num2, [FromQuery] long masterId)
        {
            return this._operationService.Add(new OperationDto {
                FirstParameter = num1,
                SecondParameter = num2,
                Type = OperationType.Subtract,
                MasterId = masterId
            });
        }

        [HttpGet("Multiply/{num1}/num2")]
        public OperationDto Multiply(int num1, int num2, [FromQuery] long masterId)
        {
            return this._operationService.Add(new OperationDto
            {
                FirstParameter = num1,
                SecondParameter = num2,
                Type = OperationType.Multiply,
                MasterId = masterId
            });
        }

        // DELETE api/<CalculatorController>/5
        [HttpDelete("{id}")]
        public OperationDto Delete(int id)
        {
            return this._operationService.Remove(new OperationDto { Id = id });
        }
    }
}
