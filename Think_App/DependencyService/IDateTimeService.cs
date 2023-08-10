using System;
namespace Think_App
{
    public interface IDateTimeService
    {
        DateTime GetNow();

        DateTime GetToday();
    }
}

