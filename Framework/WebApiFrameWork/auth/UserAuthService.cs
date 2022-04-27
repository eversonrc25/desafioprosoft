using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApiFrameWork.auth
{
    public class UserAuthService : IUserAuthService
    {
         private LiteDatabase _liteDb;
        public UserAuth FindOne(String id)
        {
             return _liteDb.GetCollection<UserAuth>("UserAuth")
                .Find(x => x.id == id).FirstOrDefault();
        }

          public UserAuthService(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
        }

        public bool Insert(UserAuth userAuth)
        {
        //   _liteDb.GetCollection<UserAuth>("UserAuth").DeleteMany

            return _liteDb.GetCollection<UserAuth>("UserAuth")
                .Upsert(userAuth);
        }

       

      
            
        
    }
}