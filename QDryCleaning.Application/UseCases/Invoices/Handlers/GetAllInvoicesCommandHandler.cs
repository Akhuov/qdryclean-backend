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
    public class GetAllInvoicesCommandHandler : BaseHandler, IRequestHandler<GetAllInvoicesCommand, List<InvoiceDto>>
    {
        public GetAllInvoicesCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<List<InvoiceDto>> Handle(GetAllInvoicesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var invoices = await _applicationDbContext.OrderInvoices.ToListAsync();

                var list_of_invoiceDtos = new List<InvoiceDto>();
                foreach (var invoice in invoices)
                {
                    list_of_invoiceDtos.Add(_mapper.Map<InvoiceDto>(invoice));
                }

                return list_of_invoiceDtos;

            }
            catch (Exception ex)
            {
                throw new InternalServerExeption(ex.Message);
            }
        }
    }
}
