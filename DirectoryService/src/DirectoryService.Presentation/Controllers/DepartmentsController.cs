using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Departments;
using DirectoryService.Application.Locations;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Requests;
using DirectoryService.Presentation.EndpointResults;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentsController
{
    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(
        [FromServices] ICommandHandler<Guid, CreateDepartmentCommand> handler,
        [FromBody] CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateDepartmentCommand(request);
        
        return await handler.Handle(command, cancellationToken);
    }
    
    [HttpPut("{departmentId}/locations")]
    public async Task<EndpointResult<Guid>> UpdateDepartmentLocations(
        Guid departmentId,
        [FromServices] ICommandHandler<Guid, UpdateDepartmentLocationsCommand> handler,
        [FromBody] List<Guid> locationIds,
        CancellationToken cancellationToken)
    {
        var request = new UpdateDepartmentLocationsRequest(departmentId, locationIds);
        var command = new UpdateDepartmentLocationsCommand(request);

        return await handler.Handle(command, cancellationToken);
    }
}