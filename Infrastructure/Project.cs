namespace Infrastructure 
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set;}

        [Required]
        [StringLength(400)]
        public string? Description {get; set;}

        //[Required] 
        //public ISet<Supervisor> AssociatedSupervisors {get;set;} //Skal IKKE bruges, da vi kan lave kald, som kan finde infoen.

        // Et Project skal have en liste af tags - EditProjectPage bliver mere seperat fra Projects felter, ift. TagGroups felter, som er meget 1:1 mht EditTagGroupPage)
        [Required]
        public ISet<Tag>? Tags {get; set;} //Associated supervisors bliver fundet ud fra tags. Edit/CreateProjectPage samler alle TagGroups og deres tags, og så giver Project alle de tags som er markeret af brugeren.
    }
}
