using System.Collections.Generic;
using ProjectBank.Core.DTOs;

namespace ProjectBank.Client
{
    public static class FrontendExtensions
    {
        public static TagGroupUpdateDTO ToUpdateDTO(this TagGroupDTO tg)
        {
            return new TagGroupUpdateDTO()
            {
                Id = tg.Id,
                DeletedTagIds = new HashSet<int>(),
                Name = tg.Name,
                NewTagsDTOs = new HashSet<TagCreateDTO>(),
                RequiredInProject = tg.RequiredInProject,
                SupervisorCanAddTag = tg.SupervisorCanAddTag,
                TagLimit = tg.TagLimit,
            };
        }

        public static TagGroupCreateDTO Depress(this TagGroupUpdateDTO tg)
        {
            return tg;
        }
    }
}