namespace Services
{
    public interface IRandomService
    {
        float Next(float minValue, float maxValue);
        int Next(int minValue, int maxValue);
    }
}