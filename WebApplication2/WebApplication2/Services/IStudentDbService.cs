using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DPOs.Requests;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IStudentDbService
    {

        string EnrollStudent(EnrollStudentRequest req);
        string PromoteStudents(PromoteStudentRequest req);
        Student GetStudent(string indexNumber);
    }
}
