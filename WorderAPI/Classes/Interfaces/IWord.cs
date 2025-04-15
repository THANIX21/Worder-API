namespace WorderAPI.Classes.Interfaces
{
    public interface IWord
    {
        int ID { get; set; }
        string Term { get; set; }
        DateTime DTCreated { get; set; }
        DateTime DTAltered { get; set; }
        int Type { get; set; }
    }
}
