using FloorMaster.Data;
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
    /// Логика взаимодействия для AddEdit.xaml
    /// </summary>
    public partial class AddEdit : Page
    {
        public string FlagAddorEdit = "default";
        public Data.PartnersImport _currentpartner = new Data.PartnersImport();

        public AddEdit(PartnersImport partner)
        {
            InitializeComponent();

            if (partner != null)
            {
                _currentpartner = partner;
                FlagAddorEdit = "edit";
            }
            else
            {
                FlagAddorEdit = "add";
            }
            DataContext = _currentpartner;


            Init();
        }

        public void Init()
        {
            try
            {
                TypeComboBox.ItemsSource = Data.MasterFloorEntities.GetContext().TypeOfPartner.ToList();
                if (FlagAddorEdit == "add")
                {
                    IdTextBox.Visibility = Visibility.Hidden;
                    IdLabel.Visibility = Visibility.Hidden;

                    NameTextBox.Text = string.Empty;
                    RatingTextBox.Text = string.Empty;
                    RegionTextBox.Text = string.Empty;
                    CityTextBox.Text = string.Empty;
                    StreetTextBox.Text = string.Empty;
                    HouseNumTextBox.Text = string.Empty;
                    IndexTextBox.Text = string.Empty;
                    FIOTextBox.Text = string.Empty;
                    PhoneTextBox.Text = string.Empty;
                    EmailTextBox.Text = string.Empty;

                    IdTextBox.Text = Data.MasterFloorEntities.GetContext().PartnersImport.Max(d => d.Id + 1).ToString();
                }

                else if (FlagAddorEdit == "edit")
                {
                    IdTextBox.Visibility = Visibility.Hidden;
                    IdLabel.Visibility = Visibility.Hidden;

                    NameTextBox.Text = _currentpartner.PartnerName.Name;
                    RatingTextBox.Text = _currentpartner.Reiting.ToString();
                    RegionTextBox.Text = _currentpartner.Adress.Regions.RegionOf;
                    CityTextBox.Text = _currentpartner.Adress.Cities.CityOf.ToString();
                    StreetTextBox.Text = _currentpartner.Adress.Streets.StreetOf.ToString();
                    HouseNumTextBox.Text = _currentpartner.Adress.HouseNum.ToString();
                    IndexTextBox.Text = _currentpartner.Adress.Indexes.IndexOf.ToString();
                    FIOTextBox.Text = _currentpartner.Directors.FIO;
                    PhoneTextBox.Text = _currentpartner.PhoneOfPartner;
                    EmailTextBox.Text = _currentpartner.EmailOfPartner;

                    TypeComboBox.SelectedItem = Data.MasterFloorEntities.GetContext().TypeOfPartner.Where(d => d.Id == _currentpartner.IdTypeOfParther).FirstOrDefault();


                }
            }
            catch (Exception)
            {

            }
        }



        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Classes.Managers.MainFrame.Navigate(new Pages.ViewPart());
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder errors = new StringBuilder();
                if (string.IsNullOrEmpty(NameTextBox.Text))
                {
                    errors.AppendLine("Заполните наименование!");
                }
                if (TypeComboBox.SelectedItem == null)
                {
                    errors.AppendLine("Выберите тип партнера!");
                }
                if (string.IsNullOrEmpty(RatingTextBox.Text) || !int.TryParse(RatingTextBox.Text, out int rating) || rating < 0)
                {
                    errors.AppendLine("Рейтинг должен быть целым неотрицательным числом!");
                }
                if (string.IsNullOrEmpty(RatingTextBox.Text))
                {
                    errors.AppendLine("Заполните рейтинг!");
                }
                if (string.IsNullOrEmpty(RegionTextBox.Text))
                {
                    errors.AppendLine("Заполните Регион!");
                }
                if (string.IsNullOrEmpty(CityTextBox.Text))
                {
                    errors.AppendLine("Заполните Город!");
                }
                if (string.IsNullOrEmpty(StreetTextBox.Text))
                {
                    errors.AppendLine("Заполните улицу!");
                }
                if (string.IsNullOrEmpty(HouseNumTextBox.Text))
                {
                    errors.AppendLine("Заполните номер дома!");
                }
                if (!int.TryParse(HouseNumTextBox.Text, out int House) || House < 0)
                {
                    errors.AppendLine("Заполните номер дома без букв и символов, только цифры!");
                }
                if (string.IsNullOrEmpty(IndexTextBox.Text))
                {
                    errors.AppendLine("Заполните индекс!");
                }
                if (!int.TryParse(IndexTextBox.Text, out int Ind) || Ind < 0)
                {
                    errors.AppendLine("Заполните индекс согласно стандарту, т.е. цифрами!");
                }
                if (string.IsNullOrEmpty(FIOTextBox.Text))
                {
                    errors.AppendLine("Заполните ФИО!");
                }
                if (string.IsNullOrEmpty(PhoneTextBox.Text))
                {
                    errors.AppendLine("Заполните номер телефона!");
                }
                if (string.IsNullOrEmpty(EmailTextBox.Text))
                {
                    errors.AppendLine("Заполните Email!");
                }

                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var selectedCategory = TypeComboBox.SelectedItem as Data.TypeOfPartner;
                if (selectedCategory == null)
                {
                    MessageBox.Show("Выберите тип партнера!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _currentpartner.IdTypeOfParther = selectedCategory.Id;
                _currentpartner.Reiting = Convert.ToInt32(RatingTextBox.Text);
                _currentpartner.PhoneOfPartner = PhoneTextBox.Text;
                _currentpartner.EmailOfPartner = EmailTextBox.Text;


                var searchDirector = (from item in Data.MasterFloorEntities.GetContext().Directors
                                      where item.FIO == FIOTextBox.Text
                                      select item).FirstOrDefault();
                if (searchDirector != null)
                {
                    _currentpartner.IdDirector = searchDirector.Id;
                }
                else
                {
                    Data.Directors directors = new Data.Directors()
                    {
                        FIO = FIOTextBox.Text
                    };
                    Data.MasterFloorEntities.GetContext().Directors.Add(directors);
                    Data.MasterFloorEntities.GetContext().SaveChanges();
                    _currentpartner.IdDirector = directors.Id;
                }

                var searchPartnerName = Data.MasterFloorEntities.GetContext().PartnerName
                           .FirstOrDefault(pn => pn.Name == NameTextBox.Text);
                if (searchPartnerName != null)
                {
                    _currentpartner.IdPartnerName = searchPartnerName.Id;
                }
                else
                {
                    var partnerName = new Data.PartnerName { Name = NameTextBox.Text };
                    Data.MasterFloorEntities.GetContext().PartnerName.Add(partnerName);
                    Data.MasterFloorEntities.GetContext().SaveChanges();
                    _currentpartner.IdPartnerName = partnerName.Id;
                }





                int houseNum = int.Parse(HouseNumTextBox.Text);
                int indexOf = int.Parse(IndexTextBox.Text);

                var address = MasterFloorEntities.GetContext().Adress
                    .FirstOrDefault(a => a.Regions.RegionOf == RegionTextBox.Text &&
                                         a.Cities.CityOf == CityTextBox.Text &&
                                         a.Streets.StreetOf == StreetTextBox.Text &&
                                         a.HouseNum == houseNum &&
                                         a.Indexes.IndexOf == indexOf);
                if (address == null)
                {
                    address = new Adress
                    {
                        Regions = new Regions { RegionOf = RegionTextBox.Text },
                        Cities = new Cities { CityOf = CityTextBox.Text },
                        Streets = new Streets { StreetOf = StreetTextBox.Text },
                        HouseNum = houseNum,
                        Indexes = new Indexes { IndexOf = indexOf }
                    };

                    MasterFloorEntities.GetContext().Adress.Add(address);
                    MasterFloorEntities.GetContext().SaveChanges();
                }


                _currentpartner.IdAdress = address.Id;



                if (address.Id != 0)
                {
                    _currentpartner.IdAdress = address.Id;
                }
                else
                {
                    MessageBox.Show("Ошибка сохранения адреса.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (FlagAddorEdit == "add")
                {

                    Data.MasterFloorEntities.GetContext().PartnersImport.Add(_currentpartner);
                    Data.MasterFloorEntities.GetContext().SaveChanges();




                    MessageBox.Show("Успешно добавлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else if (FlagAddorEdit == "edit")
                {
                    Data.MasterFloorEntities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно сохранено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                Classes.Managers.MainFrame.Navigate(new Pages.ViewPart());




            }



            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                Exception innerException = ex.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine(innerException.Message);
                    innerException = innerException.InnerException;
                }
            }
        }


    }
}

