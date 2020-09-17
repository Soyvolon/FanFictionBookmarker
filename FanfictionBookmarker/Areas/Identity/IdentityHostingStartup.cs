using System;
using FanfictionBookmarker.Areas.Identity.Data;
using FanfictionBookmarker.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FanfictionBookmarker.Areas.Identity.IdentityHostingStartup))]
namespace FanfictionBookmarker.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {

        }
    }
}