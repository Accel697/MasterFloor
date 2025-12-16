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
    /// Логика взаимодействия для CalculateMethod.xaml
    /// </summary>
    public partial class CalculateMethod : Page
    {
        public CalculateMethod()
        {
            InitializeComponent();

            using (var context = new master_floorEntities())
            {
                var products = context.products.ToList();
                cbProduct.ItemsSource = products;

                var materials = context.materials.Include("materials_types").ToList();
                cbMaterial.ItemsSource = materials;
            }
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            var product = cbProduct.SelectedItem as products;
            var material = cbMaterial.SelectedItem as materials;

            if (!int.TryParse(tbQuantity.Text, out int quantity))
            {
                tblResult.Text = "Результат: -1";
                return;
            }

            if (product == null || material == null)
            {
                tblResult.Text = "Результат: -1";
                return;
            }

            using (var context = new master_floorEntities())
            {
                var productMaterial = context.products_materials.FirstOrDefault(pm => pm.product == product.id && pm.material == material.id);

                if (productMaterial == null)
                {
                    tblResult.Text = "Результат: -1";
                    return;
                }

                tblResult.Text = $"Результат: {CalculatingMethod(decimal.Parse(productMaterial.quantity.ToString()), material.materials_types.defects_percentege, quantity)}";
            }
        }

        private int CalculatingMethod(decimal quantityMaterialInProduct, decimal defectsPercentage, int quantityProduct)
        {
            if (quantityMaterialInProduct <= 0 || defectsPercentage <= 0 || quantityProduct <= 0) return -1;

            decimal result = 0;

            result = quantityMaterialInProduct * decimal.Parse(quantityProduct.ToString());

            result += result * defectsPercentage;

            result = Math.Ceiling(result);

            return int.Parse(result.ToString());
        }
    }
}
