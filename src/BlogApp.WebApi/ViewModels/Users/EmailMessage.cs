namespace BlogApp.WebApi.ViewModels.Users
{
    public class EmailMessage
    {
        public string To { get; set; } = string.Empty;

        public int Body { get; set; }
        
        public string Subject { get; set; } = string.Empty;

        public bool IsEmailConfirmed { get; set; } = false;
    }
}