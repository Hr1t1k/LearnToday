using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnToday.Models
{
    internal interface IStudent
    {
        List<Student> GetStudentList(); // returns all existing student details
        void NewStudent(Student stud);   //
        Student GetStudentById(int id); //take student id as parameter and return student record
        void UpdateStudentById(int id); // get student id as parameter and update the record of that student
        void DeleteStudentById(int id); // get student id as parameter and delete the record for that student

    }
}
