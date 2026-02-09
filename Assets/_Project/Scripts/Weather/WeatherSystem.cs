using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class  WeatherSystem : MonoBehaviour
    {
        [SerializeField] MonoBehaviour _initialWeather;

        IWeather _currentWeather;

        void Start()
        {
            if (_initialWeather is IWeather weather)
            {
                SetWeather(weather, 1f);
            }
        }

        public void SetWeather(IWeather newWeather, float intensity = 1f)
        {
            _currentWeather?.StopWeather();
            _currentWeather = newWeather;

            if (_currentWeather == null) return;
            
            _currentWeather.StartWeather();
            _currentWeather.SetIntensity(intensity);
        }

        public void ClearWeather()
        {
            _currentWeather?.StopWeather();
            _currentWeather = null;
        }

        public void SetIntensity(float intensity)
        {
            _currentWeather?.SetIntensity(intensity);
        }
    }
}