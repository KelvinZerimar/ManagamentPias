using Newtonsoft.Json;

namespace ManagamentPias.Domain.Entities;

public class User
{
    public User()
    {

    }

    private User(string name, string email, string passwordHash, string role, bool isActive, bool isVerified)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        IsActive = isActive;
        IsVerified = isVerified;
    }

    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("email")]
    public string Email { get; set; } = null!;

    [JsonProperty("passwordHash")]
    public string PasswordHash { get; set; } = null!;// Hash de la contraseña

    [JsonProperty("role")]
    public string Role { get; set; } = null!;// Ejemplo: "Admin", "User", etc.

    [JsonProperty("isActive")]
    public bool IsActive { get; set; } = true; // Indicador de si la cuenta está activa

    [JsonProperty("isVerified")]
    public bool IsVerified { get; set; } = false; // Indicador de si el correo está verificado

    static public User Create(string name, string email, string passwordHash, string role, bool isActive, bool isVerified)
    {
        return new User(name, email, passwordHash, role, isActive, isVerified);
    }
}
