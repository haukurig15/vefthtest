using System;
using System.Collections.Generic;
using System.Linq;
using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Exceptions;
using CoursesAPI.Services.Models.Entities;

namespace CoursesAPI.Services.CoursesServices
{
	public class CoursesServiceProvider
	{
		private readonly IUnitOfWork _uow;

		private readonly IRepository<CourseInstance> _courseInstances;
		private readonly IRepository<TeacherRegistration> _teacherRegistrations;
		private readonly IRepository<CourseTemplate> _courseTemplates; 
		private readonly IRepository<Person> _persons;

		public CoursesServiceProvider(IUnitOfWork uow)
		{
			_uow = uow;

			_courseInstances      = _uow.GetRepository<CourseInstance>();
			_courseTemplates      = _uow.GetRepository<CourseTemplate>();
			_teacherRegistrations = _uow.GetRepository<TeacherRegistration>();
			_persons              = _uow.GetRepository<Person>();
		}

		/// <summary>
		/// You should implement this function, such that all tests will pass.
		/// </summary>
		/// <param name="courseInstanceID">The ID of the course instance which the teacher will be registered to.</param>
		/// <param name="model">The data which indicates which person should be added as a teacher, and in what role.</param>
		/// <returns>Should return basic information about the person.</returns>
		public PersonDTO AddTeacherToCourse(int courseInstanceID, AddTeacherViewModel model)
		{
			//This function should add a person as a teacher to a course.
			//Each course may have more than one teacher.
			//Each course can have either 0 or 1 "main" teacher.
			//Each person can only be registered once as a teacher for a given course

			var teacher = (from p in _persons.All()
							where p.SSN == model.SSN
							select p).SingleOrDefault();

			var course = (from c in _courseInstances.All()
						  where c.ID == courseInstanceID
						  select c).SingleOrDefault();

			var mainTeacher = (from m in _teacherRegistrations.All()
							   where m.Type == TeacherType.MainTeacher
							   && model.Type == TeacherType.MainTeacher
							   && courseInstanceID == m.CourseInstanceID
							   select m).SingleOrDefault();
		
			var teacherAlreadyInCourse = (from m in _teacherRegistrations.All()
							   where model.SSN == m.SSN 
							   && courseInstanceID == m.CourseInstanceID
							   select m).SingleOrDefault();

			
			if (teacher == null){
				throw new AppObjectNotFoundException();	
				
			}

			if(course == null){
				throw new AppObjectNotFoundException();
			}

			if(mainTeacher != null){
				throw new AppValidationException("msg");
			}

			if(teacherAlreadyInCourse != null){
				throw new AppValidationException("msg");
			}


			_teacherRegistrations.Add(new TeacherRegistration 
									{CourseInstanceID = courseInstanceID, 
									SSN = model.SSN,
									Type = model.Type});

			
			_uow.Save();

			return new PersonDTO
            {
                SSN = model.SSN,
                Name = (from p in _persons.All()
                       where model.SSN == p.SSN
                       select p).SingleOrDefault().Name
            };

		}

		/// <summary>
		/// You should write tests for this function. You will also need to
		/// modify it, such that it will correctly return the name of the main
		/// teacher of each course.
		/// </summary>
		/// <param name="semester"></param>
		/// <returns></returns>
		public List<CourseInstanceDTO> GetCourseInstancesBySemester(string semester = null)
		{
			if (string.IsNullOrEmpty(semester))
			{
				semester = "20153";
			}


			var courses = (from c in _courseInstances.All()
				join ct in _courseTemplates.All() on c.CourseID equals ct.CourseID
				where c.SemesterID == semester
				select new CourseInstanceDTO
				{
					Name               = ct.Name,
					TemplateID         = ct.CourseID,
					CourseInstanceID   = c.ID,
					MainTeacher        = (from t in _teacherRegistrations.All()
											join p in _persons.All() on t.SSN equals p.SSN
											join c in _courseInstances.All() 
											on t.CourseInstanceID equals c.ID
											where t.Type == TeacherType.MainTeacher
											&& c.SemesterID == semester
											select p.Name).DefaultIfEmpty("").First()
										
				}).ToList();



			return courses;
		}
	}
}
