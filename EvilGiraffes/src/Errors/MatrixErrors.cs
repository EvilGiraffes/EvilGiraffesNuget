namespace EvilGiraffes.Errors;
/// <summary>
/// Will be thrown if the matrix has already been initialized.
/// </summary>
public class MatrixInitializedException: BaseException {}
/// <summary>
/// Will be thrown if setting out of bounds of the matrix.
/// </summary>
public class MatrixOutOfBoundsException: BaseException
{
    /// <summary>
    /// The length of matrix row or column
    /// </summary>
    /// <value>Int value of the length</value>
    public int MatrixLength { get; init; }
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
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