using ConstructionSiteLibrary.Model;
using Radzen;


namespace ConstructionSiteLibrary.Utility
{
    public class GlobalVariables
    {

        public const string MinWidthNavbat = "641px";
        public const string NavbarWidthOpen = "250px";
        public const string NavbarWidthClosed = "80px";
        public const Variant ComponentVariant = Variant.Outlined;
        public const int PageSize = 8;


        public NavbarState IsNavBarOpen { get; set; } = NavbarState.Open;


    }
}
