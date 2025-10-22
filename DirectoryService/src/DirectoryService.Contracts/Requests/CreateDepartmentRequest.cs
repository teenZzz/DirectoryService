using DirectoryService.Domain.Entities;

namespace DirectoryService.Contracts;

public record CreateDepartmentRequest(string Name, string Identifier, Guid? ParentId, List<Guid> LocationIds);