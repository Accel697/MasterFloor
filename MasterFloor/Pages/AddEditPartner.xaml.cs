using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MasterFloor.Model;
using MasterFloor.Service;

namespace MasterFloor.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditPartner.xaml
    /// </summary>
    public partial class AddEditPartner : Page
    {
        partners _partner = new partners();

        public AddEditPartner(partners partner)
        {
            InitializeComponent();

            using (var context = new master_floorEntities())
            {
                var partnerTypes = context.partners_types.ToList();
                cbType.ItemsSource = partnerTypes;
            }

            if (partner != null)
            {
                _partner = partner;
                btnDelete.Visibility = Visibility.Visible;
                btnHistory.Visibility = Visibility.Visible;
            }

            DataContext = _partner;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new master_floorEntities())
            {
                try
                {
                    if (MessageBox.Show($"Вы действительно хотите удалить партнера {_partner.title}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        if (context.orders.Any(o => o.partner == _partner.id))
                        {
                            MessageBox.Show($"У данного партнера есть данные в таблице заказов, его нельзя удалить", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        var partnerInDb = context.partners.FirstOrDefault(p => p.id == _partner.id);

                        if (partnerInDb == null)
                        {
                            MessageBox.Show("Партнер не найден в базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        context.partners.Remove(partnerInDb);
                        context.SaveChanges();
                        MessageBox.Show("Партнер успешно удален", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.GoBack();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new master_floorEntities())
            {
                DataValidator validator = new DataValidator();

                var (isValid, errors) = validator.PartnerValidator(_partner);

                if (!isValid)
                {
                    MessageBox.Show(string.Join("\n", errors), "Ошибки валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    if (_partner.id == 0)
                    {
                        context.partners.Add(_partner);
                        NavigationService.GoBack();
                    }
                    else
                    {
                        var partnerInDb = context.partners.FirstOrDefault(p => p.id == _partner.id);

                        if (partnerInDb == null)
                        {
                            MessageBox.Show("Партнер не найден в базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        partnerInDb.type = _partner.type;
                        partnerInDb.title = _partner.title;
                        partnerInDb.director = _partner.director;
                        partnerInDb.email = _partner.email;
                        partnerInDb.phone = _partner.phone;
                        partnerInDb.legal_address = _partner.legal_address;
                        partnerInDb.inn = _partner.inn;
                        partnerInDb.rating = _partner.rating;
                    }

                    context.SaveChanges();
                    MessageBox.Show("Данные сохранены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PartnerHistory(_partner.id));
        }
    }
}
