using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace LearnToday.Models
{
    public class StudentRepository : IStudent
    {
        //Connection String 
        public string connStr = ConfigurationManager.ConnectionStrings["CollegeDb"].ToString();

        //Delete student record using Delete Method
        public void DeleteStudentById(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Delete from Student where StudentId=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    int result = cmd.ExecuteNonQuery();

                    if (result == 0)
                    {
                        var responsemssg = new HttpResponseMessage(HttpStatusCode.NotFound);
                        responsemssg.Content = new StringContent("No enrollment information found");
                        throw new HttpResponseException(responsemssg);
                    }
                    else if (result > 0) throw new HttpResponseException(HttpStatusCode.OK);
                }
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var responsemssg = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    responsemssg.Content = new StringContent(ex.Message);
                    throw new HttpResponseException(responsemssg);
                }
                else throw ex;
            }
        }

        //Get student detail using Get method 
        public Student GetStudentById(int id)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select * from Student where StudentId=@id order by StudentId", conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                Student stud = new Student();
                if (!reader.HasRows) return null;
                while (reader.Read())
                {
                    stud.StudentId = Int32.Parse(reader[0].ToString());
                    stud.StudentName = reader[1].ToString();
                    stud.DateOfBirth = DateTime.Parse(reader[2].ToString());
                    stud.Height = decimal.Parse(reader[3].ToString());
                    stud.Weight = float.Parse(reader[4].ToString());
                }
                return stud;
            }
        }

        //Get all student list using Get method
        public List<Student> GetStudentList()
        {
            List<Student> students = new List<Student>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select * from Student", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Student stud = new Student();
                    stud.StudentId = Int32.Parse(reader[0].ToString());
                    stud.StudentName = reader[1].ToString();
                    stud.DateOfBirth = DateTime.Parse(reader[2].ToString());
                    stud.Height = decimal.Parse(reader[3].ToString());
                    stud.Weight = float.Parse(reader[4].ToString());
                    students.Add(stud);
                }
            }
            return students;
        }

        //Add new Student using Post method
        public void NewStudent(Student stud)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Insert into student values(@StudentId ,@StudentName,@DateOfBirth,@Height,@Weight)", conn);
                    cmd.Parameters.AddWithValue("@StudentId", stud.StudentId);
                    cmd.Parameters.AddWithValue("@StudentName", stud.StudentName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", stud.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Height", stud.Height);
                    cmd.Parameters.AddWithValue("@Weight", stud.Weight);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {
                        var responsemssg = new HttpResponseMessage(HttpStatusCode.Created);
                        responsemssg.Content = new ObjectContent<Student>(stud, new JsonMediaTypeFormatter());
                        throw new HttpResponseException(responsemssg);

                    }
                }
            }
            catch (SqlException ex)
            {
                // throw ex;
                var responsemssg = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responsemssg.Content = new StringContent(ex.Message);
                throw new HttpResponseException(responsemssg);
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var responsemssg = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    responsemssg.Content = new StringContent(ex.Message);
                    throw new HttpResponseException(responsemssg);
                }
                else throw ex;
            }
        }

        //Update student name using the put method
        public void UpdateStudentById(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Update student set StudentName='XYZ' where StudentId=@id", conn);
                    
                    cmd.Parameters.AddWithValue("@id", id);
                    
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        var responsemssg = new HttpResponseMessage(HttpStatusCode.NotFound);
                        responsemssg.Content = new StringContent("No enrollment information found");
                        throw new HttpResponseException(responsemssg);
                    }
                    else if (rowsAffected > 0) throw new HttpResponseException(HttpStatusCode.OK);
                }
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var responsemssg = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    responsemssg.Content = new StringContent(ex.Message);
                    throw new HttpResponseException(responsemssg);
                }
                else throw ex;
            }
        }
    }
}