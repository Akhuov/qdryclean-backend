using AutoMapper;
using MediatR;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Exceptions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Invoices.Commands;
using QDryClean.Domain.Entities;

namespace QDryClean.Application.UseCases.Invoices.Handlers
{
    public class CreateInvoiceCommandHandler : BaseHandler, IRequestHandler<CreateInvoiceCommand, InvoiceDto>
    {
        public CreateInvoiceCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<InvoiceDto> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var invoice = _mapper.Map<Invoice>(request);
                await _applicationDbContext.Invoices.AddAsync(invoice, cancellationToken);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
                return _mapper.Map<InvoiceDto>(invoice);
            }
            catch (BadRequestExeption)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerExeption(ex.Message);
            }
        }
    }
}
