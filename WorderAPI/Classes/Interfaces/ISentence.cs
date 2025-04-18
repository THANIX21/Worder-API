using WorderAPI.Classes.Base;

namespace WorderAPI.Classes.Interfaces
{
    public interface ISentence
    {
        int ID { get; set; }
        DateTime DTCreated { get; set; }
        DateTime DTAltered { get; set; }
        List<SentenceWord> Words { get; set; }
    }
}
