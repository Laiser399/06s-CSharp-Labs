using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Lab0613
{
    class SearchItemViewModel : BaseViewModel
    {
        public string BaseDir
        {
            get;
            private set;
        }
        public string RelativePath
        {
            get;
            private set;
        }
        public string Filename
        {
            get => Path.Combine(BaseDir, RelativePath);
        }

        private RelayCommand _openFileCommand, _archiveFile;
        public RelayCommand OpenFileCommand
        {
            get =>
                _openFileCommand ?? (_openFileCommand = new RelayCommand(_ => OpenFile()));
        }
        public RelayCommand ArchiveFileCommand
        {
            get =>
                _archiveFile ?? (_archiveFile = new RelayCommand(_ => ArchiveFile()));
        }

        public SearchItemViewModel(string baseDir, string relativePath)
        {
            BaseDir = baseDir;
            RelativePath = relativePath;
        }

        private void OpenFile()
        {
            Process proc = new Process();
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.FileName = Filename;
            proc.Start();
        }

        private void ArchiveFile()
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Архивировать в...";
                dialog.Filter = "Архив|*.gz";
                dialog.DefaultExt = ".gz";
                dialog.AddExtension = true;
                dialog.FileName = Path.GetFileName(RelativePath) + ".gz";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (!ArchiveClass.Compress(Filename, dialog.FileName))
                        MessageBox.Show("Ошибка архивации.", "Ошибка!");
                }
            }
        }


       
    }
}
