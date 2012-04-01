using System.Linq;
using System.Web.Mvc;
using Scrumee.Data.Entities;
using Scrumee.Repositories.Interfaces;

namespace Scrumee.Web.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<UserStory> _userStoryRepository;
        private readonly IRepository<Task> _taskRepository;
        private readonly IRepository<User> _userRepository;

        public ProjectsController( IRepository<Project> projectRepository, IRepository<UserStory> userStoryRepository,
                                   IRepository<Task> taskRepository, IRepository<User> userRepository )
        {
            _projectRepository = projectRepository;
            _userStoryRepository = userStoryRepository;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        #region Views

        public ActionResult Index()
        {
            return RedirectToAction( "All" );
        }

        public ActionResult All()
        {
            var projects = _projectRepository
                .Where( x => x.Id > 0 )
                .ToList();

            return View( projects );
        }

        public ActionResult ProjectDetails( int id )
        {
            var project = _projectRepository.Get( id );

            return View( project );
        }

        public ActionResult UserStoryDetails( int id )
        {
            var userStory = _userStoryRepository.Get( id );

            ViewBag.Users = _userRepository
                .Where( x => x.Id > 0 )
                .ToList();

            return View( userStory );
        }

        public ActionResult TaskDetails( int id )
        {
            var task = _taskRepository.Get( id );

            ViewBag.Users = _userRepository
                .Where( x => x.Id > 0 )
                .ToList();

            return View( task );
        }

        public ActionResult AddNewUser()
        {
            return View();
        }

        #endregion Views

        #region Add

        public ActionResult AddUser( string firstName, string lastName )
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName
            };

            _userRepository.Add( user );

            return RedirectToAction( "All" );
        }

        public ActionResult AddProject( string name )
        {
            var project = new Project
            {
                Name = name
            };

            _projectRepository.Add( project );

            return RedirectToAction( "All" );
        }

        public ActionResult AddUserStory( int projectId, string name )
        {
            var project = _projectRepository.Get( projectId );

            var userStory = new UserStory
            {
                Name = name,
                Project = project
            };

            project.UserStories.Add( userStory );

            _projectRepository.Add( project );

            return RedirectToAction( "ProjectDetails", new { id = project.Id } );
        }

        public ActionResult AddTask( int userStoryId, string name, int userId )
        {
            var userStory = _userStoryRepository.Get( userStoryId );

            var task = new Task
            {
                Name = name,
                UserStory = userStory
            };

            if ( userId != 0 )
            {
                var user = _userRepository.Get( userId );
                task.User = user;
            }

            userStory.Tasks.Add( task );

            _userStoryRepository.Add( userStory );

            return RedirectToAction( "UserStoryDetails", new { id = userStory.Id } );
        }

        #endregion Add

        #region Update

        public ActionResult UpdateProject( int projectId, string name )
        {
            var project = _projectRepository.Get( projectId );

            project.Name = name;

            _projectRepository.Add( project );

            return RedirectToAction( "All" );
        }

        public ActionResult UpdateUserStory( int userStoryId, string name )
        {
            var userStory = _userStoryRepository.Get( userStoryId );

            userStory.Name = name;

            _userStoryRepository.Add( userStory );

            return RedirectToAction( "ProjectDetails", new { id = userStory.Project.Id } );
        }

        public ActionResult UpdateTask( int taskId, string name, int userId )
        {
            var task = _taskRepository.Get( taskId );

            task.Name = name;

            if ( userId != 0 )
            {
                var user = _userRepository.Get( userId );
                task.User = user;
            }
            else
            {
                task.User = null;
            }

            _taskRepository.Add( task );

            return RedirectToAction( "UserStoryDetails", new { id = task.UserStory.Id } );
        }

        #endregion Update

        #region Delete

        public ActionResult DeleteProject( int projectId )
        {
            var project = _projectRepository.Get( projectId );

            _projectRepository.Remove( project );

            return RedirectToAction( "All" );
        }

        public ActionResult DeleteUserStory( int userStoryId )
        {
            var userStory = _userStoryRepository.Get( userStoryId );

            _userStoryRepository.Remove( userStory );

            return RedirectToAction( "ProjectDetails", new { id = userStory.Project.Id } );
        }

        public ActionResult DeleteTask( int taskId )
        {
            var task = _taskRepository.Get( taskId );
            
            _taskRepository.Remove( task );

            return RedirectToAction( "UserStoryDetails", new { id = task.UserStory.Id } );
        }

        #endregion Delete

    }
}
