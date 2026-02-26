using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace VinylRecordsApplication.Pages.Manufacturer
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Page
    {
        public IEnumerable<Classes.Country> AllCountries = Classes.Country.AllCountries();
        Classes.Manufacturer changeManufacturer;

        public Add(Classes.Manufacturer changeManufacturer = null)
        {
            InitializeComponent();
            foreach (var Country in AllCountries)
                tbCountry.Items.Add(Country.Name);
            if (AllCountries.Count() > 0)
                // Выбираем первую страну
                tbCountry.SelectedIndex = 0;
            if (changeManufacturer != null)
            {
                this.changeManufacturer = changeManufacturer;
                tbName.Text = changeManufacturer.Name;
                tbPhone.Text = changeManufacturer.Phone;
                tbEmail.Text = changeManufacturer.Mail;
                tbCountry.SelectedIndex = AllCountries.ToList().FindIndex(x => x.Id == changeManufacturer.CountryCode);
                addBtn.Content = "Изменить";
            }
        }

        private void AddManufacturer(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbName.Text))
                if (!String.IsNullOrEmpty(tbPhone.Text))
                    if (!String.IsNullOrEmpty(tbEmail.Text))
                        if (CorrectPhone(tbPhone.Text))
                            if (CorrectEmail(tbEmail.Text))
                            {
                                if (changeManufacturer == null)
                                {
                                    Classes.Manufacturer manufacturer = new Classes.Manufacturer()
                                    {
                                        Name = tbName.Text,
                                        Phone = tbPhone.Text,
                                        Mail = tbEmail.Text,
                                        CountryCode = AllCountries.Where(x => x.Name == tbCountry.SelectedItem.ToString()).First().Id
                                    };
                                    manufacturer.Save();
                                    MessageBox.Show($"Поставщик {manufacturer.Name} успешно добавлен.", "Уведомление");
                                    MainWindow.mainWindow.OpenPage(new Add(manufacturer));
                                }
                                else
                                {
                                    changeManufacturer.Name = tbName.Text;
                                    changeManufacturer.Phone = tbPhone.Text;
                                    changeManufacturer.Mail = tbEmail.Text;
                                    changeManufacturer.CountryCode = AllCountries.Where(x => x.Name == tbCountry.SelectedItem.ToString()).First().Id;
                                    changeManufacturer.Save(true);
                                    MessageBox.Show($"Поставщик {changeManufacturer.Name} успешно изменён.", "Уведомление");
                                }
                            }
                            else
                                MessageBox.Show("Пожалуйста, укажите почту поставщика в формате xx@xx.xx.", "Предупреждение");
                        else
                            MessageBox.Show("Пожалуйста, укажите номер поставщика в формате 89000000000.", "Предупреждение");
                    else
                        MessageBox.Show("Пожалуйста, укажите почту поставщика.", "Предупреждение");
                else
                    MessageBox.Show("Пожалуйста, укажите телефон поставщика.", "Предупреждение");
            else
                MessageBox.Show("Пожалуйста, укажите наименование поставщика.", "Предупреждение");
        }

        public bool CorrectPhone(string Value)
        {
            string $Regex = "^8[0-9]{9}$";
            Regex regex = new Regex($Regex);
            MatchCollection matches = regex.Matches(Value);
            return matches.Count > 0;
        }

        public bool CorrectEmail(string Value)
        {
            string $Regex = "[aA-zZ]{2,20}@[aA-zZ]{2,20}.[aA-zZ]{2,3}";
            Regex regex = new Regex($Regex);
            // Получаем совпадения
            MatchCollection matches = regex.Matches(Value);
            return matches.Count > 0;
        }

        private void tbPreviewNumber(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}
