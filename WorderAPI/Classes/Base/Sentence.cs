using WorderAPI.Classes.Interfaces;

namespace WorderAPI.Classes.Base
{
    public class Sentence : ISentence
    {
        public int ID { get; set; }
        public DateTime DTCreated { get; set; }
        public DateTime DTAltered { get; set; }
        public List<SentenceWord> Words { get; set; }
    }
}
