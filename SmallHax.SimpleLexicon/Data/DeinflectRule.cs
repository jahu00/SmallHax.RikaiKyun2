namespace SmallHax.SimpleLexicon.Data
{
    public enum DeinflectType
    {
        Prefix,
        Suffix
    }

    public class DeinflectRule
    {
        public string DeinflectId { get; set; }
        public DeinflectType Type { get; set; }
        public string Text { get; set; }
        public string Replace { get; set; }
        public string[] Tags { get; set; }
    }
}