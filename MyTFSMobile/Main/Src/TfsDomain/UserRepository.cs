using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfsMobile.Contracts;

namespace TfsDomain
{
    public class UserRepository
    {

        public UserEntity GetUser(string accessKey)
        {
            using (var context = new TfsMobileEntities())
            {
                var usr = context.UserEntitySet.FirstOrDefault(u => u.AccessKey == accessKey);
                if (usr == null)
                {
                    return new DummyUser();
                }
                UpdateLastLoggedInDate(usr, context);
                return usr;
            }
        }

        public UserEntity GetUser(RequestTfsUserDto tfsUser)
        {
            using (var context = new TfsMobileEntities())
            {
                var usr = context.UserEntitySet.FirstOrDefault(u => u.Login == tfsUser.Username);
                if (usr == null)
                {
                    return new DummyUser();
                }
                UpdateLastLoggedInDate(usr, context);
                return usr;
            }
        }

        public string CreateUser(RequestTfsUserDto tfsUser)
        {
            //Check if tfsUser exists with these credential
            var usr = GetUser(tfsUser);
            if (usr is DummyUser)
            {
                usr = CreateThisUser(tfsUser);
            }
            //Create User
            //Return accessKey
            return usr.AccessKey;
        }

        private UserEntity CreateThisUser(RequestTfsUserDto tfsUser)
        {
            using (var context = new TfsMobileEntities())
            {
                var userEntity = new UserEntity()
                {
                    AccessKey = "",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Login = tfsUser.Username,
                    Password = tfsUser.Password,
                    TfsUrl = tfsUser.TfsUri.ToString()
                };
                context.UserEntitySet.Add(userEntity);
                context.SaveChanges();
                return userEntity;

            }
        }

        private void UpdateLastLoggedInDate(UserEntity usr, TfsMobileEntities context)
        {
            usr.LastLoggedInDate = DateTime.Now;
            context.SaveChanges();
        }
    }


    public partial class UserEntity
    {
        
    }

    public class DummyUser : UserEntity
    {
        public DummyUser()
        {
            AccessKey = string.Empty;
            CreatedDate = DateTime.Now;
            Id = 0;
            Login = "Not Authorized";
            ModifiedDate = DateTime.Now;
            Password = string.Empty;
        }
    }
}
