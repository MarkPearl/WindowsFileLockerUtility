using System;
using System.IO;
using System.Windows.Input;
using WindowsFileLockerUtility.Domain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace WindowsFileLockerUtility.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool _locked;
        private string _fileToLock;

        private readonly FilesLocker _filesLocker;
        private bool _isFileToLockEnabled;
        private ICommand _addFile;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _isFileToLockEnabled = true;
            _filesLocker = new FilesLocker();
        }

        public ICommand AddFileCommand
        {
            get
            {
                return _addFile ?? (_addFile = new RelayCommand(SelectFileToAdd));
            }
        }

        public bool Locked
        {
            get
            {
                return _locked;
            }
            set
            {
                _locked = value;
                RaisePropertyChanged(() => Locked);
                UpdateLockedFile();
            }
        }

        private void UpdateLockedFile()
        {
            if (!File.Exists(FileToLock))
            {
                Locked = false;
            }

            if (Locked)
            {
                _filesLocker.LockFile(FileToLock);
                IsFileToLockEnabled = false;
                return;
            }

            _filesLocker.UnlockFile(FileToLock);
            IsFileToLockEnabled = true;
        }

        private void SelectFileToAdd()
        {
            var dialog = new OpenFileDialog();
	        dialog.InitialDirectory = Environment.CurrentDirectory;
            dialog.Filter = "All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;
            var dialogResult = dialog.ShowDialog();
            if (dialogResult != true) return;

            FileToLock = dialog.FileName;
        }

        public string FileToLock
        {
            get
            {
                return _fileToLock;
            }
            set
            {
                _fileToLock = value;
                RaisePropertyChanged(() => FileToLock);
            }
        }

        public bool IsFileToLockEnabled
        {
            get { return _isFileToLockEnabled; }
            set
            {
                _isFileToLockEnabled = value;
                RaisePropertyChanged(() => IsFileToLockEnabled);
            }
        }
    }
}