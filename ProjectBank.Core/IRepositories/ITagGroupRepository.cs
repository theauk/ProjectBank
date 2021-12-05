using ProjectBank.Core.DTOs;

namespace ProjectBank.Core.IRepositories;

public interface ITagGroupRepository
{
    Task<Response> CreateAsync(TagGroupCreateDTO tagGroup);
    Task<Response> UpdateAsync(int tagGroupId, TagGroupUpdateDTO tagGroup);
    Task<Response> DeleteAsync(int tagGroupId);
    Task<TagGroupDTO?> ReadAsync(int tagGroupId);
    Task<IReadOnlyCollection<TagGroupDTO>> ReadAllAsync();
    Task<Response> AddTagAsync(int tagGroupId, int tagId);
    Task<Response> DeleteTagAsync(int tagGroupId, int tagId);
    
}