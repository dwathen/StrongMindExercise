using Microsoft.AspNetCore.Mvc;
using StrongMindExercise.Application.Errors;

namespace StrongMindExercise.API.ControllerResult;

public class ControllerResultBase : ControllerBase
{
    protected ActionResult<T> HandleResult<T>(Result<T> result)
    {
        if (result.Error == null)
        {
            return Ok(result);
        }

        if (result.Error.Code == ErrorCodes.ObjectCannotBeFound)
        {
            return NotFound(result.Error);
        }

        if (result.Error.Code == ErrorCodes.ObjectAlreadyExists || result.Error.Code == ErrorCodes.NameCannotBeDuplicate ||
            result.Error.Code == ErrorCodes.CannotHaveDuplicateChildren)
        {
            return Conflict(result.Error);
        }

        return BadRequest(result.Error);
    }

    protected ActionResult HandleResult(Result result)
    {
        if (result.Error == null)
        {
            return Ok(result);
        }

        if (result.Error.Code == ErrorCodes.ObjectCannotBeFound)
        {
            return NotFound(result.Error);
        }

        if (result.Error.Code == ErrorCodes.ObjectAlreadyExists || result.Error.Code == ErrorCodes.NameCannotBeDuplicate ||
            result.Error.Code == ErrorCodes.CannotHaveDuplicateChildren)
        {
            return Conflict(result.Error);
        }

        return BadRequest(result.Error);
    }
}
