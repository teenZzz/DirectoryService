using DirectoryService.Domain.Entities;

namespace DirectoryService.Contracts.Requests;

public record UpdateDepartmentLocationsRequest(Guid DepartmentId, List<Guid> LocationIds);