using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using PivotalServices.WebApiTemplate.CSharp.Extensions;

namespace PivotalServices.WebApiTemplate.CSharp.V2.Features.Values
{
    public class PostValues
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                //Task.Run() is used here just to demonstrate the usage of async/await method
                return await Task.Run(() => new Response { ResultParam1 = request.Param1, ResultParam2 = request.Param2 }, cancellationToken);
            }
        }

        public class Request : IRequest, IRequest<Response>
        {
            public string Param1 { get; set; }
            public string Param2 { get; set; }
        }

        public class Response
        {
            public string ResultParam1 { get; set; }
            public string ResultParam2 { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(p => p.Param1)
                    .IsNonEmptyThreeDigitNumber();

                RuleFor(p => p.Param2)
                    .NotNull()
                    .NotEmpty();
            }
        }
    }
}