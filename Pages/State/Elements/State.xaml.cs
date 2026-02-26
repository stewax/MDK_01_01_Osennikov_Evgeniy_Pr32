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

namespace VinylRecordsApplication.Pages.State.Elements
{
    /// <summary>
    /// Логика взаимодействия для State.xaml
    /// </summary>
    public partial class State : UserControl
    {
        private Classes.State state;
        private Pages.State.Main main;

        public State(Classes.State state, Pages.State.Main main)
        {
            InitializeComponent();

            this.state = state;
            this.main = main;
            tbName.Text = this.state.Name;
            tbSubname.Text = this.state.Subname;
            tbDescription.Text = this.state.Description;
        }

        private void EditState(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Pages.State.Add(state));
        }

        private void DeleteState(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Удалить состояние: {this.state.Name}?",
                                "Уведомление",
                                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IEnumerable<Classes.Record> AllRecord = Classes.Record.AllRecords();
                if (AllRecord.Where(x => x.IdState == state.Id).Count() > 0)
                {
                    MessageBox.Show($"Состояние {this.state.Name} невозможно удалить. " +
                                   "Для начала удалите зависимости.",
                                   "Уведомление");
                }
                else
                {
                    this.state.Delete();
                    main.stateParent.Children.Remove(this);
                    MessageBox.Show($"Состояние {this.state.Name} успешно удалено.",
                                   "Уведомление");
                }
            }
        }
    }
}
