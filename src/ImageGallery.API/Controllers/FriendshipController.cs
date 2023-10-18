using ImageGallery.App.Friendship.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class FriendshipController : MediatrController
{
    [HttpPost]
    [Route("/addfriend")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task AddFriend(AddFriendCommand r)
    {
        await Mediator.Send(r);
    }
    
    [HttpPost]
    [Route("/removefriend")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task DeleteFriend(RemoveFriendCommand r)
    {
        await Mediator.Send(r);
    }
    
    [HttpPost]
    [Route("/allow")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task AllowAddToFriends(AllowAddToFriendCommand r)
    {
         await Mediator.Send(r);

    }

    [HttpPost]
    [Route("/decline")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task DeclineAddToFriend(DeclineAddToFriendCommand r)
    {
        await Mediator.Send(r);
    }
}