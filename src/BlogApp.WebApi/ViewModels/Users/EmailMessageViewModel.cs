namespace BlogApp.WebApi.ViewModels.Users
{
    public class EmailMessageViewModel
    {
        public string To { get; set; } = string.Empty;

        public int Body { get; set; }
        
        public string Subject { get; set; } = string.Empty;
    }
}