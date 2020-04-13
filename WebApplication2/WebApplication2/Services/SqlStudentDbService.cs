using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DPOs.Requests;
using WebApp.Models;

namespace WebApp.Services
{
    public class SqlStudentDbService : IStudentDbService
    {

        private string ConnectionString = "Data Source=db-mssql;Initial Catalog=s18511;Integrated Security=True";
        public string EnrollStudent(EnrollStudentRequest req)
        {            
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                StringBuilder res = new StringBuilder();
                connection.Open();
                SqlTransaction trans = connection.BeginTransaction();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Studies WHERE name = @name", connection))
                {
                    command.Parameters.AddWithValue("name", req.Studies);
                    command.Transaction = trans;
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            trans.Rollback();
                            return "Failed";
                        }

                    }
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM Enrollment e INNER JOIN Studies s ON s.idStudy = e.idStudy WHERE s.name = @name;" +
                    " SELECT * FROM Studies WHERE name = @name;" +
                    " SELECT MAX(idEnrollment) + 1 FROM Enrollment;", connection))
                {

                    int IdEnrollment = -1;
                    command.Parameters.AddWithValue("name", req.Studies);
                    command.Transaction = trans;
                    using (var reader = command.ExecuteReader())
                    {
                        DateTime d = Convert.ToDateTime(req.BirthDate);
                        String sDate;
                        sDate = d.Date.ToString("MM-dd-yyyy").Substring(0,d.Date.ToString().IndexOf(" "));
                        reader.Close();
                        command.CommandText = "INSERT INTO Student (IndexNumber,FirstName,LastName,BirthDate,IdEnrollment) " + "VALUES (@index,@first_name,@last_name,'" +sDate + "',@id_enrollment)";
                        command.Parameters.AddWithValue("index", req.IndexNumber);
                        command.Parameters.AddWithValue("first_name", req.FirstName);
                        command.Parameters.AddWithValue("last_name", req.LastName);
                        command.Parameters.AddWithValue("id_enrollment", IdEnrollment);
                        try
                        {
                            command.ExecuteNonQuery();
                        }catch(SqlException e)
                        {
                            trans.Rollback();
                            return "Failed:";
                        }
                        trans.Commit();
                        res.Append("Success");
                        return res.ToString();
                    }
                }
                    
            }

        }

        public Student GetStudent(string indexNumber)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Student WHERE IndexNumber = @index", connection))
                {
                    command.Parameters.AddWithValue("index", indexNumber);
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return null;
                        }
                        else
                        {
                            return new Student
                            {
                                IndexNumber = indexNumber,
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                BirthDate = (DateTime)reader["BirthDate"],
                                IdEnrollment = (int)reader["IdEnrollment"]
                            };
                        }
                    }
                }

            }
        }

        public string PromoteStudents(PromoteStudentRequest request)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("EXECUTE zad5 @studies_name , @semester",connection))
                {

                    command.Parameters.AddWithValue("studies_name", request.Studies);
                    command.Parameters.AddWithValue("semester", request.Semester);
                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {

                            if (!reader.Read())
                            {
                                return "Failed: ";
                            }
                            Enrollment enrollment = new Enrollment
                            {
                                IdEnrollment = (int)reader["IdEnrollment"],
                                Semester = (int)reader["Semester"],
                                IdStudy = (int)reader["IdStudy"],
                                StartDate = (DateTime)reader["StartDate"]
                            };

                            return enrollment.ToString();

                        }
                    }catch(SqlException e)
                    {
                       
                        return "Failed: ";

                    }
;               }
            }
        }
        
    }
}
