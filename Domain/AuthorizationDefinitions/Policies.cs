﻿
using Domain.Permissions;

namespace Domain.AuthorizationDefinitions
{
    public static class Policies
    {
        public const string IsAdmin = "IsAdmin";
        public const string IsUser = "IsUser";
        public const string IsMyEmailDomain = "IsMyEmailDomain";
        public const string TwoFactorEnabled = "TwoFactorEnabled";
        public const string IsSubscriptionActive = "IsSubscriptionActive";
        public static string For(UserFeatures userFeature) => $"Is{userFeature}";
    }
}
