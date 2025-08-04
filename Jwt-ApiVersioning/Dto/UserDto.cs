namespace Jwt_ApiVersioning.Dto
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
       
    }

    public class EditUserDto
    {
        public string? Name { get; set; }
        public string? Mobile { get; set; }

    }
}
