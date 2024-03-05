using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.ViewModels;
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
using System.Windows.Shapes;

namespace CursovayaApp.WPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddOrUpdateUsers.xaml
    /// </summary>
    public partial class AddOrUpdateUsers : Window
    {
        private readonly AdminViewModel _vm;
        private static User _user = new();
        public AddOrUpdateUsers(AdminViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
            if (_vm.SelectedUser.Id != 0)
                Title = "Редактирование пользователя";
            else
                Title = "Добавление пользователя";
            _user.FullName = _vm.SelectedUser.FullName;
            _user.Login = _vm.SelectedUser.Login;
            _user.Password = _vm.SelectedUser.Password;
            _user.RoleId = _vm.SelectedUser.RoleId;

        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            _vm.SelectedUser.FullName = _user.FullName;
            _vm.SelectedUser.Login = _user.Login;
            _vm.SelectedUser.Password = _user.Password;
            _vm.SelectedUser.RoleId = _user.RoleId;
            _vm.UserDto.Role = _vm.UserDto.AllowedRoles.FirstOrDefault(x => x.Key == _user.RoleId).Value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _vm.SaveCommand.Execute(null);
            this.Close();
        }
    }
}
