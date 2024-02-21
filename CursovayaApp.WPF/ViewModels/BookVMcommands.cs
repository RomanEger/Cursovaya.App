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
                        var localBooksList = _repositoryBook.GetAll();
                        foreach (var item in Books)
                        {
                            var publishingId = _repositoryPublishing.Where(x => x.Name == item.Publishing).Select(x => x.Id).FirstOrDefault();
                            var authorId = _repositoryAuthor.Where(x => x.FullName == item.AuthorFullName).Select(x => x.Id).FirstOrDefault();
                            var book = new Book()
                            {
                                Id = item.Id,
                                Quantity = item.Quantity,
                                Title = item.Title,
                                PublishingHouseId = publishingId,
                                AuthorId = authorId
                            };
                            _repositoryBook.AddOrUpdate(book);

                        }

                        _repositoryBook.Save();
                        MessageBox.Show("Изменения успешно сохранены");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Не удалось сохранить изменения", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });

        public RelayCommand AddCommand =>
            new(obj =>
                {
                    _addOrUpdateBooksView = new(this);
                    SelectedBook = new BookView(0);
                    Books.Add(SelectedBook);
                    SelectedBook.GetReasons();
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
                        try
                        {
                            if (AuthorsForAdd == null)
                            {
                                var listAuthors = _repositoryAuthor.GetAll();
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

                            var aId = _repositoryAuthor.Where(x => x.FullName == SelectedBook.AuthorFullName).Select(x => x.Id).FirstOrDefault();
                            var pId = _repositoryPublishing.Where(x => x.Name == SelectedBook.Publishing).Select(x => x.Id).FirstOrDefault();

                            var book = _repositoryBook.Get(x => x.Id == SelectedBook.Id);
                            book.Id = SelectedBook.Id;
                            book.Quantity = SelectedBook.Quantity;
                            book.Title = SelectedBook.Title;
                            book.AuthorId = aId;
                            book.PublishingHouseId = pId;

                            _repositoryBook.AddOrUpdate(book);
                            _repositoryBook.Save();
                            if (SelectedBook.ForAdd)
                            {
                                var rId = _repositoryReasonsReg.Where(x => x.Name == SelectedReason).Select(x => x.Id)
                                    .FirstOrDefault();
                                var bId = _repositoryBook.Where(x => x.AuthorId == book.AuthorId && x.Title == book.Title && x.PublishingHouseId == x.PublishingHouseId).Select(x => x.Id).FirstOrDefault();
                                var regBook = new RegBook()
                                {
                                    BookId = bId,
                                    ReasonId = rId,
                                    DateOfReg = DateTime.Now,
                                    UserId = _loggedUser.CurrentUser.Id,
                                    RegQuantity = SelectedBook.QuantityToUpdate
                                };
                                _repositoryRegBook.Add(regBook);
                            }
                            else
                            {
                                var dId = _repositoryReasonsDereg.Where(x => x.Name == SelectedReason).Select(x => x.Id)
                                    .FirstOrDefault();
                                var bId = _repositoryBook.Where(x => x.AuthorId == book.AuthorId && x.Title == book.Title && x.PublishingHouseId == x.PublishingHouseId).Select(x => x.Id).FirstOrDefault();
                                var deregBook = new DeregBook()
                                {
                                    BookId = bId,
                                    ReasonId = dId,
                                    DateOfDereg = DateTime.Now,
                                    UserId = _loggedUser.CurrentUser.Id,
                                    DeregQuantity = SelectedBook.QuantityToUpdate
                                };
                                _repositoryDeregBook.Add(deregBook);

                            }
                            _repositoryBook.Save();
                            _addOrUpdateBooksView.Close();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
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
                    var author = _repositoryAuthor.Get(x => x.FullName == SelectedAuthor) ?? new Author();
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
                    try
                    {
                        var publishing = _repositoryPublishing.Get(x => x.Name == SelectedPublishing) ?? new PublishingHouse();
                        _addOrUpdatePublishingsView = new(publishing, this);
                        _addOrUpdatePublishingsView.ShowDialog();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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

        public RelayCommand GiveCommand =>
            new(obj =>
            {
                _windowForGiveView = new(true);
                _windowForGiveView.ShowDialog();
            });

        public RelayCommand RecieveCommand
            => new(obj =>
            {
                _windowForGiveView = new(false);
                _windowForGiveView.ShowDialog();
            });


    }
}
