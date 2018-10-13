using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using VideoOnDemand.Data.Data.Entities;
using VideoOnDemand.Data.Repositories;
using VideoOnDemand.UI.Models.DTOModels;
using VideoOnDemand.UI.Models.MembershipViewModels;

namespace VideoOnDemand.UI.Controllers
{
    public class MembershipController : Controller
    {
        private string _userId;
        private IMapper _mapper;
        private IReadRepository _readRepository;

        public MembershipController(IHttpContextAccessor httpContextAcessor,
            UserManager<User> userManager,
            IMapper mapper,
            IReadRepository readRepository)
        {
            var user = httpContextAcessor.HttpContext.User;
            _userId = userManager.GetUserId(user);
            _mapper = mapper;
            _readRepository = readRepository;
        }

        public IActionResult Dashboard()
        {
            var courseDtoObjects = _mapper.Map<List<CourseDTO>>(
                _readRepository.GetCourses(_userId));
            var dashboardViewModel = new DashboardViewModel();
            dashboardViewModel.Courses = new List<List<CourseDTO>>();
            var noOfRows = courseDtoObjects.Count <= 3 ? 1 : courseDtoObjects.Count / 3;
            for (int i = 0; i < noOfRows; i++)
            {
                dashboardViewModel.Courses.Add(courseDtoObjects.Take(3).ToList());
            }

            return View(dashboardViewModel);
        }

        public IActionResult Course(int id)
        {
            var course = _readRepository.GetCourse(_userId, id);
            var courseDTO = _mapper.Map<CourseDTO>(course);
            var instructorDTO = _mapper.Map<InstructorDTO>(course.Instructor);
            var moduleListDTO = _mapper.Map<List<ModuleDTO>>(course.Modules);

            for (int i = 0; i < moduleListDTO.Count; i++)
            {
                moduleListDTO[i].Downloads = course.Modules[i].Downloads.Count.Equals(0) ? null
                                                : _mapper.Map<List<DownloadDTO>>(course.Modules[i].Downloads);
                moduleListDTO[i].Videos = course.Modules[i].Videos.Count.Equals(0) ? null
                                                : _mapper.Map<List<VideoDTO>>(course.Modules[i].Videos);
            }

            var courseViewModel = new CourseViewModel
            {
                Course = courseDTO,
                Instructor = instructorDTO,
                Modules = moduleListDTO
            };

            return View(courseViewModel);
        }

        public IActionResult Video(int id)
        {
            var video = _readRepository.GetVideo(_userId, id);
            var course = _readRepository.GetCourse(_userId, video.CourseId);
            var videoDTO = _mapper.Map<VideoDTO>(video);
            var courseDTO = _mapper.Map<CourseDTO>(course);
            var instructorDTO = _mapper.Map<InstructorDTO>(course.Instructor);
            var videos = _readRepository.GetVideos(_userId, video.ModuleId).ToList();
            var count = videos.Count;
            var index = videos.IndexOf(video);
            var previous = videos.ElementAtOrDefault(index - 1);
            var previousId = previous == null ? 0 : previous.Id;
            var next = videos.ElementAtOrDefault(index + 1);
            var nextId = next == null ? 0 : next.Id;
            var nextTitle = next == null ? string.Empty : next.Title;
            var nextThumb = next == null ? string.Empty : next.Thumbnail;

            var videoViewModel = new VideoViewModel
            {
                Video = videoDTO,
                Instructor = instructorDTO,
                Course = courseDTO,
                LessonInfo = new LessonInfoDTO
                {
                    LessonNumber = index + 1,
                    NumberOfLessons = count,
                    NextVideoId = nextId,
                    PreviousVideoId = previousId,
                    NextVideoTitle = nextTitle,
                    NextVideoThumbnail = nextThumb,
                    CurrentVideoTitle = video.Title,
                    CurrentVideoThumbnail = video.Thumbnail
                }
            };

            return View(videoViewModel);
        }
    }
}
