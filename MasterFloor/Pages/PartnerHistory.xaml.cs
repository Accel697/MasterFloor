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
    /// Логика взаимодействия для PartnerHistory.xaml
    /// </summary>
    public partial class PartnerHistory : Page
    {
        public PartnerHistory(long id)
        {
            InitializeComponent();

            using (var context = new master_floorEntities())
            {
                var history = context.orders.Include("products").Where(h => h.partner == id).ToList();
                ListViewHistory.ItemsSource = history;
            }
        }
    }
}
