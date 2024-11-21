using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace Defectz
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadData();
        }


        private void LoadData()
        {
            try
            {
                var partnerMaterialData = (from partner in Data.MasterFloorEntities.GetContext().PartnersImport
                                           join partnerProduct in Data.MasterFloorEntities.GetContext().PartnerProductsImport
                                           on partner.Id equals partnerProduct.IdPartnerName
                                           join product in Data.MasterFloorEntities.GetContext().ProductsImport
                                           on partnerProduct.IdProduction equals product.Id
                                           join productTypeJoin in Data.MasterFloorEntities.GetContext().ProductTypeImport
                                           on product.IdProductType equals productTypeJoin.Id into productTypeGroup
                                           from productType in productTypeGroup.DefaultIfEmpty()
                                           join materialTypeJoin in Data.MasterFloorEntities.GetContext().MaterialTypeImport
                                           on product.IdProductType equals materialTypeJoin.Id into materialTypeGroup
                                           from materialType in materialTypeGroup.DefaultIfEmpty()
                                           join partnerName in Data.MasterFloorEntities.GetContext().PartnerName
                                           on partner.IdPartnerName equals partnerName.Id
                                           group new { partnerProduct, productType, materialType, partnerName } by partner into g
                                           select new
                                           {
                                               PartnerName = g.FirstOrDefault().partnerName.Name,
                                               MaterialRequired = (int)Math.Ceiling(
                                                   g.Sum(p => p.partnerProduct.CountOfProduction) *
                                                   g.Select(p => p.productType != null ? p.productType.CoefOfType : 1).FirstOrDefault() *
                                                   (1 - g.Select(p => p.materialType != null ? p.materialType.PercentOfDefect : 0).FirstOrDefault())
                                               )
                                           }).ToList();

                PartnerMaterialDataGrid.ItemsSource = partnerMaterialData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        public int CalculateRequiredMaterial(int totalProduction, double coeffOfType, double percentDefect)
        {
            double requiredMaterial = totalProduction * coeffOfType;
            requiredMaterial += requiredMaterial * (percentDefect / 100);
            return (int)Math.Ceiling(requiredMaterial);
        }



    }
}