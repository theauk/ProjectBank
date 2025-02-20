﻿using ProjectBank.Core.DTOs;

namespace ProjectBank.Client
{
    public static class FrontendExtensions
    {
        public static TagGroupUpdateDTO ToUpdateDto(this TagGroupDTO tg)
        {
            return new TagGroupUpdateDTO()
            {
                Id = tg.Id,
                Name = tg.Name,
                SelectedTagValues = new HashSet<string>(),
                RequiredInProject = tg.RequiredInProject,
                SupervisorCanAddTag = tg.SupervisorCanAddTag,
                TagLimit = tg.TagLimit,
            };
        }
    }
}