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

namespace FloorMaster.Pages
{
    /// <summary>
    /// Логика взаимодействия для ViewPart.xaml
    /// </summary>
    public partial class ViewPart : Page
    {
        public ViewPart()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var partners = Data.MasterFloorEntities.GetContext().PartnersImport.ToList();
            var partnerDiscounts = (from partner in partners
                                    join product in Data.MasterFloorEntities.GetContext().PartnerProductsImport
                                    on partner.Id equals product.IdPartnerName into productGroup
                                    from product in productGroup.DefaultIfEmpty()
                                    group product by partner into g
                                    select new
                                    {
                                        Partner = g.Key,
                                        Discount = CalculateDiscount(g.Sum(p => p?.CountOfProduction ?? 0))
                                    }).ToList();




            MasterListView.ItemsSource = partnerDiscounts;


        }


        private int CalculateDiscount(int totalCount)
        {
            if (totalCount == 0) return 0;
            if (totalCount < 10000) return 0;
            if (totalCount >= 10000 && totalCount < 50000) return 5;
            if (totalCount >= 50000 && totalCount < 300000) return 10;
            return 15;
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            var partnerData = editButton.DataContext;
            if (partnerData != null)
            {
                var partner = (partnerData as dynamic).Partner;
                Classes.Managers.MainFrame.Navigate(new Pages.AddEdit(partner));
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Managers.MainFrame.Navigate(new Pages.AddEdit(null));
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            Button historyButton = sender as Button;
            var partnerData = historyButton.DataContext;

            if (partnerData != null)
            {
                var partner = (partnerData as dynamic).Partner;
                Classes.Managers.MainFrame.Navigate(new History(partner));
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите партнера из списка.");
            }
        }


    }
}
