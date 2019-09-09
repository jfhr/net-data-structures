namespace NetDataStructures.Matrices
{
    public interface IMatrix
    {
        int this[int x, int y] { get; }

        int SizeX { get; }
        int SizeY { get; }
    }
}