using Domain.Entities;
using Infrastructure.Databases.MySql;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions
{
    public static class MySqlExtensions
    {
        public static IApplicationBuilder ApplyMySqlMigrations(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                                        .GetRequiredService<IServiceScopeFactory>()
                                        .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<MySqlContext>();
            context?.Database.Migrate();

            return app;
        }

        public static IApplicationBuilder SeedMySqlDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                            .GetRequiredService<IServiceScopeFactory>()
                            .CreateScope();

            using (var context = serviceScope.ServiceProvider.GetService<MySqlContext>())
            {
                //Avoid to apply seeds again
                if (context.Users.Any())
                {
                    return app;
                }

                //Creating Users
                var william = new User { UserName = "William", CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)) };
                var rafaela = new User { UserName = "Rafaela", CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)) };
                var joana = new User { UserName = "Joana", CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)) };
                var patrick = new User { UserName = "Patrick", CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)) };
                var fabio = new User { UserName = "fabio", CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)) };

                context.Users.AddRange(new List<User> { william, rafaela, joana, patrick, fabio });
                context.SaveChanges();

                //William is selling a car
                var williamsPost = new Post 
                { 
                    UserId = william.Id, 
                    Content = "I'm selling a car, anyone interested please get in touch!",
                    CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                    IsQUote = false,
                    IsRepost = false,
                    Quote = null,
                    OriginalPostId = null
                };
                context.Posts.Add(williamsPost);
                context.SaveChanges();

                //Rafaela is trying to help William by repost the announcement
                var rafaelasRepost = new Post
                {
                    UserId = rafaela.Id,
                    Content = null,
                    CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                    IsQUote = false,
                    IsRepost = true,
                    Quote = null,
                    OriginalPostId = williamsPost.Id
                };
                context.Posts.Add(rafaelasRepost);
                context.SaveChanges();

                //William thanks Rafaela for her support
                var williamsQuote = new Post
                {
                    UserId = william.Id,
                    Content = null,
                    CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                    IsQUote = true,
                    IsRepost = false,
                    Quote = "I managed to sell my car thanks to your repost!",
                    OriginalPostId = rafaelasRepost.Id
                };
                context.Posts.Add(williamsQuote);
                context.SaveChanges();

                //Joana is making some posts
                var joanasFirstPost = new Post
                {
                    UserId = joana.Id,
                    Content = "I'm looking forward to going to the beach",
                    CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                    IsQUote = false,
                    IsRepost = false,
                    Quote = null,
                    OriginalPostId = null
                };
                var joanasSecondPost = new Post
                {
                    UserId = joana.Id,
                    Content = "I almost forgot sunscreen...",
                    CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                    IsQUote = false,
                    IsRepost = false,
                    Quote = null,
                    OriginalPostId = null
                };
                var joanasThirdPost = new Post
                {
                    UserId = joana.Id,
                    Content = "What bad luck, I arrived at the beach and it started to rain",
                    CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                    IsQUote = false,
                    IsRepost = false,
                    Quote = null,
                    OriginalPostId = null
                };
                context.Posts.AddRange(new List<Post> { joanasFirstPost, joanasSecondPost, joanasThirdPost });
                context.SaveChanges();

                //Patrick and Fabio starts to interact with Joana
                var fabiosQuote = new Post
                {
                    UserId = fabio.Id,
                    Content = null,
                    CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                    IsQUote = true,
                    IsRepost = false,
                    Quote = "enjoy the beach even in the rain!",
                    OriginalPostId = joanasThirdPost.Id
                };
                var patricksQuote = new Post
                {
                    UserId = fabio.Id,
                    Content = null,
                    CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                    IsQUote = true,
                    IsRepost = false,
                    Quote = "be careful with the storm",
                    OriginalPostId = joanasThirdPost.Id
                };
                context.Posts.AddRange(new List<Post> { fabiosQuote, patricksQuote });
                context.SaveChanges();

            }

            return app;
        }
    }
}
