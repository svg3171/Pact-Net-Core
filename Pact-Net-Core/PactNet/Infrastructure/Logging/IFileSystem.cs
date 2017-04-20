using System.IO;



namespace PactNet.Infrastructure.Logging
{
    public interface IFileSystem
    {
        IFile File { get; set; }
        IDirectory Directory { get; set; }
    }

    public interface IDirectory
    {
        void CreateDirectory(string pactConfigPactDir);
    }

    class FileSystem : IFileSystem
    {
        private readonly IFile _file = new MyFile();

        public IFile File
        {
            get
            {
                return _file;
            }

            set
            {
            }
        }


        private readonly IDirectory _directory = new MyDirectory();

        public IDirectory Directory
        {
            get { return _directory; }
            set { }
        }
    }


    public interface IFile 
    {
        Stream Open(string logFilePath, FileMode append, FileAccess write, FileShare read);
        void WriteAllText(string pactFilePath, string pactFileJson);
        string ReadAllText(string pactFileUri);
    }


    public class MyFile : IFile
    {
        public Stream Open(string logFilePath, FileMode append, FileAccess write, FileShare read)
        {
            var stream = File.Open(logFilePath, append, write, read);
            return stream;
        }


        public void WriteAllText(string pactFilePath, string pactFileJson)
        {
            File.WriteAllText(pactFilePath, pactFileJson);
        }


        public string ReadAllText(string pactFileUri)
        {
            return File.ReadAllText(pactFileUri);
        }
    }


    public class MyDirectory : IDirectory
    {
        public void CreateDirectory(string pactConfigPactDir)
        {
            Directory.CreateDirectory(pactConfigPactDir);
        }
    }
}