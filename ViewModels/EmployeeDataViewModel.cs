using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace coursach.ViewModels
{
    public class EmployeeDataViewModel
    {
        [DisplayName("Порядковый номер")]
        public int Id { get; set; }

        [Remote(action: "CheckFullName", controller: "Validation")]
        [DisplayName("Полное имя")]
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string FullName { get; set; } = null!;
        public string Salt { get; set; } = null!;

        [DisplayName("Номер телефона")]
        [MinLength(17,ErrorMessage ="Некорректный номер телефона")]
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string Phone { get; set; } = null!;

        [DisplayName("Почта")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*\@([\w-]+\.)+[a-zA-Z]{2,7}$", ErrorMessage = "Введите корректный email адрес")]
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string Email { get; set; } = null!;

        [DisplayName("Логин")]
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        [Remote(action: "CheckLogin", controller: "Validation", ErrorMessage ="Данный логин уже используется")]
        public string Login { get; set; } = null!;
        
        [DisplayName("Пароль")]
        [MinLength(4, ErrorMessage = "Минимальная длина пароля 4 символа")]
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string Password { get; set; } = null!;
        public string Sallt { get; set; }

        [DisplayName("Роль")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        [DisplayName("Роль")]
        public int RoleId { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string EmployeeInf { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public int EmployeeInfId { get; set; }
    }
}
