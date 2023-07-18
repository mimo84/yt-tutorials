using System.ComponentModel.DataAnnotations;

namespace FoodDiary.Core.Models;

public record UserEnvelope<T>([Required] T User);
