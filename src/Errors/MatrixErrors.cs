namespace EvilGiraffes.Errors;
public class MatrixInitializedException: BaseException {}
public class MatrixOutOfBoundsException: BaseException
{
    public int MatrixLength { get; init; }
    public int FullLength { get; init; }
    public MatrixOutOfBoundsException(int matrixLength, int fullLength)
    {
        MatrixLength = matrixLength;
        FullLength = fullLength;
    }
    public MatrixOutOfBoundsException(string message, int matrixLength, int fullLength): base(message)
    {
        MatrixLength = matrixLength;
        FullLength = fullLength;
    }
}