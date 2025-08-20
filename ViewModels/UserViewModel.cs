using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MovieSoftware.MVVM.Model.Classes;
using MovieSoftware.MVVM.Model.Repositories;
using MovieSoftware.Parent_classes;

namespace MovieSoftware.MVVM.ViewModel
{
    public class UserViewModel : ViewModelBasic
    {
        //Arbejder vi med CSV, så skal det ændres fx clients.csv
        private readonly IUserRepository userRepository = new FileUserRepository("clients.txt");

        public ObservableCollection<User> Users { get; }
        public ICollectionView UsersCollectionView { get; }

        private string userName;
        public string UserName { get => userName; set { userName = value; OnPropertyChanged(); } }

        private string passWord;
        public string PassWord
        {
            get => passWord; set { passWord = value; OnPropertyChanged(); }
        }

        private User selectedUser;
        public User SelectedUser
        {
            get => selectedUser; set { selectedUser = value; OnPropertyChanged(); }
        }

        public ICommand SaveUserCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand ValidateUserLoginCommand { get; }

        public UserViewModel()
        {
            Users = new ObservableCollection<User>(userRepository.GetAll());
            UsersCollectionView = CollectionViewSource.GetDefaultView(Users);
            UsersCollectionView.Filter = UsersFilter;

            
            AddUserCommand = new RelayCommand(_ => AddUser, _ => CanAddUser());
            SaveUserCommand = new RelayCommand(_ => SaveUser(), _ => CanSaveUser());
            ValidateUserLoginCommand = new RelayCommand(_ => ValidateUserLogin(), _ => CanLoginUser());
            DeleteUserCommand = new RelayCommand(_ => DeleteUser(), _ => CanDeleteUser());
        }

        private void AddUser()
        {
            var user = new User { UserName = userName, Password = passWord };
            if(Users.Where(u =>u.UserName == user.UserName).Any())
            {
                MessageBox.Show($"Kunne ikke tilføje. Brugeren eksisterer allerede");
            }
            else
            {
                Users.Add(user);
                userRepository.AddUser(user);
                MessageBox.Show($"Bruger: {user.UserName} tilføjet"); //, MessageBoxButton.OK, MessageBoxImage.Information);)

                UserName = string.Empty;
                PassWord = string.Empty;
            }
        }

        private void SaveUser()
        {
            userRepository.UpdateUser(SelectedUser);
            MessageBox.Show($"Ændringer gemt"); //, MessageBoxButton.OK, MessageBoxImage.Information);)
            SelectedUser = null;
        }

        private void DeleteUser()
        {
            userRepository.DeleteUser(SelectedUser);
            Users.Remove(SelectedUser);
            MessageBox.Show($"Bruger slettet"); //, MessageBoxButton.OK, MessageBoxImage.Information);)
            SelectedUser = null;
        }

        public void ValidateUserLogin()
        {
            bool loginValue = userRepository.ValidateUserLogin(UserName, PassWord);
            MainWindowViewModel.CurrentUser = userRepository.GetUser(userName);
            //skal linke til MainWindowViewModel, current View med RelayCommand

            if (loginValue)
            {
                MessageBox.Show($"Login valideret. Tryk for at fortsætte"); //, MessageBoxButton.OK, MessageBoxImage.Information);)
            }
            else
            {
                MessageBox.Show($"Username eller adgangskode er forkert. Prøv igen"); //, MessageBoxButton.OK, MessageBoxImage.Information);)

            }
        }

        private bool CanAddUser()=> !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(PassWord);
        private bool CanSaveUser() => SelectedUser != null;
        private bool CanDeleteUser() => SelectedUser != null;
        private bool CanLoginUser() => !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(PassWord);
        



       


    }
        
}
