namespace Core;

public interface ITagRepository
{
    Task<(Response, TagDTO)> CreateAsync(TagCreateDTO tag);
    Task<Response> DeleteAsync(int tagId);
    Task<(Response, TagDTO)> ReadAsync(int tagId);
    Task<(Response, IReadOnlyCollection<TagDTO>)> ReadAllAsync();
}