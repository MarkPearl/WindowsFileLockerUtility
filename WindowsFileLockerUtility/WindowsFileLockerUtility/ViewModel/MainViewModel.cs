using System;
using System.IO;
using System.Windows.Input;
using WindowsFileLockerUtility.Domain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace WindowsFileLockerUtility.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool _locked;
        private string _fileToLock;

        private readonly FilesLocker _filesLocker;
	    private readonly LockFileSelectDialog _lockFileSelectDialog;

	    private bool _isFileToLockEnabled;
	    private bool _canLock;
	    private ICommand _addFile;

	    public MainViewModel()
        {
            _isFileToLockEnabled = true;
	        _canLock = false;
            _filesLocker = new FilesLocker();
	        _lockFileSelectDialog = new LockFileSelectDialog();
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
	            UpdateWhetherFileCanBeLocked();
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

	    public bool CanLock
	    {
		    get { return _canLock; }
		    private set
		    {
			    _canLock = value;
			    RaisePropertyChanged(() => CanLock);
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
			var proposedFileToSelect = _lockFileSelectDialog.SelectedFile();
			if (proposedFileToSelect == null) return;
			FileToLock = proposedFileToSelect;
		}

		private void UpdateWhetherFileCanBeLocked()
		{
			if (File.Exists(FileToLock))
			{
				CanLock = true;
				return;
			}
			CanLock = false;
		}
    }
}