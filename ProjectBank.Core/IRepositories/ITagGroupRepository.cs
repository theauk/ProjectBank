namespace ProjectBank.Core.IRepositories;

public interface ITagGroupRepository
{
    Task<Response> CreateAsync(TagGroupCreateDTO tagGroup);
    Task<Response> UpdateAsync(int tagGroupId, TagGroupUpdateDTO tagGroup);
    Task<Response> DeleteAsync(int tagGroupId);
    Task<Option<TagGroupDTO?>> ReadAsync(int tagGroupId);
    Task<IReadOnlyCollection<TagGroupDTO>> ReadAllAsync();
}