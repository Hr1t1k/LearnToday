using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnToday.Models
{
    public partial class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal Height { get; set; }
        public float Weight { get; set; }
    }
}