namespace DragonspiritGames.PlatformerController
{
    public interface IWeather
    {
        public void StartWeather();
        public void StopWeather();
        public void SetIntensity(float intensity);
    }
}
