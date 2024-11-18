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
    /// Логика взаимодействия для History.xaml
    /// </summary>
    public partial class History : Page
    {
        private readonly Data.PartnersImport _selectedPartner;

        public History(Data.PartnersImport selectedPartner)
        {
            InitializeComponent();
            _selectedPartner = selectedPartner;

            LoadHistory();
        }

        private void LoadHistory()
        {
            var history = Data.MasterFloorEntities.GetContext().PartnerProductsImport
                .Where(p => p.IdPartnerName == _selectedPartner.Id)
                .Select(p => new
                {
                    Production = p.Production,
                    CountOfProduction = p.CountOfProduction,
                    DateOfSale = p.DateOfSale
                })
                .ToList();

            HistoryDataGrid.ItemsSource = history;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Managers.MainFrame.Navigate(new Pages.ViewPart());
        }
    }

}
