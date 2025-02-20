﻿namespace ProjectBank.Server.Model;

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

        _ => throw new NotSupportedException($"{response} not supported")
    };

    public static IActionResult ToActionResult(this Response response, string location,object? value) => response switch
    {
        Response.Created => new CreatedResult(location, value),
        _ => ToActionResult(response)
    };
    
    public static ActionResult<T> ToActionResult<T>(this Option<T> option) where T : class? => option.IsSome ? option.Value : new NotFoundResult();
}