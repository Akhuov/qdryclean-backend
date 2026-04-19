using AutoMapper;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;

namespace QDryClean.Application.UseCases
{
    public class BaseHandler
    {
        protected readonly IApplicationDbContext _applicationDbContext;
        protected readonly ICurrentUserService _currentUserService;
        protected readonly IMapper _mapper;

        public BaseHandler(
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