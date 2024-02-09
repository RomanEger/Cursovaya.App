using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using CursovayaApp.WPF.Views;
using System.Windows;

namespace CursovayaApp.WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private User _thisUser;

        public User ThisUser
        {
            get => _thisUser;
            set
            {
                _thisUser = value;
                OnPropertyChanged("User");
            }
        }


        private RelayCommand _loginCommand;

        public RelayCommand LoginCommand =>
            _loginCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        User q = DbClass.entities.Users.FirstOrDefault(x => x.Login == ThisUser.Login && x.Password == ThisUser.Password) ?? new User();
                        LoggedUser loggedUser = new()
                        {
                            CurrentUser = q
                        };
                        if (q.RoleId == 1)
                        {
                            MyFrame.Navigate(new AdminPage());
                        }
                        else if (q.RoleId == 2 || q.RoleId == 3)
                        {
                            MyFrame.Navigate(new BooksPage());
                        }
                        else
                        {
                            MessageBox.Show("Неправильный логин или пароль");
                        }

                        ThisUser.Password = null;
                    }
                    catch (Exception)
                    {
                        //string fileName = $@"C:\Users\error{DateTime.Now}.txt";
                        //FileStream fileStream = new FileStream(fileName, FileMode.Create);
                        //StreamWriter sw = new StreamWriter(fileStream);
                        //sw.Write(ex.Message);
                        //sw.Close();
                    }
                });




        public LoginViewModel()
        {
            ThisUser = new User();
        }
    }
}
