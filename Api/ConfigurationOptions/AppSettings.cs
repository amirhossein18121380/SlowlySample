namespace Api.ConfigurationOptions;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }



    public string AllowedHosts { get; set; }

    public CORS CORS { get; set; }


    public Dictionary<string, string> SecurityHeaders { get; set; }
}
