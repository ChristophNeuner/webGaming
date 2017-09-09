using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using webGaming.Models;

namespace webGaming.Models.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("td", Attributes = "identity-role")]
    public class RoleUsersTagHelper : TagHelper
    {
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        //private object lockObject = new object();

        public RoleUsersTagHelper(UserManager<AppUser> um, RoleManager<IdentityRole> rm)
        {
            userManager = um;
            roleManager = rm;
        }

        [HtmlAttributeName("identity-role")]
        public string Role { get; set; }

        public override /*async*/ void Process(TagHelperContext context, TagHelperOutput output)
        {
            //lock (lockObject)
            //{
                List<string> names = new List<string>();
                IdentityRole role = roleManager.FindByIdAsync(Role).Result;
                if (role != null)
                {
                    foreach (var user in userManager.Users)
                    {
                        if (user != null && userManager.IsInRoleAsync(user, role.Name).Result)
                        {
                            names.Add(user.UserName);
                        }
                    }
                }

                output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names.ToArray()));
            //}

            //List<string> names = new List<string>();
            //IdentityRole role = await roleManager.FindByIdAsync(Role);
            //if (role != null)
            //{
            //    foreach (var user in userManager.Users)
            //    {
            //        if (user != null && await userManager.IsInRoleAsync(user, role.Name))
            //        {
            //            names.Add(user.UserName);
            //        }
            //    }
            //}

            //output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names.ToArray()));
        }
    }
}
