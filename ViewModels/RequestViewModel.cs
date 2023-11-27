using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace coursach.ViewModels
{
    public class RequestViewModel
    {
        [DisplayName("Порядковый номер")]
        public int Id { get; set; }
        [DisplayName("Полное имя клиента")]
        public string FullNameClient { get; set; } = null!;
        [DisplayName("Порядковый номер")]
        public int ClientId { get; set; }
        
        [DisplayName("Техническое задание")]
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string? TechnicalTask { get; set; } = null!;

        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        [DisplayName("Номер телефона")]
        public string Phone { get; set; } = null!;
        
        [DisplayName("Почта")]
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public string Email { get; set; } = null!;
        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }
        
        [DisplayName("Дата начала обработки заявки")]
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        public DateTime? TakeDate { get; set; }

        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        [DisplayName("Статус заявки")]
        public string Status { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        [DisplayName("Статус заявки")]
        public int StatusId { get; set; }

        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        [DisplayName("Сотрудник")]
        public string? EmployeeInf { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле для заполнения")]
        [DisplayName("Сотрудник")]
        public int? EmployeeInfId { get; set; }
    }
}
