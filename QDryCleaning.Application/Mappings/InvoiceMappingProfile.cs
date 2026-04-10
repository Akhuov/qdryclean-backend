using AutoMapper;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Invoices.Commands;
using QDryClean.Domain.Entities;
namespace QDryClean.Application.Mappings
{
    public class InvoiceMappingProfile : Profile
    {
        public InvoiceMappingProfile()
        {
            CreateMap<Invoice, InvoiceDto>().ReverseMap();
            CreateMap<InvoiceDto, CreateInvoiceCommand>().ReverseMap();
        }
    }
}
