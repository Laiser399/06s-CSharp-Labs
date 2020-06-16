using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Lab0613
{
    class MainWindowViewModel : BaseViewModel
    {
        private string _searchDir = @"C:\Users\Sergey\Desktop\poe";
        public string SearchDir
        {
            get => _searchDir;
            set
            {
                _searchDir = value;
                NotifyPropertyChanged(nameof(SearchDir));
            }
        }

        private RelayCommand _changeSearchDirCommand;
        public RelayCommand ChangeSearchDirCommand
        {
            get =>
                _changeSearchDirCommand ?? (_changeSearchDirCommand = new RelayCommand(_ => ChangeSearchDir()));
        }

        private string _pattern = ".txt$";
        public string Pattern
        {
            get => _pattern;
            set
            {
                _pattern = value;
                NotifyPropertyChanged(nameof(Pattern));
            }
        }

        private bool _isRecursivelySearch = false;
        public bool IsRecursivelySearch
        {
            get => _isRecursivelySearch;
            set
            {
                _isRecursivelySearch = value;
                NotifyPropertyChanged(nameof(IsRecursivelySearch));
            }
        }

        private RelayCommand _searchCommand;
        public RelayCommand SearchCommand
        {
            get =>
                _searchCommand ?? (_searchCommand = new RelayCommand(_ => Search()));
        }

        private ObservableCollection<SearchItemViewModel> _searchResults;
        public ObservableCollection<SearchItemViewModel> SearchResults
        {
            get =>
                _searchResults ?? (_searchResults = new ObservableCollection<SearchItemViewModel>());
        }

        private RelayCommand _archiveAllCommand;
        public RelayCommand ArchiveAllCommand
        {
            get =>
                _archiveAllCommand ?? (_archiveAllCommand = new RelayCommand(_ => ArchiveAll()));
        }

        public MainWindowViewModel()
        {
        }

        private void ChangeSearchDir()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (Directory.Exists(SearchDir))
                    dialog.SelectedPath = SearchDir;

                if (dialog.ShowDialog() == DialogResult.OK)
                    SearchDir = dialog.SelectedPath;
            }
        }

        private void Search()
        {
            Regex pattern;
            try
            {
                pattern = new Regex(Pattern);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Неверный паттерн.", "Ошибка!");
                return;
            }

            List<string> relativePaths;
            try
            {
                relativePaths = SearchClass.FindFiles(SearchDir, pattern, IsRecursivelySearch);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Ошибка поиска. Возможно, указан неверный путь.", "Ошибка!");
                return;
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Указанная директория не существует.", "Ошибка!");
                return;
            }

            SearchResults.Clear();
            foreach (string relativePath in relativePaths)
                SearchResults.Add(new SearchItemViewModel(SearchDir, relativePath));
        }

        private void ArchiveAll()
        {
            if (SearchResults.Count == 0)
                return;

            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                string archiveDir = dialog.SelectedPath;
                if (Directory.GetFileSystemEntries(archiveDir).Length != 0)
                {
                    if (MessageBox.Show($"Директория {archiveDir} не пуста. Продолжить?", "?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        return;
                }

                bool isAllNice = true;
                foreach (var item in SearchResults)
                {
                    string archiveFilename = Path.Combine(archiveDir, item.RelativePath) + ".gz";
                    Directory.CreateDirectory(Path.GetDirectoryName(archiveFilename));
                    isAllNice &= ArchiveClass.Compress(item.Filename, archiveFilename);
                }

                if (isAllNice)
                    MessageBox.Show("Архивация завершена.", "Инфо");
                else
                    MessageBox.Show("Возникли ошибки во время архивации. Не все файлы архивированы.", "Инфо");
            }
        }
    }
}
