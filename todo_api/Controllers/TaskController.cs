using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace todo_api.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class TaskController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TaskController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select 
                                TaskId, 
                                TaskTitle,
                                TaskDescription,
                                TaskPhase
                             from
                                dbo.Task
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TodoAppConnection");
            SqlDataReader myReader;
            using (SqlConnection myConnection = new SqlConnection(sqlDataSource))
            {
                myConnection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConnection))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConnection.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Task task)
        {
            string query = @"
                            insert into dbo.Task
                                values (@TaskTitle, @TaskDescription, @TaskPhase)
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TodoAppConnection");
            SqlDataReader myReader;
            using (SqlConnection myConnection = new SqlConnection(sqlDataSource))
            {
                myConnection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@TaskTitle", task.TaskTitle);
                    myCommand.Parameters.AddWithValue("@TaskDescription", task.TaskDescription);
                    myCommand.Parameters.AddWithValue("@TaskPhase", task.TaskPhase);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConnection.Close();
                }
            }
            return new JsonResult($"Task {task.TaskTitle} added to the Database.");
        }

        [HttpPut]
        public JsonResult Put(Task task)
        {
            string query = @"
                            update dbo.Task
                            set
                            TaskTitle = @TaskTitle,
                            TaskDescription = @TaskDescription,
                            TaskPhase = @TaskPhase
                            where
                            TaskId = @TaskId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TodoAppConnection");
            SqlDataReader myReader;
            using (SqlConnection myConnection = new SqlConnection(sqlDataSource))
            {
                myConnection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@TaskId", task.TaskId);
                    myCommand.Parameters.AddWithValue("@TaskTitle", task.TaskTitle);
                    myCommand.Parameters.AddWithValue("@TaskDescription", task.TaskDescription);
                    myCommand.Parameters.AddWithValue("@TaskPhase", task.TaskPhase);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConnection.Close();
                }
            }
            return new JsonResult($"Task {task.TaskId} has changed to {task.TaskTitle}.");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                            delete from dbo.Task
                            where TaskId = @TaskId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TodoAppConnection");
            SqlDataReader myReader;
            using (SqlConnection myConnection = new SqlConnection(sqlDataSource))
            {
                myConnection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConnection))
                {
                    myCommand.Parameters.AddWithValue("@TaskId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConnection.Close();
                }
            }
            return new JsonResult($"Task {id} has been deleted.");
        }
    }
}
