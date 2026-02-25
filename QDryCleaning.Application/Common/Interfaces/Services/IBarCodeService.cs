namespace QDryClean.Application.Common.Interfaces.Services
{
    public interface IBarCodeService
    {
        byte[] GenerateCode128(string content);
    }
}
