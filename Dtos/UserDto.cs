namespace TestAAIbackend.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
    }

    public class CreateUserDto
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
    }

    public class UpdateUserDto
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
    }
}
