using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProjectBank.Infrastructure
{
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

            // USERS
            var gustav = new User { Name = "Gustav Metnik-Beck" };
            var viktor = new User { Name = "Viktor Mønster" };
            var thea = new User { Name = "Thea Kjeldsmark" };
            var mai = new User { Name = "Mai Sigurd" };
            var oliver = new User { Name = "Oliver Nord" };
            var joanna = new User { Name = "Joanna Laursen" };
            var morten = new User { Name = "Morten Nielsen" };
            var kasper = new User { Name = "Kasper Mørk Jensen" };
            var cecilie = new User { Name = "Cecilie Rønberg" };
            var carl = new User { Name = "Carl Vestebryg" };
            var sille = new User { Name = "Sille Mortensen" };
            var josefine = new User { Name = "Josefine Nørgaard" };
            var paolo = new User { Name = "Paolo Tell" };
            var rasmus = new User { Name = "Rasmus Lystrøm" };

            // TAGS
            // Semesters
            var firstSemester =  new Tag { Value = "1. Semester" };
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

            // TagGroups
            if (!await context.TagGroups.AnyAsync())
            {
                context.TagGroups.AddRange(
                    new TagGroup { Name = "Semester", RequiredInProject = true, SupervisorCanAddTag = false, TagLimit = 4, Tags = new HashSet<Tag>() { 
                        firstSemester,
                        secondSemester,
                        thirdSemster,
                        fourthSemester,
                        fifthSemester,
                        sixthSemester,
                        spring,
                        fall
                    }},
                    new TagGroup { Name = "Subject", RequiredInProject = false, SupervisorCanAddTag = true, TagLimit = 1, Tags = new HashSet<Tag>() {
                        discMathSubject,
                        teamComSubject,
                        basicProSubject,
                        uXSubject,
                        bachelorSubject,
                        reflecItSubject,
                        functProSubject,
                        algoSubject,
                    }},
                    new TagGroup { Name = "Level", RequiredInProject = false, SupervisorCanAddTag = false, TagLimit = 3, Tags = new HashSet<Tag>() {
                        bachelorLevel,
                        masterLevel,
                        phdLevel,
                    }},
                    new TagGroup { Name = "Language", RequiredInProject = true, SupervisorCanAddTag = true, TagLimit = 1, Tags = new HashSet<Tag>() {
                        danishLanguage,
                        englishLanguage,
                    }},
                    new TagGroup { Name = "Programme", RequiredInProject = false, SupervisorCanAddTag = false, TagLimit = 10, Tags = new HashSet<Tag>() {
                        bDataProgramme,
                        bDigiProgramme,
                        bGlobalProgramme,
                        bSWUProgramme,
                        mComScienceProgramme,
                        mDataProgramme,
                        mDigiProgramme,
                    }},
                    new TagGroup { Name = "Programming Language", RequiredInProject = false, SupervisorCanAddTag = true, TagLimit = 2, Tags = new HashSet<Tag>() {
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
                    }},
                    new TagGroup { Name = "ECTS", RequiredInProject = false, SupervisorCanAddTag = false, TagLimit = 1, Tags = new HashSet<Tag>() {
                        sevenEcts,
                        fifteenEcts,
                    }},
                    new TagGroup { Name = "Topics", RequiredInProject = false, SupervisorCanAddTag = true, TagLimit = 10, Tags = new HashSet<Tag>() {
                        dbTopic,
                    }}
                );
            }

            // Create users
            if (!await context.Users.AnyAsync())
            {
                context.Users.AddRange(
                    gustav,
                    viktor,
                    thea,
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
                    rasmus
                );
            }

            // Create Projects
            if (!await context.Projects.AnyAsync())
            {
                context.Projects.AddRange(
                    new Project 
                    { 
                        Name = "1st year project", 
                        Description = "This report talks about the proces of programing a streaming service, in java.", 
                        Tags = new HashSet<Tag>() { secondSemester, spring, uXSubject, danishLanguage, javaProLang }, 
                        Supervisors = new HashSet<User>() { kasper, cecilie } 
                    },
                    new Project 
                    { 
                        Name = "2nd year project", 
                        Description = "In this project we made a program for a corporation. We chose Microsoft and made them a algorithm.", 
                        Tags = new HashSet<Tag>() { fourthSemester, spring, functProSubject, danishLanguage, fSharpProLang }, 
                        Supervisors = new HashSet<User>() { viktor, thea } 
                    },
                    new Project 
                    { 
                        Name = "Project Bank", 
                        Description = "In this project, you have to implement a service for students and teachers to create and share project thesis.", 
                        Tags = new HashSet<Tag>() { thirdSemster, fall, danishLanguage, cSharpProLang }, 
                        Supervisors = new HashSet<User>() { gustav, mai } 
                    },
                    new Project 
                    { 
                        Name = "Coq Proofer", 
                        Description = "You will implement a program from scratch in Java using Coq to prove mathematical theorems.", 
                        Tags = new HashSet<Tag>() { firstSemester, fall, danishLanguage, javaProLang, discMathSubject, teamComSubject }, 
                        Supervisors = new HashSet<User>() { sille, josefine } 
                    },
                    new Project 
                    { 
                        Name = "Data Platforms", 
                        Description = "You will in this project have to implement a platform for linking pictures to text in GoLang.", 
                        Tags = new HashSet<Tag>() { fifthSemester, fall, danishLanguage, goProLang }, 
                        Supervisors = new HashSet<User>() { paolo, rasmus } 
                    },
                    new Project 
                    { 
                        Name = "Bachelor Project", 
                        Description = "This is your official bachelor project.", 
                        Tags = new HashSet<Tag>() { sixthSemester, spring, englishLanguage, bachelorLevel, reflecItSubject }, 
                        Supervisors = new HashSet<User>() { morten } 
                    },
                    new Project 
                    { 
                        Name = "Master Project", 
                        Description = "This master wil discuss the problems associeret with interface and userbility. Aswell as presending a possible solution to the problem. ", 
                        Tags = new HashSet<Tag>() { fall, spring, englishLanguage, masterLevel, pythonProLang }, 
                        Supervisors = new HashSet<User>() { kasper } 
                    },
                    new Project 
                    { 
                        Name = "Eye Tracker AI", 
                        Description = "This project is all about AIs and the principles of Machine Learning. You will have to implement an AI being able to track and predict eye movements.", 
                        Tags = new HashSet<Tag>() { haskelProLang, phdLevel, englishLanguage, fall, spring }, 
                        Supervisors = new HashSet<User>() { cecilie } 
                    },
                    new Project 
                    { 
                        Name = "EHR system ", 
                        Description = "In this project you aretasked with making an electronic health record for a small clinic with minimal security.", 
                        Tags = new HashSet<Tag>() { fifthSemester, fall, englishLanguage, javaProLang, jsProLang, sqlProLang }, 
                        Supervisors = new HashSet<User>() { carl } 
                    },
                    new Project 
                    { 
                        Name = "New ITU Website", 
                        Description = "You will have to implement, test and maintain a new website replacing the very broken Learnit.", 
                        Tags = new HashSet<Tag>() { cSharpProLang, sixthSemester, mDigiProgramme, danishLanguage }, 
                        Supervisors = new HashSet<User>() { rasmus } 
                    }
                );
            }

            // Create universities
            if (!await context.Universities.AnyAsync())
            {
                context.Universities.Add(new University { DomainName = "itu.dk", TagGroups = context.TagGroups.Select(tg => tg).ToHashSet(), Users = context.Users.Select(u => u).ToHashSet(), Projects = context.Projects.Select(p => p).ToHashSet()});
            }

            await context.SaveChangesAsync();
        }
    }
}
