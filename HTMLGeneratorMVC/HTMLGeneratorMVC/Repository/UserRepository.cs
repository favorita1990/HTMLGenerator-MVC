using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HTMLGeneratorMVC.Context;
using HTMLGeneratorMVC.Models;

namespace OnlineFlowerShop.Repository
{
    public class UserRepository
    {
        private GeneratorDb db = new GeneratorDb();
       
        public int FlowerUserId(string email)
        {
            try
            {
                return db.Users.Where(w => w.Email == email).Select(s => s.Id).ToList().Last();
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public string UserRoleName(string email)
        {
            try
            {
                //var role = (from r in db.Roles where r.Name.Contains("Admin") select r).FirstOrDefault();
                //var users = db.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();
                //if (users.Find(x => x.Id == userId) != null)

                var user = db.Users.First(u => u.Email == email);
                int userId = user.Id;


                var userRoles = db.Roles.Include(r => r.Users).ToList();

                var userRoleNames = (from r in userRoles
                                     from u in r.Users
                                     where u.UserId == userId
                                     select r.Name).FirstOrDefault();


                if (userRoleNames != null)
                {
                    return userRoleNames;
                }
                else
                {

                    return "";
                }


            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}