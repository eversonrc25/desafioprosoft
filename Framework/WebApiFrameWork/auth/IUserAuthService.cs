using System;
using System.Collections.Generic;

namespace WebApiFrameWork.auth
{
    public interface IUserAuthService
    {
         
        UserAuth FindOne(String id);
        bool Insert(UserAuth forecast);
        //bool Update(WeatherForecast forecast);
    }
}