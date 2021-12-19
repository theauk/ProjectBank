namespace ProjectBank.Server.Model;

// Modified code from Rasmus Lystrøm

public static class Extensions
{
    public static IActionResult ToActionResult(this Response response) => response switch
    {
        Response.Updated => new NoContentResult(),
        Response.Deleted => new NoContentResult(),
        Response.NotFound => new NotFoundResult(),
        Response.Conflict => new ConflictResult(),
        Response.BadRequest => new BadRequestResult(),
        Response.Created => new AcceptedResult(),

        _ => throw new NotSupportedException($"{response} not supported")
    };
    
    public static ActionResult<T> ToActionResult<T>(this Option<T> option) where T : class? => option.IsSome ? option.Value : new NotFoundResult();
}
