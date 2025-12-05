using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterFloor.Model;

namespace MasterFloor.Service
{
    public class DataValidator
    {
        public (bool isValid, List<string> errors) PartnerValidator(partners partner)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(partner.title) || partner.title.Length < 3 || partner.title.Length > 60)
                errors.Add("Название должно содержать от 3 до 60 символов");

            if (partner.type == 0)
                errors.Add("Укажите тип партнера");

            if (string.IsNullOrEmpty(partner.director) || partner.director.Length < 3 || partner.director.Length > 90)
                errors.Add("ФИО директора должно содержать от 3 до 90 символов");

            if (string.IsNullOrEmpty(partner.email) || partner.email.Length < 5 || partner.email.Length > 100)
                errors.Add("Email должен содержать от 5 до 100 символов");

            if (string.IsNullOrEmpty(partner.phone) || partner.phone.Length < 3 || partner.phone.Length > 15)
                errors.Add("Номер телефона должен содержать от 3 до 15 символов");

            if (string.IsNullOrEmpty(partner.legal_address))
                errors.Add("Укажите юридический адрес партнера");

            if (partner.inn <= 0)
                errors.Add("Укажите ИНН партнера");

            if (partner.rating <= 0 || partner.rating > 10)
                errors.Add("Рейтинг может быть от 1 до 10");

            return (errors.Count == 0, errors);
        }
    }
}
