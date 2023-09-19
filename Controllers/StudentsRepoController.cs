using LearnToday.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Services;

namespace LearnToday.Controllers
{
    public class StudentsRepoController : ApiController
    {
        static readonly StudentRepository studRepo = new StudentRepository();
        // Get Method (api/StudentsRepo)-> Returns all students present in the Database.
        public IEnumerable<Student> Get()
        {
            List<Student> result = studRepo.GetStudentList();
            return result;
        }
        //Get Method (api/StudentsRepo?id=) -> returns the student details from database with given id if available.
        public HttpResponseMessage Get(int id)
        {
            try
            {
                Student stud = studRepo.GetStudentById(id) ?? throw new HttpResponseException(HttpStatusCode.NotFound);
                return Request.CreateResponse(HttpStatusCode.OK, stud); // if success returns the status code ok and object stud as message.
            }
            catch (WebException ex)
            {
                return Request.CreateResponse(ex.Response); //if any exception occurs return the status code and the message of exception.
            }
        }
        //Delete Method (api/StudentsRepo?id=) -> Deletes the record with the given id.
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                //another method to check if record available or not directly before actually deleting.
                //if (studRepo.GetStudentById(id) == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound,"No enrollment information found.");
                studRepo.DeleteStudentById(id);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (WebException ex)
            {
                return Request.CreateResponse(ex.Response);
            }
        }
        //Put Method (api/StudentsRepo?id=?) -> Checks if the student with given id is present in the DB and then update the name.
        public HttpResponseMessage Put(int id)
        {
            try
            {
                //if (studRepo.GetStudentById(id) == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No student found with given ID.");
                studRepo.UpdateStudentById(id);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (WebException ex)
            {
                return Request.CreateResponse(ex.Response);
            }
        }
        // Post Method ->Insert new record in the database.
        public HttpResponseMessage Post(int id, string name, DateTime dob, decimal height, float weight)
        {
            try
            {
                // if (studRepo.GetStudentById(id) != null) return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Id Already exists");
                Student stud = new Student { StudentId = id, StudentName = name, DateOfBirth = dob, Height = height, Weight = weight };
                studRepo.NewStudent(stud);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (WebException ex)
            {
                return Request.CreateResponse(ex.Response);
            }
        }
    }
}
