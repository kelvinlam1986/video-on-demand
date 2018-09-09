﻿using System.Collections.Generic;
using System.Linq;
using VideoOnDemand.Data.Data.Entities;

namespace VideoOnDemand.Data.Repositories
{
    public class MockReadRepository : IReadRepository
    {
        #region Mock Data 

        List<Course> _courses = new List<Course>
        {
            new Course {
                Id = 1,
                InstructorId = 1,
                MarqueeImageUrl = "/images/laptop.jpg",
                ImageUrl = "/images/course.jpg", Title = "C# For Beginners",
                Description = "Course 1 Description: A very very long description."
            },
            new Course
            {
                Id = 2,
                InstructorId = 1,
                MarqueeImageUrl = "/images/laptop.jpg",
                ImageUrl = "/images/course2.jpg", Title = "Programming C#",
                Description = "Course 2 Description: A very very long description."
            },
            new Course
            {
                Id = 3, InstructorId = 2,
                MarqueeImageUrl = "/images/laptop.jpg",
                ImageUrl = "/images/course3.jpg", Title = "MVC 5 For Beginners",
                Description = "Course 3 Description: A very very long description."
            },
             new Course {
                Id = 4,
                InstructorId = 1,
                MarqueeImageUrl = "/images/laptop.jpg",
                ImageUrl = "/images/course.jpg", Title = "The road to React",
                Description = "Course 1 Description: A very very long description."
            },
            new Course {
            Id = 5,
            InstructorId = 1,
            MarqueeImageUrl = "/images/laptop.jpg",
            ImageUrl = "/images/course.jpg", Title = "The road to Angular",
            Description = "Course 1 Description: A very very long description."
        },
        };

        List<UserCourse> _userCourses = new List<UserCourse>
        {
            new UserCourse { UserId = "3aaf6b47-efd1-4973-8aa5-10a40fc15491", CourseId = 1 },
            new UserCourse { UserId = "00000000-0000-0000-0000-000000000000", CourseId = 2 },
            new UserCourse { UserId = "4ad684f8-bb70-4968-85f8-458aa7dc19a3", CourseId = 3 },
            new UserCourse { UserId = "00000000-0000-0000-0000-000000000000", CourseId = 1 },
            new UserCourse { UserId = "3aaf6b47-efd1-4973-8aa5-10a40fc15491", CourseId = 4 },
            new UserCourse { UserId = "3aaf6b47-efd1-4973-8aa5-10a40fc15491", CourseId = 5 }
        };

        List<Module> _modules = new List<Module>
        {
            new Module { Id = 1, Title = "Module 1", CourseId = 1 },
            new Module { Id = 2, Title = "Module 2", CourseId = 1 },
            new Module { Id = 3, Title = "Module 3", CourseId = 2 }
        };

        List<Download> _downloads = new List<Download>
        {
            new Download {
                Id = 1, ModuleId = 1, CourseId = 1,
                Title = "ADO.NET 1 (PDF)",
                Url = "https://1drv.ms/b/s!AuD5OaH0ExAwn48rX9TZZ3kAOX6Peg" },
            new Download{
                Id = 2, ModuleId = 1, CourseId = 1,
                Title = "ADO.NET 2 (PDF)",
                Url = "https://1drv.ms/b/s!AuD5OaH0ExAwn48rX9TZZ3kAOX6Peg" },
            new Download{
                Id = 3, ModuleId = 3, CourseId = 2,
                Title = "ADO.NET 1 (PDF)",
                Url = "https://1drv.ms/b/s!AuD5OaH0ExAwn48rX9TZZ3kAOX6Peg" }
        };

        List<Instructor> _instructors = new List<Instructor>
        {
            new Instructor{
                Id = 1, Name = "John Doe",
                Thumbnail = "/images/Ice-Age-Scrat-icon.png",
                Description = "Lorem ipsum dolor sit amet, consectetur elit."
            },
            new Instructor{
                Id = 2, Name = "Jane Doe",
                Thumbnail = "/images/Ice-Age-Scrat-icon.png",
                Description = "Lorem ipsum dolor sit, consectetur adipiscing."
            }
        };

        List<Video> _videos = new List<Video>
        {
            new Video {
                Id = 1, ModuleId = 1, CourseId = 1, Position = 1,
                Title = "Video 1 Title", Description = "Video 1 Description: A very very long description.", Duration = 50,
                Thumbnail = "/images/video1.jpg", Url = "http://some_url/manifest"
            },
            new Video {
                Id = 2, ModuleId = 1, CourseId = 1, Position = 2,
                Title = "Video 2 Title", Description = "Video 2 Description: A very very long description.", Duration = 45,
                Thumbnail = "/images/video2.jpg", Url = "http://some_url/manifest"
            },
            new Video {
                Id = 3, ModuleId = 3, CourseId = 2, Position = 1,
                Title = "Video 3 Title", Description = "Video 3 Description: A very very long description.", Duration = 41,
                Thumbnail = "/images/video3.jpg", Url = "http://some_url/manifest"
            },
            new Video {
                Id = 4, ModuleId = 2, CourseId = 1, Position = 1,
                Title = "Video 4 Title", Description = "Video 4 Description: A very very long description.", Duration = 42,
                Thumbnail = "/images/video4.jpg", Url = "http://some_url/manifest"
            }
        };

        #endregion

        public IEnumerable<Course> GetCourses(string userId)
        {
            var courses = _userCourses.Where(x => x.UserId.Equals(userId))
                .Join(_courses, uc => uc.CourseId, c => c.Id,
                (uc, c) => new { Course = c })
                .Select(s => s.Course);
            foreach (var course in courses)
            {
                course.Instructor = _instructors.SingleOrDefault(s => s.Id.Equals(course.InstructorId));
                course.Modules = _modules.Where(m => m.CourseId.Equals(course.Id)).ToList();
            }

            return courses;
        }

        public Course GetCourse(string userId, int courseId)
        {
            var course = _userCourses.Where(x => x.UserId.Equals(userId))
                .Join(_courses, uc => uc.CourseId, c => c.Id,
                (uc, c) => new { Course = c })
                .SingleOrDefault(s => s.Course.Id.Equals(courseId)).Course;
            course.Instructor = _instructors.SingleOrDefault(s => s.Id.Equals(course.InstructorId));
            course.Modules = _modules.Where(m => m.CourseId.Equals(course.Id)).ToList();

            foreach (var module in course.Modules)
            {
                module.Downloads = _downloads.Where(x => x.ModuleId.Equals(x.Id)).ToList();
                module.Videos = _videos.Where(x => x.ModuleId.Equals(x.Id)).ToList();
            }

            return course;
        }

        public Video GetVideo(string userId, int videoId)
        {
            var video = _videos.Where(v => v.Id.Equals(videoId))
                            .Join(_userCourses, v => v.CourseId, uc => uc.CourseId,
                            (v, uc) => new { Video = v, UserCourse = uc })
                            .Where(vuc => vuc.UserCourse.UserId.Equals(userId))
                            .FirstOrDefault().Video;
            return video;
        }

        public IEnumerable<Video> GetVideos(string userId, int moduleId = 0)
        {
            var videos = _videos.Join(_userCourses, v => v.CourseId, uc => uc.CourseId,
                            (v, uc) => new { Video = v, UserCourse = uc })
                            .Where(vuc => vuc.UserCourse.UserId.Equals(userId));

            return moduleId.Equals(0) ?
                    videos.Select(s => s.Video) :
                    videos.Where(x => x.Video.ModuleId.Equals(moduleId))
                    .Select(s => s.Video);

        }
    }
}
