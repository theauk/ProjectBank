namespace ProjectBank.Server.Model;
// Modified code from Rasmus Lystrøm
public static class Extensions
{
    public static IActionResult ToActionResult(this Response response) => response switch
    {
        Response.Updated => new AcceptedResult(),
        Response.Deleted => new AcceptedResult(),
        Response.NotFound => new NotFoundResult(),
        Response.Conflict => new ConflictResult(),
        Response.Created => new AcceptedResult(),
        Response.BadRequest => new BadRequestResult(),
        
        _ => throw new NotSupportedException($"{response} not supported")
    };
    
    public static ActionResult<T> ToActionResult<T>(this Option<T> option) where T : class? => option.IsSome ? option.Value : new NotFoundResult();
}
