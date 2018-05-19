using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using coursesApi.Models;
using studentApi.Models;

namespace Assignment1.Controllers
{
    [Route("api/courses")]
    public class CoursesController : Controller
    {

        

        private static List<Course> _courses = new List<Course>
        {
            new Course {
                ID = 1,
                Name = "Web services",
                TemplateID = "T-514-VEFT",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(3)
            },
            new Course {
                ID = 2,
                Name = "Forritun",
                TemplateID = "F-111-FORR",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(3)
            }
        };

        private static List<Student> _students = new List<Student>
        {
            new Student
            {
                SSN = "0103952839",
                Name = "Haukur Ingi Gunnarsson",
                courseIdLink = 1
            },

            new Student
            {
                SSN = "0101010100",
                Name = "Dr.Einstein",
                courseIdLink = 1
            },
                        
            new Student
            {
                SSN = "2904942309",
                Name = "Kristín Sif Vigfúsdóttir",
                courseIdLink = 2
            }, 

            new Student
            {
                SSN = "1508301599",
                Name = "John Johnson",
                courseIdLink = 2
            }     
        };
            
        

        // GET api/courses
        [HttpGet]
        [Route("")]
        public IActionResult GetCourses()
        {
            return Ok(_courses);
        }

        // GET api/courses/5
        [HttpGet]
        [Route("{courseId:int}", Name = "GetCourseById")]
        public IActionResult GetCourseById(int courseId)
        {
            var result = _courses.SingleOrDefault(x => x.ID == courseId);

            if(result == null) 
            {
                return NotFound();
            }
            
            return Ok(result);
        }

        // POST /api/courses
        [HttpPost]
        [Route("")]
        public IActionResult AddCourse([FromBody] Course course)
        {

            if(!ModelState.IsValid)
            {
                return StatusCode(412);
            }

            _courses.Add(course);

            return CreatedAtRoute("GetCourseById", new {courseId = course.ID}, course);
        }

        // PUT api/courses/5
        [HttpPut]
        [Route("{courseId}")]
        public IActionResult UpdateCourse(int courseId, [FromBody] Course update)
        {
            var course = _courses.SingleOrDefault(x => x.ID == courseId);

            if(course == null)
            {
                return NotFound();
            }

            course.ID = update.ID;
            course.Name = update.Name;
            course.TemplateID = update.TemplateID;
            course.StartDate = update.StartDate;
            course.EndDate = update.EndDate;

            if(!ModelState.IsValid){
                return StatusCode(412);
            }

            return Ok(course);
        }

        // DELETE api/courses/5
        [HttpDelete]
        [Route("{courseId:int}")]
        public IActionResult DeleteCourseById(int courseId)
        {
            var course = _courses.SingleOrDefault(x => x.ID == courseId);

            if(course == null)
            {
                return NotFound("invalid Id");
            }

            _courses.Remove(course);

            return StatusCode(204);
        }

        // Get api/courses/id/students
        [HttpGet]
        [Route("{courseId:int}/students", Name = "GetStudentById")]
        public IActionResult GetStudentsByCourseId(int courseId) 
        {
            var course = _courses.SingleOrDefault(x => x.ID == courseId);

            if(course == null)
            {
                return NotFound("invalid ID");
            }

            var students = _students.Where(x => x.courseIdLink == courseId);

            return Ok(students);
        }

        // Post apu/courses/id
        [HttpPost]
        [Route("{courseId:int}")]
        public IActionResult AddStudentToCourse(int courseId, [FromBody] Student student)
        {
            var course = _courses.SingleOrDefault(x => x.ID == courseId);

            if(course == null)
            {
                return NotFound("invalid ID");
            }

            if(!ModelState.IsValid)
            {
                return StatusCode(412);
            }

            _students.Add(student);

            return CreatedAtRoute("GetStudentById", new {courseId = student.courseIdLink}, student);
        }   
    }
}