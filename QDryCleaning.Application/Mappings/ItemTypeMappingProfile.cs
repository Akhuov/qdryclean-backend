using AutoMapper;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.ItemTypes.Commands;
using QDryClean.Domain.Entities;

namespace QDryClean.Application.Mappings
{
    public class ItemTypeMappingProfile : Profile
    {
        public ItemTypeMappingProfile()
        {
            CreateMap<ItemType, ItemTypeDto>();
            CreateMap<ItemTypeDto, CreateItemTypeCommand>().ReverseMap();

            CreateMap<CreateItemTypeCommand, ItemType>()
                    .ForMember(dest => dest.Items, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                    .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                    .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<ItemTypeDto, ItemType>()
                    .ForMember(dest => dest.Items, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                    .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                    .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
