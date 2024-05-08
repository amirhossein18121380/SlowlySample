using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SlowlySimulate.Application.Common;
using SlowlySimulate.Application.Users.Queries;
using SlowlySimulate.CrossCuttingConcerns.DateTimes;
using SlowlySimulate.Domain.Identity;
using SlowlySimulate.Domain.Models;

namespace SlowlySimulate.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly Dispatcher _dispatcher;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ICurrentUser _currentUser;
        private Guid UserId = Guid.Empty;
        private readonly IMapper _mapper;


        public HomeController(Dispatcher dispatcher,
            IDateTimeProvider dateTimeProvider,
            ICurrentUser currentUser,
            IMapper mapper)
        {
            _dispatcher = dispatcher;
            _dateTimeProvider = dateTimeProvider;
            _currentUser = currentUser;
            _mapper = mapper;
            UserId = _currentUser.IsAuthenticated ? _currentUser.UserId : Guid.Empty;
        }


        public async Task<IActionResult> Index()
        {
            var friends = await GetFriendsWithCount();
            return View(friends);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<(List<SlowlyUser> Models, int Count)> GetFriendsWithCount()
        {
            var profileModel = await _dispatcher.DispatchAsync(new GetUserFriendsQuery());
            if (profileModel.Result is List<SlowlyUser> models)
            {
                return (models, models.Count);
            }

            return (new List<SlowlyUser>(), 0);
        }

    }
}
