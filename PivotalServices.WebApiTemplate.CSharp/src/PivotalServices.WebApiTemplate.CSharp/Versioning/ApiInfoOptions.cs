namespace PivotalServices.WebApiTemplate.CSharp.Versioning
{
    public class ApiInfoOptions
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DeprecationMessage { get; set; } = string.Empty;
        public ApiContact Contact { get; set; } = new ApiContact();
        public ApiLicense License { get; set; } = new ApiLicense();
    }

    public class ApiContact
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class ApiLicense
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
