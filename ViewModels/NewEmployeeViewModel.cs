using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace coursach.ViewModels
{
    public class NewEmployeeViewModel
    {
        public int Id { get; set; }
        [Remote(action: "CheckName", controller: "Validation")]
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string Name { get; set; } = null!;

        [Remote(action: "CheckLastName", controller: "Validation")]
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string LastName { get; set; } = null!;

        [Remote(action: "CheckPatronymic", controller: "Validation")]
        public string? Patronymic { get; set; }

        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string Phone { get; set; } = null!;

        [RegularExpression(@"^[\w-]+(\.[\w-]+)*\@([\w-]+\.)+[a-zA-Z]{2,7}$", ErrorMessage = "Введите корректный email адрес")]
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string Login { get; set; } = null!;

        [MinLength(4, ErrorMessage ="Минимальная длина пароля 4 символа")]
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string Password { get; set; } = null!;
        
        [Required(ErrorMessage = "Обязательное поле для заполнения")] 
        public int Role { get; set; }
    }
}
