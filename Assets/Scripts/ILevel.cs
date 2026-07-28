public interface ILevel
{
    int Width { get; }
    int Length { get; }

    bool IsBlocked(int x, int y);
    int Floor(int x, int y);
}