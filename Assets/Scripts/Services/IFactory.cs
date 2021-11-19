namespace Services
{
    public interface IFactory<TObject>
    {
        TObject Get();
    }
}