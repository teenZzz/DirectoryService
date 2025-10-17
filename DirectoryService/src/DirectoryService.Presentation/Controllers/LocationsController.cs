using DirectoryService.Application;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations;
using DirectoryService.Contracts;
using DirectoryService.Presentation.EndpointResults;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationsController : ControllerBase
{
    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(
        [FromServices] ICommandHandler<Guid, CreateLocationCommand> handler,
        [FromBody] CreateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateLocationCommand(request);
        
        return await handler.Handle(command, cancellationToken);
    }
}