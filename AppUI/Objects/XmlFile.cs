namespace AppUI.Objects
{
    public class XmlFile
    {
        public string Name { get; set; } = null!;

        public string Path { get; set; } = null!;

        public string Size { get; set; } = null!;

        public DateTime ModificationDate { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
