//namespace BackgroundServer;

//public interface IBirthdayNotificationService
//{
//    Task SendBirthdayNotificationToAll();
//}

//public class BirthdayNotificationService : IBirthdayNotificationService
//{
//    private readonly IUserService _userService;
//    private readonly IHubContext<BirthdayHub> _hubContext;


//    public BirthdayNotificationService(IUserService userService, IHubContext<BirthdayHub> hubContext)
//    {
//        _userService = userService;
//        _hubContext = hubContext;
//    }


//    public async Task SendBirthdayNotificationToAll()
//    {
//        var today = DateTime.Today;
//        var users = await _userService.GetAsync();
//        var usersWithBirthday = users.Where(user => user.DateOfBirth?.Month == today.Month && user.DateOfBirth?.Day == today.Day);

//        foreach (var user in usersWithBirthday)
//        {
//            var displayName = user.DisplayName;
//            if (displayName != null)
//            {
//                await _hubContext.Clients.All.SendAsync("SendBirthdayNotification", $"{displayName}'s birthday is today!");
//            }
//        }
//    }

//    private async Task SendBirthdayNotification()
//    {
//        var users = await _userService.GetAsync();
//        var today = DateTime.Now.Date;

//        foreach (var user in users)
//        {
//            if (user.DateOfBirth?.Date == today)
//            {
//                if (!string.IsNullOrEmpty(user.UserId.ToString()))
//                {
//                    await _hubContext.Clients.Client(user.UserId.ToString()).SendAsync("SendBirthdayNotification", user.DisplayName);
//                }
//            }
//        }
//    }
//}

//public class BirthdayHub : Hub
//{
//    // No need for additional methods unless required
//}