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

namespace VinylRecordsApplication.Pages.Records.Elements
{
    /// <summary>
    /// Логика взаимодействия для Record.xaml
    /// </summary>
    public partial class Record : UserControl
    {
        private IEnumerable<Classes.State> AllState = Classes.State.AllState();
        private Classes.Record record;
        private Pages.Records.Main main;
        public Record(Classes.Record record, Pages.Records.Main main)
        {
            InitializeComponent();
            IEnumerable<Classes.Manufacturer> allManufacturer = Classes.Manufacturer.AllManufacturers();

            this.record = record;
            this.main = main;

            tbName.Text = record.Name;
            tbYear.Text = record.Year.ToString();
            tbFormat.Text = record.Format == 0 ? "Моно" : "Стерео";
            switch (record.Size)
            {
                case 0:
                    tbSize.Text = "7 дюймов";
                    break;
                case 1:
                    tbSize.Text = "10 дюймов";
                    break;
                case 2:
                    tbSize.Text = "12 дюймов";
                    break;
                case 3:
                    tbSize.Text = "Иной";
                    break;
            }
            tbManufacturer.Text = allManufacturer
                .Where(x => x.Id == record.IdManufacturer)
                .First().Name;
            tbPrice.Text = record.Price.ToString();
            tbState.Text = AllState
                .Where(x => x.Id == record.IdState)
                .First().Name;
            tbDescription.Text = record.Description;
        }
        private void EditRecord(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Add(this.record));
        }
        private void DeleteRecord(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Удалить виниловую пластинку: {this.record.Name}?",
                                "Уведомление",
                                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IEnumerable<Classes.Supply> allSupply = Classes.Supply.AllSupplies();
                if (allSupply.Where(x => x.IdRecord == record.Id).Count() > 0)
                {
                    MessageBox.Show($"Виниловую пластинку '{this.record.Name}' невозможно удалить. Для начала удалите зависимости.",
                                    "Уведомление");
                }
                else
                {
                    this.record.Delete();
                    main.recordsParent.Children.Remove(this);
                    MessageBox.Show($"Пластинка '{this.record.Name}' успешно удалена.", "Уведомление");
                }
            }
        }
    }
}
