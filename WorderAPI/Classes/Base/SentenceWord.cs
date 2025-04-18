using WorderAPI.Classes.Interfaces;

namespace WorderAPI.Classes.Base
{
    public class SentenceWord : ISentenceWord
    {
        public int ID { get; set; }
        public int IDSentence { get; set; }
        public string Word { get; set; }
        public int Position { get; set; }
    }
}
