using System;
using Microsoft.Win32;

namespace WindowsFileLockerUtility
{
	public class LockFileSelectDialog
	{

		public string SelectedFile()
		{
			var dialog = new OpenFileDialog();
			dialog.InitialDirectory = Environment.CurrentDirectory;
			dialog.Filter = "All files (*.*)|*.*";
			dialog.FilterIndex = 1;
			dialog.RestoreDirectory = true;
			var dialogResult = dialog.ShowDialog();
			if (dialogResult != true) return null;
			return dialog.FileName;
		}
	}
}