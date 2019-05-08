namespace PdfPackage.Validator
{
    public interface IValidator
    {
        bool Validate(string fileName);
    }
}