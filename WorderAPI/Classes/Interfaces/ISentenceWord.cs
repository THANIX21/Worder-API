namespace WorderAPI.Classes.Interfaces
{
    public interface ISentenceWord
    {
        int ID { get; set; }
        int IDSentence { get; set; }
        string Word { get; set; }
        int Position { get; set; }
    }
}
