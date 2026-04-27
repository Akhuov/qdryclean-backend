using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Exceptions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Invoices.Quarries;

namespace QDryClean.Application.UseCases.Invoices.Handlers
{
    public class GetByIdInvoiceCommandHandler : BaseHandler, IRequestHandler<GetByIdInvoiceCommand, InvoiceDto>
    {
        public GetByIdInvoiceCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<InvoiceDto> Handle(GetByIdInvoiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var invoice = await _applicationDbContext.Invoices.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
                return _mapper.Map<InvoiceDto>(invoice);

            }
            catch (Exception ex)
            {
                throw new InternalServerExeption(ex.Message);
            }
        }
    }
}
