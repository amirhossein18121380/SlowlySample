using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Api.ViewModels.Topic
{
    public class BaseAddUpdateViewModel
    {
        [Required(ErrorMessage = "Topic Name is required.")]
        public string TopicName { get; set; }
    }
}
