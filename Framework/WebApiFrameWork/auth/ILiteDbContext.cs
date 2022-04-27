using LiteDB;

namespace WebApiFrameWork.auth
{
    public interface ILiteDbContext
    {
        LiteDatabase Database { get; }
    }
}