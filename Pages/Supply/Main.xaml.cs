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

namespace VinylRecordsApplication.Pages.Supply
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        IEnumerable<Classes.Supply> AllSupplies = Classes.Supply.AllSupplies();
        public Main()
        {
            InitializeComponent();
            foreach (var supply in AllSupplies)
            {
                supplyParent.Children.Add(new Pages.Supply.Elements.Supply(supply, this));
            }
                
        }
    }
}
