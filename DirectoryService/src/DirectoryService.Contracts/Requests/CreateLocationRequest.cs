using DirectoryService.Contracts.DTO;

namespace DirectoryService.Contracts;

public record CreateLocationRequest(string Name, AddressDto Address, string Timezone);