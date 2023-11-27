using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace coursach.ViewModels
{
    public class AuthorizationViewModel
    {

        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string? Password { get; set; }
    }
}
