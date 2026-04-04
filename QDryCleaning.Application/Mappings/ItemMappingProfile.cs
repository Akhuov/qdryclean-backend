using AutoMapper;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Items.Commands;
using QDryClean.Application.ViewModels;
using QDryClean.Domain.Entities;

namespace QDryClean.Application.Mappings
{
    public class ItemMappingProfile : Profile
    {
        public ItemMappingProfile()
        {
            CreateMap<Item, ItemDto>().ReverseMap();
            CreateMap<ItemDto, ItemViewModel>().ReverseMap();

            CreateMap<CreateItemCommand, Item>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ItemTypeId, opt =>
                {
                    opt.Condition(src => src.ItemTypeId != 0);
                })
                .ForMember(dest => dest.OrderId, opt =>
                {
                    opt.Condition(src => src.OrderId != null && src.OrderId != 0);
                });
        }
    }
}
