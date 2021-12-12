namespace ProjectBank.Core.DTOs
{
    public record TagDTO
    {
        public int Id { get; init; }

        [Required] public string Value { get; init; }
        
        public int TagGroupId { get; init; } //TODO Skal vi have den her med når den ikke er i entitien? - Den skal nok implementeres mht. det som Mai og Oliver skrev sammen om - eller også skal vi have TagGroupDTOs i ProjectDTO istedet for TagDTOs
    }

    public record TagCreateDTO
    {
        [Required] public string Value { get; init; }
        
        public int TagGroupId { get; init; }
    }
}