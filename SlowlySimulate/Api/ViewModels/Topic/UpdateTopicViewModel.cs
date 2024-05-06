using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Api.ViewModels.Topic;
public class UpdateTopicViewModel : BaseAddUpdateViewModel
{
    [Required]
    public Guid TopicId { get; set; }
}