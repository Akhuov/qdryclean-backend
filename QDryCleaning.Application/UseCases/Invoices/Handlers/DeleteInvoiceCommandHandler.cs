using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Exceptions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.UseCases.Invoices.Commands;

namespace QDryClean.Application.UseCases.Invoices.Handlers
{
    public class DeleteInvoiceCommandHandler : BaseHandler, IRequestHandler<DeleteInvoiceCommand, string>
    {
        public DeleteInvoiceCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<string> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var invoice = await _applicationDbContext.Invoices.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
                if (invoice is not null)
                {
                    _applicationDbContext.Invoices.Remove(invoice);
                    await _applicationDbContext.SaveChangesAsync(cancellationToken);
                    return $"Invoice {invoice.Id} Deleted Succesfully!";
                }
                else
                {
                    throw new BadRequestExeption($"Invoice with ID {request.Id} not found.");
                }
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
