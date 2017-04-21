using Newtonsoft.Json;

namespace PactNet.Models
{

    public class PactFileMetadata
    {
        public string PactSpecificationVersion { get; set; }
    }
    public class PactFile : PactDetails
    {
        public PactFileMetadata Metadata { get; set; }

        public PactFile()
        {
            Metadata = new PactFileMetadata
            {
                PactSpecificationVersion = "1.1.0"
            };
        }
    }
}