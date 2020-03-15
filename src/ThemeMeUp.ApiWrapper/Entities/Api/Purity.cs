namespace ThemeMeUp.ApiWrapper.Entities.Api
{
    public class Purity
    {
        public bool SFW { get; private set; }
        public bool Sketchy { get; private set; }
        public bool NSFW { get; private set; }

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
    }
}
