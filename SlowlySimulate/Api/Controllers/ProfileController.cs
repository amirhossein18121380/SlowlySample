﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlowlySimulate.Api.ViewModels.Profile;
using SlowlySimulate.Application.Common;
using SlowlySimulate.Application.Topic.Queries;
using SlowlySimulate.Application.TopicOfInterest.Commands;
using SlowlySimulate.Application.TopicOfInterest.Queries;
using SlowlySimulate.Application.UserLanguage.Commands;
using SlowlySimulate.Application.UserLanguage.Queries;
using SlowlySimulate.Application.Users.Commands;
using SlowlySimulate.Application.Users.Queries;
using SlowlySimulate.CrossCuttingConcerns.DateTimes;
using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Domain.Identity;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Domain.Repositories.Extension;
using SlowlySimulate.Persistence.Permissions;
using SlowlySimulate.Shared.Dto.Profile;

namespace SlowlySimulate.Api.Controllers;

public class ProfileController : Controller
{
    private readonly Dispatcher _dispatcher;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUser _currentUser;
    private Guid UserId = Guid.Empty;
    private readonly IMapper _mapper;
    public ProfileController(Dispatcher dispatcher,
        UserManager<ApplicationUser> userManager,
        ILogger<UsersController> logger,
        IDateTimeProvider dateTimeProvider,
        ICurrentUser currentUser,
        IMapper mapper)
    {
        _dispatcher = dispatcher;
        _userManager = userManager;
        _dateTimeProvider = dateTimeProvider;
        _currentUser = currentUser;
        _mapper = mapper;
        UserId = _currentUser.IsAuthenticated ? _currentUser.UserId : Guid.Empty;
    }

    public IActionResult Profile()
    {
        return View();
    }

    public async Task<IActionResult> LoadPartial(string partialName)
    {
        object model = null;

        switch (partialName)
        {
            case "_TopicOfInterests":
                var fixedTopics = await FixedTopicOfInterests();
                var userTopics = await UserTopicOfInterests();
                model = new TopicViewModel
                {
                    FixedTopics = fixedTopics,
                    UserTopics = userTopics
                };
                break;
            case "_Languages":
                model = await GetUserLanguagesQuery();
                break;
            case "_Profile":
                model = await GetUserProfileQuery();
                break;
            case "_MatchingPreferences":
                model = await GetUserMatchingPreferences();
                break;
            case "_ExcludedTopics":
                var existTopics = await FixedTopicOfInterests();
                var interestTopicOfUser = await UserTopicOfInterests();
                var userExcludedTopics = await UserExcludedTopics();
                model = new ExcludeTopicViewModel
                {
                    FixedTopics = existTopics,
                    UserTopics = interestTopicOfUser,
                    UserExcludedTopics = userExcludedTopics
                };
                break;
            case "_SlowlyPlus":
                break;
            case "_Settings":
                break;
            default:
                break;
        }

        return PartialView(partialName, model);
    }

    #region Topic Of Interest 

    [HttpGet]
    [Authorize(Permissions.TopicOfInterest.ReadTopicRelated)]
    public async Task<List<Topic>> FixedTopicOfInterests()
    {
        var response = await _dispatcher.DispatchAsync(new GetTopicsQuery());
        if (response.Result is List<Topic> topics)
        {
            return topics;
        }

        return new List<Topic>();
    }

    [HttpGet]
    [Authorize(Permissions.TopicOfInterest.ReadTopicRelated)]
    public async Task<List<Topic>> UserTopicOfInterests()
    {
        var userTopics = await _dispatcher.DispatchAsync(new GetTopicOfInterestQuery());
        return userTopics;
    }

    [HttpGet]
    [Authorize(Permissions.TopicOfInterest.UserExcludedTopics)]
    public async Task<List<Topic>> UserExcludedTopics()
    {
        var userExcludedTopics = await _dispatcher.DispatchAsync(new GetUserExcludedTopicsQuery());
        return userExcludedTopics;
    }


    [HttpPost]
    [Authorize(Permissions.TopicOfInterest.AddTopicOfInterest)]
    public async Task<ActionResult<ApiResponse>> AddTopicOfInterest(List<Guid> topicIds)
    {
        var res = await _dispatcher.DispatchAsync(new AddTopicOfInterestCommand() { UserId = UserId, TopicIds = topicIds });
        return Ok(new ApiResponse(res.StatusCode, res.Message));
    }

    [HttpPost]
    [Authorize(Permissions.TopicOfInterest.ExcludedTopics)]
    public async Task<ActionResult<ApiResponse>> ExcludedTopics(List<Guid> topicIds)
    {
        var res = await _dispatcher.DispatchAsync(new ExcludedUserTopicsCommand() { UserId = UserId, TopicIds = topicIds });
        return Ok(new ApiResponse(res.StatusCode, res.Message));
    }

    [HttpPost]
    [Authorize(Permissions.TopicOfInterest.AddSubTopic)]
    public async Task<ActionResult<ApiResponse>> AddSubTopic(AddSubTopicViewModel model)
    {
        var res = await _dispatcher.DispatchAsync(new AddSubTopicCommand() { UserId = UserId, TopicId = model.TopicId, SubTopicName = model.SubTopicName });
        return Ok(new ApiResponse(res.StatusCode, res.Message));
    }

    [HttpGet]
    [Authorize(Permissions.TopicOfInterest.GetSubTopics)]
    public async Task<ActionResult<ApiResponse>> GetSubTopics(Guid topicId)
    {
        var subTopics = await _dispatcher.DispatchAsync(new GetSubTopicsQuery() { UserId = UserId, TopicId = topicId });
        return Ok(new ApiResponse(statusCode: 200, "Fetched Subtopics successfully", subTopics));
    }

    #endregion

    #region Matching Preferences

    [HttpPost, ActionName("MatchingPreferences")]
    public async Task<ApiResponse> MatchingPreferences([FromBody] MatchingPreferencesViewModel request)
    {
        var response = await _dispatcher.DispatchAsync(new UpdateMatchingPreferencesCommand()
        {
            UserId = UserId,
            AllowAddMeById = request.AllowAddMeById,
            AutoMatch = request.AutoMatch,
            ProfileSuggestions = request.ProfileSuggestions,
            EnableAgeFilter = request.EnableAgeFilter,
            BeMale = request.BeMale,
            BeFemale = request.BeFemale,
            BeNonBinary = request.BeNonBinary,
            AgeFrom = request.AgeFrom,
            AgeTo = request.AgeTo
        });

        return new ApiResponse(response.StatusCode, response.Message, response.Result);
    }

    [HttpGet, ActionName("GetUserMatchingPreferences")]
    public async Task<GetUserMatchingPreferencesViewModel> GetUserMatchingPreferences()
    {
        var profileModel = await _dispatcher.DispatchAsync(new GetUserMatchingPreferencesQuery());
        if (profileModel.Result is GetUserMatchingPreferencesDto model)
        {
            var matching = _mapper.Map<GetUserMatchingPreferencesViewModel>(model);
            return matching;
        }
        return new GetUserMatchingPreferencesViewModel();
    }

    [HttpGet]
    public async Task<GetProfileViewModel> GetUserProfileQuery()
    {
        var profileModel = await _dispatcher.DispatchAsync(new GetUserProfile());
        if (profileModel.Result is GetProfileDto model)
        {
            var profile = _mapper.Map<GetProfileViewModel>(model);
            return profile;
        }

        return new GetProfileViewModel();
    }

    [HttpGet]
    public async Task<GetProfilePreviewViewModel> GetUserProfilePreviewQuery()
    {
        var profilePreviewModel = await _dispatcher.DispatchAsync(new GetProfilePreviewQuery());
        if (profilePreviewModel.Result is ProfilePreviewDto model)
        {
            var preview = _mapper.Map<GetProfilePreviewViewModel>(model);
            return preview;
        }
        return new GetProfilePreviewViewModel();
    }

    [HttpPost]
    public async Task<ApiResponse> UpdateProfile([FromBody] UpdateProfileViewModel request)
    {
        var response = await _dispatcher.DispatchAsync(new UpdateProfileCommand()
        {
            UserId = UserId,
            LetterLength = (LetterLength)request.LetterLength,
            ReplyTime = (ReplyTime)request.ReplyTime,
            AboutMe = request.AboutMe,
            AllowAddMeById = request.AllowAddMeById
        });

        return new ApiResponse(response.StatusCode, response.Message, response.Result);
    }

    #endregion

    #region LanguageView

    [HttpGet]
    [Authorize(Permissions.Language.Read)]
    public List<LanguageModel> GetAllLanguages()
    {
        var allLanguages = LanguageExtensions.GetAllLanguageModel();
        return allLanguages;
    }

    [HttpGet]
    [Authorize(Permissions.Language.Read)]
    public string GetLanguage(int id)
    {
        var lang = id.GetLanguageName();
        return lang;
    }

    [HttpGet]
    [Authorize(Permissions.Language.Read)]
    public async Task<List<LanguagesOfUser>> GetUserLanguagesQuery()
    {
        var userLanguages = await _dispatcher.DispatchAsync(new GetUserLanguagesQuery());
        return userLanguages;
    }

    [HttpPost]
    [Authorize(Permissions.Language.Create)]
    public async Task<ActionResult<ApiResponse>> AddUserLanguage(UserLanViewModel userLan)
    {
        var res = await _dispatcher.DispatchAsync(new AddUserLanguageCommand()
        {
            UserId = UserId,
            LanguageId = userLan.LanguageId,
            LanguageLevel = userLan.LanguageLevel
        });

        return Ok(new ApiResponse(res.StatusCode, res.Message, res.Result));
    }

    [HttpPost]
    [Authorize(Permissions.Language.Update)]
    public async Task<ActionResult<ApiResponse>> UpdateLanguage(UpdateLanguageViewModel model)
    {
        var res = await _dispatcher.DispatchAsync(new UpdateUserLanguageLevelCommand() { UserId = UserId, LanguageId = model.LanguageId, LanguageLevel = model.Level });
        return Ok(new ApiResponse(res.StatusCode, res.Message, res.Result));
    }

    [HttpPost]
    [Authorize(Permissions.Language.Delete)]
    public async Task<ActionResult<ApiResponse>> DeleteLanguage(int languageId)
    {
        var res = await _dispatcher.DispatchAsync(new DeleteUserLanguageCommand() { UserId = UserId, LanguageId = languageId });
        return Ok(new ApiResponse(res.StatusCode, res.Message));
    }

    #endregion
}