namespace ProjectBank.Core.IRepositories;

public interface ITagRepository
{
    Task<Response> CreateAsync(TagCreateDTO tag);
    Task<Response> DeleteAsync(int tagId);
    Task<Option<TagDTO?>> ReadAsync(int tagId);
    Task<IReadOnlyCollection<TagDTO>> ReadAllAsync();
}