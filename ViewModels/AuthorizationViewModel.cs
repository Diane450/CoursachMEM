using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace coursach.ViewModels
{
    public class AuthorizationViewModel
    {

        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        [DisplayName("Логин")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        [DisplayName("Пароль")]
        public string? Password { get; set; }
    }
}
