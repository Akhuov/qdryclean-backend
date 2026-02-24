using AutoMapper;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;

namespace QDryClean.Application.UseCases
{
    public class CommandHandlerBase
    {
        protected readonly IApplicationDbContext _applicationDbContext;
        protected readonly ICurrentUserService _currentUserService;
        protected readonly IMapper _mapper;

        public CommandHandlerBase(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
    }
}