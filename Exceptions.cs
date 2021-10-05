using System;
namespace HRV
{
    public class Exceptions
    {
        public Exceptions()
        {
        }
    }

    public class UserNotFoundInDatabase : Exception
    {
        public UserNotFoundInDatabase() : base("User Not Found in Database") { }
    }
}
