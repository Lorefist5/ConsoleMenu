using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMenu.Test.Model;

public class Course
{
    public string CourseName { get; set; }
    public string CourseCode { get; set; }
    public int Credits { get; set; }
    public string Instructor { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }

    public override string ToString()
    {
        return $@"
    Course Information
    ------------------
    Course Name    : {CourseName}
    Course Code    : {CourseCode}
    Credits        : {Credits}
    Instructor     : {Instructor}
    Start Date     : {StartDate.ToShortDateString()}
    End Date       : {EndDate.ToShortDateString()}
    Description    : {Description}
    ";
    }
}