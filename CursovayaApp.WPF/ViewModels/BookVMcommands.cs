using CursovayaApp.WPF.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using CursovayaApp.WPF.Views.Windows;

namespace CursovayaApp.WPF.ViewModels
{
    public partial class BooksViewModel
    {
        public RelayCommand GoBackCommand =>
            new(obj =>
                {
                    if (MyFrame.frame.CanGoBack)
                        if (_loggedUser.CurrentUser.RoleId != 1)
                        {
                            if (MessageBox.Show(
                                    "Это действие приведет к выходу из аккаунта. Вы уверены, что хотите продолжить?",
                                    "Выход",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                                MyFrame.frame.GoBack();
                        }
                        else
                            MyFrame.frame.GoBack();
                });

        public RelayCommand SaveCommand =>
            new(obj =>
                {
                    try
                    {
                        var localBooksList = DbClass.entities.Books.ToList();
                        foreach (var item in Books)
                        {
                            var publishingId = DbClass.entities.PublishingHouses.Where(x => x.Name == item.Publishing).Select(x => x.Id).FirstOrDefault();
                            var authorId = DbClass.entities.Authors.Where(x => x.FullName == item.AuthorFullName).Select(x => x.Id).FirstOrDefault();
                            var book = new Book()
                            {
                                Id = item.Id,
                                Quantity = item.Quantity,
                                Title = item.Title,
                                PublishingHouseId = publishingId,
                                AuthorId = authorId
                            };
                            DbClass.entities.Books.AddOrUpdate(book);

                        }

                        DbClass.entities.SaveChanges();
                        MessageBox.Show("Изменения успешно сохранены");
                    }
                    catch (Exception ex)
                    {
                        //string fileName = $@"C:\Users\error{DateTime.Now}.txt";
                        //FileStream fileStream = new FileStream(fileName, FileMode.Create);
                        //StreamWriter sw = new StreamWriter(fileStream);
                        //sw.Write(ex.Message);
                        //sw.Close();
                    }
                });

        public RelayCommand AddCommand =>
            new(obj =>
                {
                    _addOrUpdateBooksView = new(this);
                    SelectedBook = new BookView(0);
                    Books.Add(SelectedBook);
                    _addOrUpdateBooksView.ShowDialog();
                });

        public RelayCommand UpdateCommand =>
            new(obj =>
                {
                    _addOrUpdateBooksView = new(this);
                    _addOrUpdateBooksView.ShowDialog();
                });

        public RelayCommand FirstBooksCommand =>
            new(obj => Pagination.FirstT(ref _books, _sortedListBooks));

        public RelayCommand BackBooksCommand =>
            new(obj => Pagination.BackT(ref _books, _sortedListBooks));

        public RelayCommand ForwardBooksCommand =>
            new(obj => Pagination.ForwardT(ref _books, _sortedListBooks));

        public RelayCommand LastBooksCommand =>
            new(obj => Pagination.LastT(ref _books, _sortedListBooks));

        public RelayCommand AddOrUpdateBookCommand =>
            new(obj =>
                {
                    if (Books.Any(x => x.Id == SelectedBook.Id))
                    {
                        if (AuthorsForAdd == null)
                        {
                            var listAuthors = DbClass.entities.Authors.ToList();
                            AuthorsForAdd = new ObservableCollection<string>(listAuthors.Select(x => x.FullName).Distinct());
                        }
                        if (!AuthorsForAdd.Contains(SelectedBook.AuthorFullName))
                        {
                            if (MessageBox.Show(
                                    "Данный автор еще не представлен в нашей библиотеке. Хотите его добавить?",
                                    "",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                var author = new Author()
                                {
                                    FullName = SelectedBook.AuthorFullName
                                };
                                _addOrUpdateAuthorsView = new(author, this);
                                _addOrUpdateAuthorsView.ShowDialog();
                            }
                            else return;
                        }

                        var aId = DbClass.entities.Authors.Where(x => x.FullName == SelectedBook.AuthorFullName).Select(x => x.Id).FirstOrDefault();
                        var pId = DbClass.entities.PublishingHouses.Where(x => x.Name == SelectedBook.Publishing).Select(x => x.Id).FirstOrDefault();

                        var book = DbClass.entities.Books.FirstOrDefault(x => x.Id == SelectedBook.Id) ?? new Book();
                        book.Id = SelectedBook.Id;
                        book.Quantity = SelectedBook.Quantity;
                        book.Title = SelectedBook.Title;
                        book.AuthorId = aId;
                        book.PublishingHouseId = pId;

                        DbClass.entities.Books.AddOrUpdate(book);
                        DbClass.entities.SaveChanges();
                        if (SelectedBook.ForAdd)
                        {
                            var rId = DbClass.entities.ReasonsReg.Where(x => x.Name == SelectedReason).Select(x => x.Id)
                                .FirstOrDefault();
                            var bId = DbClass.entities.Books.Where(x => x.AuthorId == book.AuthorId && x.Title == book.Title && x.PublishingHouseId == x.PublishingHouseId).Select(x => x.Id).FirstOrDefault();
                            var regBook = new RegBook()
                            {
                                BookId = bId,
                                ReasonId = rId,
                                DateOfReg = DateTime.Now,
                                UserId = _loggedUser.CurrentUser.Id,
                                RegQuantity = SelectedBook.QuantityToUpdate
                            };
                            DbClass.entities.RegBooks.Add(regBook);
                        }
                        else
                        {
                            var dId = DbClass.entities.ReasonsReg.Where(x => x.Name == SelectedReason).Select(x => x.Id)
                                .FirstOrDefault();
                            var bId = DbClass.entities.Books.Where(x => x.AuthorId == book.AuthorId && x.Title == book.Title && x.PublishingHouseId == x.PublishingHouseId).Select(x => x.Id).FirstOrDefault();
                            var deregBook = new DeregBook()
                            {
                                BookId = bId,
                                ReasonId = dId,
                                DateOfDereg = DateTime.Now,
                                UserId = _loggedUser.CurrentUser.Id,
                                DeregQuantity = SelectedBook.QuantityToUpdate
                            };
                            DbClass.entities.DeregBooks.Add(deregBook);

                        }
                        DbClass.entities.SaveChanges();
                        _addOrUpdateBooksView.Close();
                    }
                });

        public RelayCommand AddOrUpdateAuthorCommand =>
            new(obj =>
                {
                    var dialogResult = MessageBox.Show("Если Вы хотите добавить нового автора -> Да\n" +
                        "Если Вы хотите изменить существующего автора? -> Нет", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        var author = new Author();
                        _addOrUpdateAuthorsView = new(author, this);
                        _addOrUpdateAuthorsView.ShowDialog();
                    }
                    else if (dialogResult == MessageBoxResult.No)
                    {
                        var window = new ChooseForAdd(this, true);
                        window.ShowDialog();
                    }
                });

        public RelayCommand AddAuthorCommand =>
            new(obj =>
                {
                    var author = DbClass.entities.Authors.FirstOrDefault(x => x.FullName == SelectedAuthor) ?? new Author();
                    _addOrUpdateAuthorsView = new(author, this);
                    _addOrUpdateAuthorsView.ShowDialog();
                });

        public RelayCommand AddOrUpdatePublishingCommand =>
            new(obj =>
                {
                    var dialogResult = MessageBox.Show("Если Вы хотите добавить новое издательство -> Да\n" +
                        "Если Вы хотите изменить существующее издательство? -> Нет", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        var publishing = new PublishingHouse();
                        _addOrUpdatePublishingsView = new(publishing, this);
                        _addOrUpdatePublishingsView.ShowDialog();
                    }
                    else if (dialogResult == MessageBoxResult.No)
                    {
                        var window = new ChooseForAdd(this, false);
                        window.ShowDialog();
                    }
                });

        public RelayCommand AddPublishingCommand =>
            new(obj =>
                {
                    var publishing = DbClass.entities.PublishingHouses.FirstOrDefault(x => x.Name == SelectedPublishing) ?? new PublishingHouse();
                    _addOrUpdatePublishingsView = new(publishing, this);
                    _addOrUpdatePublishingsView.ShowDialog();
                });

        public RelayCommand CancelCommand =>
            new(obj =>
                {
                    if (MessageBox.Show("Вы уверены, что хотите отменить все изменения?\nЭто действие удалит текущую запись!",
                        "Отмена изменений",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.No) return;

                    int i = Books.ToList().IndexOf(SelectedBook) - 1;
                    Books.Remove(SelectedBook);
                    SelectedBook = Books.ElementAt(i);
                    _addOrUpdateBooksView?.Close();
                });

        public RelayCommand GiveCommand
            => new(obj =>
            {
                _windowForGiveView = new();
                _windowForGiveView.ShowDialog();
            });

        public RelayCommand RecieveCommand
            => new(obj =>
            {
                //_windowForRecieve = new();
                //_windowForRecieve.ShowDialog();
            });
    }
}
