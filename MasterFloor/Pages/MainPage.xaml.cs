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

namespace MasterFloor.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            LoadPartners();
        }

        private void LoadPartners()
        {
            using (var context = new master_floorEntities())
            {
                var partners = context.partners.Include("partners_types").ToList();
                ListViewPartners.ItemsSource = partners;
            }
        }

        private void btnAddPartner_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPartner(null));
        }

        private void ListViewPartners_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListViewPartners.SelectedItem is partners selectedPartner)
            {
                if (selectedPartner != null)
                {
                    NavigationService.Navigate(new AddEditPartner(selectedPartner));
                }
                else
                {
                    MessageBox.Show($"Партнер не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            using (var context = new master_floorEntities())
            {
                if (Visibility == Visibility.Visible)
                {
                    context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    ListViewPartners.ItemsSource = null;
                    LoadPartners();
                }
            }
        }

        private void btnToCalculateMethod_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CalculateMethod());
        }
    }
}
