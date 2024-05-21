namespace ConstructionSiteLibrary.Interfaces
{
    public interface ILocationService
    {
        public Task<string> GetCurrentLocation();
    }
}
