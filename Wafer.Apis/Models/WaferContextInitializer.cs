using System.Collections.Generic;
using System.Linq;
using Wafer.Apis.Utils;

namespace Wafer.Apis.Models
{
    public class WaferContextInitializer
    {
        public static void Init(WaferContext context)
        {
            if (!context.Users.Any())
            {
                var encryptedPwd = Generator.EncryptedDefaultPassword();

                context.Users.Add(new User
                {
                    Username = "admin",
                    FullName = "administrator",
                    Password = encryptedPwd,
                    Email = "admin@sydq.net"
                });

                var users = Enumerable.Range(1, 24).Select(t => {
                    var info = Generator.NewUserInfo();
                    return new User
                    {
                        Username = info.name,
                        FullName = info.fullName,
                        Email = info.email,
                        Password = encryptedPwd
                    };
                });

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            if (!context.Menus.Any())
            {
                var menus = new List<Menu>
                {
                    new Menu{ Text = "Home", Url = "/home", IsActive = true },
                    new Menu{ Text = "User", Url = "/user", IsActive = true }
                };

                context.Menus.AddRange(menus);
                context.SaveChanges();
            }
        }
    }
}
