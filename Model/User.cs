namespace TestAAIbackend.Models
{
    public class User
    {
        public int Id { get; set; } // PK
        public string Name { get; set; } = null!; // obligatorio en EF
        public string LastName { get; set; } = null!;
    }
}
