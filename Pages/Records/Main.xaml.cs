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
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public IEnumerable<Classes.State> AllState = Classes.State.AllState();
        public IEnumerable<Classes.Record> AllRecords = Classes.Record.AllRecords();
        public IEnumerable<Classes.Manufacturer> AllManufacturers = Classes.Manufacturer.AllManufacturers();
        private bool CreateUI = false;
        public List<Classes.Record> searchRecords;

        public Main()
        {
            InitializeComponent();
            searchRecords = AllRecords.ToList();
            CreateUI = true;
            LoadAllRecord(AllRecords.ToList());
            LoadAllManufacture();
            LoadAllState();
        }
        public void LoadRecord()
        {
            AllRecords = Classes.Record.AllRecords();
            LoadAllRecord(AllRecords.ToList());
        }
        public void LoadAllRecord(List<Classes.Record> allRecords)
        {
            recordsParent.Children.Clear();
            foreach (var record in allRecords)
            {
                recordsParent.Children.Add(new Pages.Records.Elements.Record(record, this));
            }
        }
        public void LoadAllManufacture()
        {
            tbManufacturer.Items.Clear();

            foreach (var manufacturer in AllManufacturers)
            {
                tbManufacturer.Items.Add(manufacturer.Name);
            }
            tbManufacturer.Items.Add("Выберите ...");
            tbManufacturer.SelectedIndex = tbManufacturer.Items.Count - 1;
        }
        public void LoadAllState()
        {
            tbState.Items.Clear();

            foreach (var state in AllState)
            {
                tbState.Items.Add(state.Name);
            }
            tbState.Items.Add("Выберите ...");
            tbState.SelectedIndex = tbState.Items.Count - 1;
        }
        public void RecordsFilter()
        {
            List<Classes.Record> filterRecords = new List<Classes.Record>();
            if (tbManufacturer.SelectedIndex != tbManufacturer.Items.Count - 1)
            {
                int manufacturerId = AllManufacturers
                    .Where(y => y.Name == tbManufacturer.SelectedItem.ToString())
                    .First().Id;
                filterRecords = AllRecords.Where(x => x.IdManufacturer == manufacturerId).ToList();
            }
            else
            {
                filterRecords = AllRecords.ToList();
            }
            if (tbState.SelectedIndex != tbState.Items.Count - 1)
            {
                int stateId = AllState
                    .Where(y => y.Name == tbState.SelectedItem.ToString())
                    .First().Id;
                filterRecords = filterRecords.Where(x => x.IdState == stateId).ToList();
            }
            if (!string.IsNullOrEmpty(tbName.Text))
            {
                string searchText = tbName.Text.ToLower();
                if (searchText.Contains("моно"))
                {
                    filterRecords = filterRecords.Where(x => x.Format == 0).ToList();
                }
                else if (searchText.Contains("стерео"))
                {
                    filterRecords = filterRecords.Where(x => x.Format == 1).ToList();
                }
                else
                {
                    filterRecords = filterRecords.Where(x =>
                        x.Name.ToLower().Contains(searchText) ||
                        x.Year.ToString().Contains(searchText) ||
                        x.Price.ToString().Contains(searchText) ||
                        x.Description.ToLower().Contains(searchText)
                    ).ToList();
                }
            }
            searchRecords.Clear();
            searchRecords = filterRecords;
            LoadAllRecord(filterRecords);
        }
        private void FilterRecords(object sender, SelectionChangedEventArgs e)
        {
            if (CreateUI)
            {
                RecordsFilter();
            }
        }
        private void SearchRecords(object sender, KeyEventArgs e)
        {
            RecordsFilter();
        }
    }
}
