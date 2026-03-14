namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : MyRecipeBookException
{
    public IList<string> ErrorMenssages { get; set; }

    public ErrorOnValidationException(IList<string> errorMenssages)
    {
        ErrorMenssages = errorMenssages;
    }
}