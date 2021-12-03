using ProjectBank.Core.DTOs;

namespace ProjectBank.Core.IRepositories;

public interface ITagGroupRepository
{
    Task<(Response, TagGroupDTO)> CreateAsync(TagGroupCreateDTO tagGroup);
    Task<Response> UpdateAsync(int tagGroupId, TagGroupUpdateDTO tagGroup);
    Task<Response> DeleteAsync(int tagGroupId);
    Task<(Response, TagGroupDTO)> ReadAsync(int tagGroupId);
    Task<(Response, IReadOnlyCollection<TagGroupDTO>)> ReadAllAsync();
}