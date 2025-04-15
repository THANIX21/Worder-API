using WorderAPI.Classes.Interfaces;

namespace WorderAPI.Classes.Base
{
    public class WordType : IWordType
    {
        public int ID { get; set; }
        public string Type { get; set; }
    }
}
