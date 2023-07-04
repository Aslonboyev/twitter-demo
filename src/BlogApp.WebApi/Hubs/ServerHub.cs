using BlogApp.WebApi.Models;
using BlogApp.WebApi.ViewModels.BlogPosts;
using Microsoft.AspNetCore.SignalR;

namespace BlogApp.WebApi.Hubs
{
    public class ServerHub : Hub
    {
        public void BroadcastUser(User user)
        {
            Clients.All.SendAsync("ReceiveUser", user);
        }

        public void BroadcastMessage(BlogPostViewModel blogPostViewModel)
        {
            Clients.All.SendAsync("ReceivePost", blogPostViewModel);
        }
    }
}
