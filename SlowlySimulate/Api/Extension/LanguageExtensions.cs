using Domain.Constants;
using SlowlySimulate.Api.ViewModels.Profile;

namespace Domain.Repositories.Extension;

public static class LanguageExtensions
{
    private static readonly Dictionary<int, string> Languages = new Dictionary<int, string> {
    { 1, "English" },
    { 2, "Spanish" },
    { 3, "French" },
    { 4, "German" },
    { 5, "Chinese" },
    { 6, "Japanese" },
    { 7, "Italian" },
    { 8, "Russian" },
    { 9, "Arabic" },
    { 10, "Portuguese" },
    { 11, "Dutch" },
    { 12, "Swedish" },
    { 13, "Norwegian" },
    { 14, "Danish" },
    { 15, "Finnish" },
    { 16, "Korean" },
    { 17, "Turkish" },
    { 18, "Greek" },
    { 19, "Hindi" },
    { 20, "Bengali" },
    { 21, "Punjabi" },
    { 22, "Urdu" },
    { 23, "Indonesian" },
    { 24, "Malay" },
    { 25, "Vietnamese" },
    { 26, "Thai" },
    { 27, "Hebrew" },
    { 28, "Polish" },
    { 29, "Czech" },
    { 30, "Hungarian" },
    { 31, "Romanian" },
    { 32, "Bulgarian" },
    { 33, "Croatian" },
    { 34, "Serbian" },
    { 35, "Slovak" },
    { 36, "Slovenian" },
    { 37, "Macedonian" },
    { 38, "Albanian" },
    { 39, "Estonian" },
    { 40, "Latvian" },
    { 41, "Lithuanian" },
    { 42, "Georgian" },
    { 43, "Armenian" },
    { 44, "Azerbaijani" },
    { 45, "Kazakh" },
    { 46, "Uzbek" },
    { 47, "Turkmen" },
    { 48, "Kyrgyz" },
    { 49, "Tajik" },
    { 50, "Mongolian" },
    { 51, "Nepali" },
    { 52, "Sinhala" },
    { 53, "Burmese" },
    { 54, "Khmer" },
    { 55, "Lao" },
    { 56, "Malagasy" },
    { 57, "Swahili" },
    { 58, "Yoruba" },
    { 59, "Igbo" },
    { 60, "Hausa" },
    { 61, "Zulu" },
    { 62, "Xhosa" },
    { 63, "Afrikaans" },
    { 64, "Fijian" },
    { 65, "Tongan" },
    { 66, "Samoan" },
    { 67, "Maori" },
    { 68, "Hawaiian" },
    { 69, "Cherokee" },
    { 70, "Navajo" },
    { 71, "Inuktitut" },
    { 72, "Greenlandic" },
    { 73, "Sami" },
    { 74, "Faroese" },
    { 75, "Basque" },
    { 76, "Catalan" },
    { 77, "Galician" },
    { 78, "Aragonese" },
    { 79, "Occitan" },
    { 80, "Corsican" },
    { 81, "Luxembourgish" },
    { 82, "Monégasque" },
    { 83, "Maltese" },
    { 84, "Icelandic" },
    { 85, "Frisian" },
    { 86, "Sorbian" },
    { 87, "Walloon" },
    { 88, "Picard" },
    { 89, "Limburgish" },
    { 90, "Alemannic" },
    { 91, "Franconian" },
    { 92, "Silesian" },
    { 93, "Kashubian" },
    { 94, "Sardinian" },
    { 95, "Sicilian" },
    { 96, "Romansh" },
    { 97, "Ladin" },
    { 98, "Friulian" },
    { 99, "Rhaeto-Romanic" },
    { 100, "Esperanto" }
    };


    private static readonly Dictionary<int, LanguageLevel> LanguageLevels = new Dictionary<int, LanguageLevel>
    {
        { 1, LanguageLevel.Interested },
        { 2, LanguageLevel.Beginner },
        { 3, LanguageLevel.Intermediate },
        { 4, LanguageLevel.Advanced },
        { 5, LanguageLevel.Fluent },
        { 6, LanguageLevel.Native },
        // Add more language levels as needed
    };

    public static bool IsValidLanguageId(this int languageId)
    {
        return Languages.ContainsKey(languageId);
    }

    public static string GetLanguageName(this int languageId)
    {
        return Languages.TryGetValue(languageId, out var languageName) ? languageName : string.Empty;
    }

    public static LanguageLevel GetLanguageLevel(this int languageLevelId)
    {
        return LanguageLevels.TryGetValue(languageLevelId, out var languageLevel) ? languageLevel : LanguageLevel.Interested;
    }
    public static IEnumerable<string> GetAllLanguages()
    {
        return Languages.Values;
    }
    public static List<LanguageModel> GetAllLanguageModel()
    {
        var list = new List<LanguageModel>();
        foreach (var language in Languages)
        {
            var model = new LanguageModel
            {
                LanguageId = language.Key,
                LanguageName = language.Value
            };

            list.Add(model);
        }
        return list;
    }


}
