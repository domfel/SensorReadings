using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SensorReadings.Utilities
{
    public class RequestExecutor<TIn, TOut>
    {
        private readonly AbstractValidator<TIn> _validator;
        private readonly IQueryHandler<TIn, TOut> _handler;

        public RequestExecutor(
            AbstractValidator<TIn> validator,
            IQueryHandler<TIn, TOut> handler)
        {
            _validator = validator;
            _handler = handler;            
        }


        public async Task<IActionResult> Execute(TIn input)
        {
            try
            {
                var result = _validator.Validate(input);
                if (result.IsValid)
                {
                    TOut response = await _handler.Execute(input);
                    return new OkObjectResult(response);
                }
                return new BadRequestObjectResult(result.Errors);
            }
            catch (Exception e)
            {
                //Add Loogging
                return new ObjectResult("INTERNAL SERVER ERROR")
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }
    }
}
