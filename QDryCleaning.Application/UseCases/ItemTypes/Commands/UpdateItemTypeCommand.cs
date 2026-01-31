using MediatR;
using QDryClean.Application.Common.Responses;

namespace QDryClean.Application.UseCases.ItemTypes.Commands
{
    public class UpdateItemTypeCommand : IRequest<ApiResponse<UpdateItemTypeCommand>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}