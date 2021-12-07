

namespace ProjectBank.Server.Model;

public static class Extensions
{
    public static IActionResult ToActionResult(this Response response) => response switch
    {
        Response.Updated => new NoContentResult(),
        Response.Deleted => new NoContentResult(),
        Response.NotFound => new NotFoundResult(),
        Response.Conflict => new ConflictResult(),
        Response.Created => new AcceptedResult(),
        Response.BadRequest => new BadRequestResult(),
        
        _ => throw new NotSupportedException($"{response} not supported")
    };
}