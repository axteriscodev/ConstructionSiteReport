using System.Diagnostics;
using ConstructionSiteLibrary.Interfaces;
namespace AppMAUI.Services
{
    public class LocationService : ILocationService
    {

        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public async Task<string> GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                {
                    Debug.WriteLine($"Latitude: {location.Latitude:0.#####}, Longitude: {location.Longitude:0.#####}, Altitude: {location.Altitude:0.#}");
                    string gpsLocation = $"Lat: {location.Latitude:0.#####}, Lon: {location.Longitude:0.#####}, Alt: {location.Altitude:0.#}";
                    return gpsLocation;
                }
                else
                {
                    return "NO GPS";
                }
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                // Unable to get location
                return "";
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }
    }
}