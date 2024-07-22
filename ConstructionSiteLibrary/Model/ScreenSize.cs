namespace ConstructionSiteLibrary.Model
{
    public class ScreenSize
    {
        public int Width { get; set; }
        public int Height { get; set; }


        public ScreenSize Clone()
        {
            return new() { Height = Height, Width = Width };
        }
    
    }
}
