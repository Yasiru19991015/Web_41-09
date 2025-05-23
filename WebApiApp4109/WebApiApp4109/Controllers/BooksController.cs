using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApiApp4109.Models;
using WebApiApp4109.Util;

namespace WebApiApp4109.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly DBConnection db = new DBConnection();

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var list = new List<Books>();
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "SELECT * FROM Books";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Books
                {
                    BookId = (int)reader["BookId"],
                    Title = reader["Title"].ToString(),
                    Author = reader["Author"].ToString(),
                    ISBN = reader["ISBN"].ToString(),
                    YearPublished = (int)reader["YearPublished"],
                    Publisher = reader["Publisher"].ToString(),
                    Category = reader["Category"].ToString(),
                    ShelfLocation = reader["ShelfLocation"].ToString()
                });
            }


            return Ok(list);
        }
        [HttpGet("get/{id}")]
        public IActionResult GetById(int id)
        {
            Books emp = null;
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "SELECT * FROM Books WHERE BookId = @id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                emp = new Books
                {
                    BookId = (int)reader["BookId"],
                    Title = reader["Title"].ToString(),
                    Author = reader["Author"].ToString(),
                    ISBN = reader["ISBN"].ToString(),
                    YearPublished = (int)reader["YearPublished"],
                    Publisher = reader["Publisher"].ToString(),
                    Category = reader["Category"].ToString(),
                    ShelfLocation = reader["ShelfLocation"].ToString()
                };
            }

            db.ConClose();
            return emp == null ? NotFound("Book not found") : Ok(emp);
        }

        [HttpPost("add")]
        public IActionResult AddBook([FromBody] Books emp)
        {
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "INSERT INTO Books (Title, Author, ISBN, YearPublished, Publisher, Category, ShelfLocation) " +
                         "VALUES (@Title,@Author, @ISBN, @YearPublished, @Publisher, @Category, @ShelfLocation)";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Title", emp.Title);
            cmd.Parameters.AddWithValue("@Author", emp.Author);
            cmd.Parameters.AddWithValue("@ISBN", emp.ISBN);
            cmd.Parameters.AddWithValue("@YearPublished", emp.YearPublished);
            cmd.Parameters.AddWithValue("@Publisher", emp.Publisher);
            cmd.Parameters.AddWithValue("@Category", emp.Category);
            cmd.Parameters.AddWithValue("@ShelfLocation", emp.ShelfLocation);

            int rows = cmd.ExecuteNonQuery();
            db.ConClose();

            return rows > 0 ? Ok("Book added successfully") : StatusCode(500, "Error adding Book");
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteBook(int id)
        {
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "DELETE FROM Books WHERE BookId = @Id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();
            db.ConClose();

            return rows > 0 ? Ok("Book deleted successfully") : NotFound("Book not found");
        }

    }
}
