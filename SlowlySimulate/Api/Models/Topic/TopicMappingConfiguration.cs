using SlowlySimulate.CrossCuttingConcerns.Models;

namespace SlowlySimulate.Api.Models.Topic;
public static class TopicMappingConfiguration
{
    //public static IEnumerable<RoleModel> ToModels(this IEnumerable<Role> entities)
    //{
    //    return entities.Select(x => x.ToModel());
    //}

    public static List<Domain.Models.Topic> ToModel(this ApiResponse result)
    {
        if (result?.Result == null)
        {
            return null;
        }

        if (result.Result is List<Domain.Models.Topic> sourceList)
        {
            return sourceList;
        }

        return new List<Domain.Models.Topic>();
    }

}