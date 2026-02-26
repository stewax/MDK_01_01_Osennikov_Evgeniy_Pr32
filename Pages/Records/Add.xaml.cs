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

namespace VinylRecordsApplication.Pages.Records
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Page
    {
        public IEnumerable<Classes.Manufacturer> Manufacturers = Classes.Manufacturer.AllManufacturers();
        public IEnumerable<Classes.State> AllState = Classes.State.AllState();
        private Classes.Record changeRecord;
        public Add(Classes.Record changeRecord = null)
        {
            InitializeComponent();
            foreach (var item in Manufacturers)
            {
                tbManufacturer.Items.Add(item.Name);
            }
            if (Manufacturers.Count() > 0)
            {
                tbManufacturer.SelectedIndex = 0;
            }
            foreach (var item in AllState)
            {
                tbState.Items.Add(item.Name);
            }
            if (AllState.Count() > 0)
            {
                tbState.SelectedIndex = 0;
            }
            if (changeRecord != null)
            {
                this.changeRecord = changeRecord;

                tbName.Text = changeRecord.Name;
                tbYear.Text = changeRecord.Year.ToString();
                tbPrice.Text = changeRecord.Price.ToString().Replace(",", ".");
                tbDescription.Text = changeRecord.Description;

                tbFormat.SelectedIndex = changeRecord.Format;
                tbManufacturer.SelectedIndex = Manufacturers.ToList()
                    .FindIndex(x => x.Id == changeRecord.IdManufacturer);

                tbSize.SelectedIndex = changeRecord.Size;
                tbState.SelectedIndex = AllState.ToList()
                    .FindIndex(x => x.Id == changeRecord.IdState);
                addBth.Content = "Изменить";
            }
        }
        private void AddRecord(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbName.Text))
            {
                if (!string.IsNullOrEmpty(tbYear.Text))
                {
                    if (!string.IsNullOrEmpty(tbPrice.Text))
                    {
                        if (tbName.Text.Length <= 250)
                        {
                            if (changeRecord == null)
                            {
                                Classes.Record newRecord = new Classes.Record()
                                {
                                    Name = tbName.Text,
                                    Year = Convert.ToInt32(tbYear.Text),
                                    Format = tbFormat.SelectedIndex,
                                    Size = tbSize.SelectedIndex,
                                    IdManufacturer = Manufacturers
                                        .Where(x => x.Name == tbManufacturer.SelectedValue.ToString())
                                        .First().Id,
                                    Price = float.Parse(tbPrice.Text.Replace(".", ",")),
                                    IdState = AllState
                                        .Where(x => x.Name == tbState.SelectedItem.ToString())
                                        .First().Id,
                                    Description = tbDescription.Text
                                };

                                newRecord.Save();

                                MessageBox.Show($"Пластинка '{newRecord.Name}' успешно добавлена.", "Уведомление");
                                MainWindow.mainwindow.OpenPage(new Pages.Records.Add(newRecord));
                            }
                            else
                            {
                                changeRecord.Name = tbName.Text;
                                changeRecord.Year = Convert.ToInt32(tbYear.Text);
                                changeRecord.Format = tbFormat.SelectedIndex;
                                changeRecord.Size = tbSize.SelectedIndex;
                                changeRecord.IdManufacturer = Manufacturers
                                    .Where(x => x.Name == tbManufacturer.SelectedValue.ToString())
                                    .First().Id;
                                changeRecord.Price = float.Parse(tbPrice.Text.Replace(".", ","));
                                changeRecord.IdState = AllState
                                    .Where(x => x.Name == tbState.SelectedItem.ToString())
                                    .First().Id;
                                changeRecord.Description = tbDescription.Text;

                                changeRecord.Save(true);

                                MessageBox.Show($"Пластинка '{changeRecord.Name}' успешно изменена.", "Уведомление");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Наименование пластинки слишком большое.", "Предупреждение");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, укажите стоимость пластинки.", "Предупреждение");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, укажите год выпуска пластинки.", "Предупреждение");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, укажите наименование пластинки.", "Предупреждение");
            }
        }
        private void tbPreviewNumber(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }
        private void tbPreviewFloat(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.Text, 0) || e.Text == ".");
        }
    }
}
