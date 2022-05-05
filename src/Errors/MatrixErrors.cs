namespace EvilGiraffes.Errors;
public class MatrixInitializedException: BaseException
{
    new protected static string  defaultMessage = "Matrix has already been initialized.";
}