using System.Collections.Generic;
using System.IO;

namespace WindowsFileLockerUtility.Domain
{
    public class FilesLocker
    {
        private readonly Dictionary<string, FileStream> _lockedFiles;

        public FilesLocker()
        {
            _lockedFiles = new Dictionary<string, FileStream>();
        }

        public void LockFile(string filename)
        {
            var lockedFileStream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.None); 
            _lockedFiles.Add(filename, lockedFileStream);
        }

        public void UnlockFile(string filename)
        {
            var fs = _lockedFiles[filename];
            fs.Close();
            _lockedFiles.Remove(filename);
        }
    }
}