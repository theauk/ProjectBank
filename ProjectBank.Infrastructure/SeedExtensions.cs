using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProjectBank.Infrastructure;

public static class SeedExtensions
{
    public static async Task<IHost> SeedAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ProjectBankContext>();

            await SeedProjectBankAsync(context);
        }

        return host;
    }

    private static async Task SeedProjectBankAsync(ProjectBankContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Universities.AnyAsync())
        {
            // USERS
            var gustav = new User { Name = "Gustav Metnik-Beck", Email = "gume@itu.dk", Role = Role.Admin };
            var viktor = new User { Name = "Viktor Mønster", Email = "vikm@itu.dk", Role = Role.Admin };
            var mai = new User { Name = "Mai Sigurd", Email = "maod@itu.dk", Role = Role.Admin };
            var oliver = new User { Name = "Oliver Nord", Email = "olno@itu.dk", Role = Role.Admin };
            var joanna = new User { Name = "Joanna Laursen", Email = "jskl@itu.dk", Role = Role.Admin };
            var morten = new User { Name = "Morten Nielsen", Email = "morten@itu.dk", Role = Role.Supervisor };
            var kasper = new User { Name = "Kasper Mørk Jensen", Email = "kasper@itu.dk", Role = Role.Supervisor };
            var cecilie = new User { Name = "Cecilie Rønberg", Email = "cecilie@itu.dk", Role = Role.Supervisor };
            var carl = new User { Name = "Carl Vestebryg", Email = "carl@itu.dk", Role = Role.Supervisor };
            var sille = new User { Name = "Sille Mortensen", Email = "sille@itu.dk", Role = Role.Supervisor };
            var josefine = new User { Name = "Josefine Nørgaard", Email = "josf@itu.dk", Role = Role.Supervisor };
            var paolo = new User { Name = "Paolo Tell", Email = "pate@itu.dk", Role = Role.Admin }; // todo: update
            var rasmus = new User { Name = "Rasmus Lystrøm", Email = "rnie@itu.dk", Role = Role.Admin };
            var soren = new User { Name = "Søren Brostrøm", Email = "soren@itu.dk", Role = Role.Student };

            var peanutbutter = new User { Name = "Peanut Butter", Email = "peanutbutterpb@hotmail.com", Role = Role.Admin };

            var usersSet = new HashSet<User>()
            {
                gustav,
                viktor,
                mai,
                oliver,
                joanna,
                morten,
                kasper,
                cecilie,
                carl,
                sille,
                josefine,
                paolo,
                rasmus,
                soren
            };

            // TAGS
            // Semesters
            var firstSemester = new Tag { Value = "1. Semester" };
            var secondSemester = new Tag { Value = "2. Semester" };
            var thirdSemster = new Tag { Value = "3. Semester" };
            var fourthSemester = new Tag { Value = "4. Semester" };
            var fifthSemester = new Tag { Value = "5. Semester" };
            var sixthSemester = new Tag { Value = "6. Semester" };
            var spring = new Tag { Value = "Spring" };
            var fall = new Tag { Value = "Fall" };

            // Subjects
            var discMathSubject = new Tag { Value = "Discrete Maths" };
            var teamComSubject = new Tag { Value = "Teamwork and communication" };
            var basicProSubject = new Tag { Value = "Basic programing with project" };
            var uXSubject = new Tag { Value = "User Experience and webprograming" };
            var bachelorSubject = new Tag { Value = "Bachelor project" };
            var reflecItSubject = new Tag { Value = "Reflection over IT" };
            var functProSubject = new Tag { Value = "Functionel programmering" };
            var algoSubject = new Tag { Value = "Algorithms and Data structures" };

            // Level
            var bachelorLevel = new Tag { Value = "Bachelor" };
            var masterLevel = new Tag { Value = "MSc. Master" };
            var phdLevel = new Tag { Value = "Phd" };

            // Language
            var danishLanguage = new Tag { Value = "Danish" };
            var englishLanguage = new Tag { Value = "English" };

            // Programme
            var bDataProgramme = new Tag { Value = "BSc in Data Science" };
            var bDigiProgramme = new Tag { Value = "BSc in Digital Design and Interactive Technologies" };
            var bGlobalProgramme = new Tag { Value = "BSc in Global Business Informatics" };
            var bSWUProgramme = new Tag { Value = "BSc in Software Development" };
            var mComScienceProgramme = new Tag { Value = "MSc in Computer Science" };
            var mDataProgramme = new Tag { Value = "MSc in Data Science" };
            var mDigiProgramme = new Tag { Value = "MSc in Digital Design and Communication" };

            // Programming Languages
            var javaProLang = new Tag { Value = "Java" };
            var cSharpProLang = new Tag { Value = "C#" };
            var cPlusProLang = new Tag { Value = "C++" };
            var sqlProLang = new Tag { Value = "SQL" };
            var pythonProLang = new Tag { Value = "Python" };
            var goProLang = new Tag { Value = "GoLang" };
            var rubyProLang = new Tag { Value = "Ruby" };
            var jsProLang = new Tag { Value = "JavaScript" };
            var fSharpProLang = new Tag { Value = "F#" };
            var haskelProLang = new Tag { Value = "Haskel" };

            // ECTS
            var sevenEcts = new Tag { Value = "7.5" };
            var fifteenEcts = new Tag { Value = "15" };

            // Topics
            var dbTopic = new Tag { Value = "Databases" };
            var aiTopic = new Tag { Value = "Artificial Intelligence" };
            var webTopic = new Tag { Value = "Web Development" };
            var logicTopic = new Tag { Value = "Logic" };
            var algorithmsTopic = new Tag { Value = "Algorithms" };

            // Fake
            var fake = new Tag { Value = "Fake" };

            // TAGGROUPS
            var semesterTG = new TagGroup
            {
                Name = "Semester",
                RequiredInProject = true,
                SupervisorCanAddTag = false,
                TagLimit = 4,
                Tags = new HashSet<Tag>()
                {
                    firstSemester,
                    secondSemester,
                    thirdSemster,
                    fourthSemester,
                    fifthSemester,
                    sixthSemester,
                    spring,
                    fall
                }
            };

            var subjectTG = new TagGroup
            {
                Name = "Subject",
                RequiredInProject = false,
                SupervisorCanAddTag = true,
                TagLimit = 1,
                Tags = new HashSet<Tag>()
                {
                    discMathSubject,
                    teamComSubject,
                    basicProSubject,
                    uXSubject,
                    bachelorSubject,
                    reflecItSubject,
                    functProSubject,
                    algoSubject,
                }
            };

            var levelTG = new TagGroup
            {
                Name = "Level",
                RequiredInProject = false,
                SupervisorCanAddTag = false,
                TagLimit = 3,
                Tags = new HashSet<Tag>()
                {
                    bachelorLevel,
                    masterLevel,
                    phdLevel,
                }
            };

            var languageTG = new TagGroup
            {
                Name = "Language",
                RequiredInProject = true,
                SupervisorCanAddTag = true,
                TagLimit = 1,
                Tags = new HashSet<Tag>()
                {
                    danishLanguage,
                    englishLanguage,
                }
            };

            var programmeTG = new TagGroup
            {
                Name = "Programme",
                RequiredInProject = false,
                SupervisorCanAddTag = false,
                TagLimit = 10,
                Tags = new HashSet<Tag>()
                {
                    bDataProgramme,
                    bDigiProgramme,
                    bGlobalProgramme,
                    bSWUProgramme,
                    mComScienceProgramme,
                    mDataProgramme,
                    mDigiProgramme,
                }
            };

            var proLangTG = new TagGroup
            {
                Name = "Programming Language",
                RequiredInProject = false,
                SupervisorCanAddTag = true,
                TagLimit = 2,
                Tags = new HashSet<Tag>()
                {
                    javaProLang,
                    cSharpProLang,
                    cPlusProLang,
                    sqlProLang,
                    pythonProLang,
                    goProLang,
                    rubyProLang,
                    jsProLang,
                    fSharpProLang,
                    haskelProLang,
                }
            };

            var ectsTG = new TagGroup
            {
                Name = "ECTS",
                RequiredInProject = false,
                SupervisorCanAddTag = false,
                TagLimit = 1,
                Tags = new HashSet<Tag>()
                {
                    sevenEcts,
                    fifteenEcts,
                }
            };

            var topicsTG = new TagGroup
            {
                Name = "Topics",
                RequiredInProject = false,
                SupervisorCanAddTag = true,
                TagLimit = 10,
                Tags = new HashSet<Tag>()
                {
                    dbTopic,
                    aiTopic,
                    webTopic,
                    logicTopic,
                    algorithmsTopic
                }
            };

            var tagGroupsSet = new HashSet<TagGroup>()
            {
                semesterTG,
                subjectTG,
                levelTG,
                languageTG,
                programmeTG,
                proLangTG,
                ectsTG,
                topicsTG
            };

            var fakeTG = new TagGroup
            {
                Name = "Fake Filter",
                RequiredInProject = false,
                SupervisorCanAddTag = false,
                TagLimit = null,
                Tags = new HashSet<Tag>() { fake }
            };

            // PROJECTS
            var firstYearP = new Project
            {
                Name = "1st year project",
                Description = "This report talks about the proces of programing a streaming service, in java.",
                Tags = new HashSet<Tag>() { secondSemester, spring, uXSubject, danishLanguage, javaProLang, algorithmsTopic },
                Supervisors = new HashSet<User>() { kasper }
            };

            var secondYearP = new Project
            {
                Name = "2nd year project",
                Description = "In this project we made a program for a corporation. We chose Microsoft and made them a algorithm.",
                Tags = new HashSet<Tag>() { fourthSemester, spring, functProSubject, danishLanguage, fSharpProLang },
                Supervisors = new HashSet<User>() { viktor, cecilie }
            };

            var projectBankP = new Project
            {
                Name = "Project Bank",
                Description = "In this project, you have to implement a service for students and teachers to create and share project thesis.",
                Tags = new HashSet<Tag>() { thirdSemster, fall, danishLanguage, cSharpProLang, webTopic },
                Supervisors = new HashSet<User>() { gustav, mai }
            };

            var coqP = new Project
            {
                Name = "Coq Proof Service",
                Description = "You will implement a program from scratch in Java using Coq to prove mathematical theorems.",
                Tags = new HashSet<Tag>() { firstSemester, fall, danishLanguage, javaProLang, discMathSubject, teamComSubject, logicTopic, algorithmsTopic },
                Supervisors = new HashSet<User>() { sille, josefine }
            };

            var dataPlatformP = new Project
            {
                Name = "Data Platforms",
                Description = "You will in this project have to implement a platform for linking pictures to text in GoLang.",
                Tags = new HashSet<Tag>() { fifthSemester, fall, danishLanguage, goProLang, webTopic, dbTopic },
                Supervisors = new HashSet<User>() { paolo, rasmus }
            };

            var bachelorP = new Project
            {
                Name = "Bachelor Project",
                Description = "This is your official bachelor's project.",
                Tags = new HashSet<Tag>() { sixthSemester, spring, englishLanguage, bachelorLevel, reflecItSubject },
                Supervisors = new HashSet<User>() { morten }
            };

            var masterP = new Project
            {
                Name = "Master Project",
                Description = "This project wil discuss the problems associate with interface and usability. As well as presenting a possible solution to the problem. ",
                Tags = new HashSet<Tag>() { fall, spring, englishLanguage, masterLevel, pythonProLang },
                Supervisors = new HashSet<User>() { kasper }
            };

            var eyeP = new Project
            {
                Name = "Eye Tracker AI",
                Description = "This project is all about AIs and the principles of Machine Learning. You will have to implement an AI being able to track and predict eye movements.",
                Tags = new HashSet<Tag>() { haskelProLang, phdLevel, englishLanguage, fall, spring, aiTopic },
                Supervisors = new HashSet<User>() { cecilie }
            };

            var EHRP = new Project
            {
                Name = "EHR System ",
                Description = "In this project you are tasked with making an electronic health record for a small clinic with minimal security.",
                Tags = new HashSet<Tag>() { fifthSemester, fall, englishLanguage, javaProLang, jsProLang, sqlProLang, webTopic, dbTopic },
                Supervisors = new HashSet<User>() { carl }
            };

            var ituWebSiteP = new Project
            {
                Name = "New ITU Website",
                Description = "You will have to implement, test and maintain a new website replacing the very broken Learnit.",
                Tags = new HashSet<Tag>() { cSharpProLang, sixthSemester, mDigiProgramme, danishLanguage, webTopic, dbTopic },
                Supervisors = new HashSet<User>() { rasmus }
            };

            var ituAlgorithms = new Project
            {
                Name = "Kattis Problems",
                Description = "You will have to develop various new Kattis exercises for different programming levels.",
                Tags = new HashSet<Tag>() { fourthSemester, englishLanguage, algorithmsTopic },
                Supervisors = new HashSet<User>() { cecilie }
            };

            var ituMoonServers = new Project
            {
                Name = "Space Servers",
                Description = "This project focuses on exploring the advantages and possible drawbacks of placing servers on the moon.",
                Tags = new HashSet<Tag>() { sixthSemester, englishLanguage, danishLanguage },
                Supervisors = new HashSet<User>() { carl }
            };

            var fakeProject = new Project
            {
                Name = "Fake Project",
                Description = "This is a fake project for testing",
                Tags = new HashSet<Tag>(),
                Supervisors = new HashSet<User>() { peanutbutter }
            };

            var projectsSet = new HashSet<Project>()
            {
                firstYearP,
                secondYearP,
                projectBankP,
                coqP,
                dataPlatformP,
                bachelorP,
                masterP,
                eyeP,
                EHRP,
                ituWebSiteP,
                ituAlgorithms,
                ituMoonServers
            };

            // UNIVERSITIES
            var ituUni = new University
            {
                DomainName = "itu.dk",
                TagGroups = tagGroupsSet,
                Users = usersSet,
                Projects = projectsSet
            };

            var fakeUni = new University
            {
                DomainName = "hotmail.com",
                TagGroups = new HashSet<TagGroup> { fakeTG },
                Users = new HashSet<User> { peanutbutter },
                Projects = new HashSet<Project> { fakeProject }
            };

            context.Universities.AddRange(ituUni, fakeUni);
        }

        await context.SaveChangesAsync();
    }
}
