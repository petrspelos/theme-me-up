namespace ThemeMeUp.ApiWrapper.Entities.Api
{
    public class Purity
    {
        public bool SFW { get; set; }
        public bool Sketchy { get; set; }
        public bool NSFW { get; set; }

        public Purity() { }

        public Purity(byte raw)
        {
            SFW = raw.IsBitSet(3);
            Sketchy = raw.IsBitSet(2);
            NSFW = raw.IsBitSet(1);
        }

        public Purity(string input)
        {
            input = input.Trim().ToLower();

            if(input == "sfw")
            {
                SFW = true;
            }
            else if(input == "sketchy")
            {
                Sketchy = true;
            }
            else if(input == "nsfw")
            {
                NSFW = true;
            }
        }

        public string ToQueryParameter() => $"purity={SFW.ToBinaryString()}{Sketchy.ToBinaryString()}{NSFW.ToBinaryString()}";
    }
}
