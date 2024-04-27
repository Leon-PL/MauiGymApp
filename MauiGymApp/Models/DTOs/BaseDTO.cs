namespace MauiGymApp.Models.DTOs
{
    /// <summary>
    /// Provides properties to make storing and retrieving objects easier.
    /// </summary>
    public abstract class BaseDTO
    { 
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
    }
}