using CursovayaApp.WPF.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using CursovayaApp.WPF.Views.Windows;

namespace CursovayaApp.WPF.ViewModels
{
    public partial class BooksViewModel
    {
        public RelayCommand GoBackCommand =>
            new(_ =>
                {
                    if (MyFrame.Frame.CanGoBack)
                        if (_loggedUser.CurrentUser.RoleId != 1)
                        {
                            if (MessageBox.Show(
                                    "Это действие приведет к выходу из аккаунта. Вы уверены, что хотите продолжить?",
                                    "Выход",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                                MyFrame.Frame.GoBack();
                        }
                        else
                            MyFrame.Frame.GoBack();
                });

        public RelayCommand SaveCommand =>
            new(_ =>
                {
                    try
                    {
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
            new(_ =>
                {
                    _addOrUpdateBooksView = new(this);
                    SelectedBook = new BookView(0);
                    Books.Add(SelectedBook);
                    InitBookForUpdate(SelectedBook);
                    _addOrUpdateBooksView.ShowDialog();
                });

        void InitBookForUpdate(BookView bookView)
        {
            if (BookForUpdate == null)
                BookForUpdate = new BookView(bookView.OldQuantity);
            BookForUpdate.QuantityToUpdate = bookView.QuantityToUpdate;
            BookForUpdate.Quantity = bookView.Quantity;
            BookForUpdate.Publishing = bookView.Publishing;
            BookForUpdate.Id = bookView.Id;
            BookForUpdate.Title = bookView.Title;
            BookForUpdate.AuthorFullName = bookView.Title;
        }

        public RelayCommand UpdateCommand =>
            new(_ =>
                {
                    _addOrUpdateBooksView = new(this);
                    InitBookForUpdate(SelectedBook ?? new BookView(0));
                    _addOrUpdateBooksView.ShowDialog();
                });

        public RelayCommand FirstBooksCommand =>
            new(_ => Pagination.FirstT(ref _books, _sortedListBooks));

        public RelayCommand BackBooksCommand =>
            new(_ => Pagination.BackT(ref _books, _sortedListBooks));

        public RelayCommand ForwardBooksCommand =>
            new(_ => Pagination.ForwardT(ref _books, _sortedListBooks));

        public RelayCommand LastBooksCommand =>
            new(_ => Pagination.LastT(ref _books, _sortedListBooks));

        public RelayCommand AddOrUpdateBookCommand =>
            new(_ =>
                {
                    if (Books.Any(x => SelectedBook != null && x.Id == SelectedBook.Id))
                    {
                        if (SelectedBook.AuthorFullName == null ||
                            SelectedBook.Title == null ||
                            SelectedBook.Publishing == null ||
                            SelectedBook.Quantity < 0)
                        {
                            MessageBox.Show("Проверьте корректность данных");
                            return;
                        }
                        try
                        {
                            if (AuthorsForAdd == null)
                            {
                                var listAuthors = _repositoryAuthor.GetAll();
                                AuthorsForAdd = new ObservableCollection<string>(listAuthors.Select(x => x.FullName).Distinct());
                            }
                            if (SelectedBook != null && !AuthorsForAdd.Contains(SelectedBook.AuthorFullName))
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

                            var aId = _repositoryAuthor.Where(x => SelectedBook != null && x.FullName == SelectedBook.AuthorFullName).Select(x => x.Id).FirstOrDefault();
                            var pId = _repositoryPublishing.Where(x => SelectedBook != null && x.Name == SelectedBook.Publishing).Select(x => x.Id).FirstOrDefault();

                            var book = _repositoryBook.Get(x => SelectedBook != null && x.Id == SelectedBook.Id) ?? new Book();
                            book.Id = SelectedBook?.Id ?? 0;
                            book.Quantity = SelectedBook?.Quantity ?? 0;
                            book.Title = SelectedBook?.Title ?? "";
                            book.AuthorId = aId;
                            book.PublishingHouseId = pId;

                            _repositoryBook.AddOrUpdate(book);
                            _repositoryBook.Save();
                            var bId = _repositoryBook
                                    .Where(x => x.AuthorId == book.AuthorId && x.Title == book.Title && x.PublishingHouseId == book.PublishingHouseId)
                                    .Select(x => x.Id)
                                    .FirstOrDefault();
                            if (SelectedBook is { ForAdd: true })
                            {
                                var rId = _repositoryReasonsReg.Where(x => x.Name == SelectedReason).Select(x => x.Id)
                                    .FirstOrDefault();
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
                                var deregBook = new DeregBook()
                                {
                                    BookId = bId,
                                    ReasonId = dId,
                                    DateOfDereg = DateTime.Now,
                                    UserId = _loggedUser.CurrentUser.Id,
                                    DeregQuantity = SelectedBook?.QuantityToUpdate ?? 0
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
            new(_ =>
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
            new(_ =>
                {
                    var author = _repositoryAuthor.Get(x => x.FullName == SelectedAuthorForAdd) ?? new Author();
                    _addOrUpdateAuthorsView = new(author, this);
                    _addOrUpdateAuthorsView.ShowDialog();
                });

        public RelayCommand AddOrUpdatePublishingCommand =>
            new(_ =>
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
            new(_ =>
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
            new(_ =>
                {
                    if (MessageBox.Show("Вы уверены, что хотите отменить все изменения?",
                        "Отмена изменений",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.No) return;

                    if (SelectedBook != null)
                    {
                        SelectedBook.Quantity = BookForUpdate.Quantity;
                        SelectedBook.AuthorFullName = BookForUpdate.AuthorFullName;
                        SelectedBook.QuantityToUpdate = BookForUpdate.QuantityToUpdate;
                        SelectedBook.Title = BookForUpdate.Title;
                    }

                });

        public RelayCommand GiveCommand =>
            new(_ =>
            {
                _windowForGiveView = new(true);
                _windowForGiveView.ShowDialog();
            });

        public RelayCommand RecieveCommand
            => new(_ =>
            {
                _windowForGiveView = new(false);
                _windowForGiveView.ShowDialog();
            });


    }
}
