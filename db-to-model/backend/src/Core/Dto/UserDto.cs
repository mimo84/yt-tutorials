namespace FoodDiary.Core.Dto;

public record NewUserDto(string UserName, string Email, string Password, string DisplayName, string Bio = "", string FirstName = "", string FamilyName = "", string Address = "");

public record LoginUserDto(string Email, string Password);

public record UpdatedUserDto(string UserName, string DisplayName, string Bio, string FirstName, string FamilyName, string Address, string Password);

public record UserDto(string DisplayName, string Token, string Bio, string FirstName, string FamilyName, string Address);