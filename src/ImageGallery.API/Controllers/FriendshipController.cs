using ImageGallery.App.Friendship.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.API.Controllers;

[Authorize]
public class FriendshipController : MediatrController
{
    [HttpPost]  
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task AddFriend(AddFriendCommand r)
    {
        await Mediator.Send(r);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task DeleteFriend(RemoveFriendCommand r)
    {
        await Mediator.Send(r);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task AllowAddToFriends(AllowAddToFriendCommand r)
    {
         await Mediator.Send(r);

    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task DeclineAddToFriend(DeclineAddToFriendCommand r)
    {
        await Mediator.Send(r);
    }
}