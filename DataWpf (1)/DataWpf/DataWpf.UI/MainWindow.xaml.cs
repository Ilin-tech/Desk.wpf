using DataWpf.ViewModel;
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

namespace DataWpf.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel mainViewModel = new MainWindowViewModel(Mediator.Instance);
            this.DataContext = mainViewModel;
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            NewEditWindow newWindow = new NewEditWindow();
            newWindow.DataContext = new NewEditWindowViewModel(Mediator.Instance);
            newWindow.ShowDialog();
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel viewModel = (MainWindowViewModel)DataContext;
            NewEditWindow editWindow = new NewEditWindow();
            editWindow.DataContext = new NewEditWindowViewModel(viewModel.CurrentPerson.Clone(), Mediator.Instance);
            editWindow.ShowDialog();
        }
    }
}
