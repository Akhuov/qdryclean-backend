using MediatR;
using QDryClean.Application.Common.Responses;
using System.Text.Json.Serialization;

namespace QDryClean.Application.UseCases.ItemTypes.Commands
{
    public class UpdateItemTypeCommand : IRequest<ApiResponse<UpdateItemTypeCommand>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}