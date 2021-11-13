using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace discordbot
{
    class main
    {
        //static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();
        static void Main(string[] args) => new main().StartBot();

        public static class tokenInformation
        {
            public static string Token { get; set; }
       
        }


        public void StartBot()
        {
            Console.WriteLine("Please enter the token.");

            tokenInformation.Token = Convert.ToString(Console.ReadLine());
            RunBotAsync().GetAwaiter().GetResult();
            
            

        }


        private DiscordSocketClient _client;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .BuildServiceProvider();

            string token = tokenInformation.Token;

            _client.Log += _client_Log;

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await Task.Delay(1700);

            await reader();


            
        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;

        }
 

        private async Task reader()
        {

            Console.WriteLine("Please put the server id.");
            string guildid = Convert.ToString(Console.ReadLine());
            Console.WriteLine(guildid.Length);

            if (guildid.Length < 10)
            {
                Console.WriteLine("Its not a serverid. (The server id too short.)");
                await reader();
                
            } else if (guildid.Length > 18)
            {
                Console.WriteLine("Its not a serverid.");
                await reader();
            }
            else if (guildid.Length == 18)
            {
                Console.WriteLine("Please put the number for the action.");
                Console.WriteLine("");
                var table = new ConsoleTables.ConsoleTable("Number", "Action", "Required Permission");
                table
                    .AddRow("1", "Kick all members from server", "KICK_MEMBERS")
                    .AddRow("2", "Ban all members from server", "BAN_MEMBERS")
                    .AddRow("3", "Delete all roles from server", "ADMINISTRATOR")
                    .AddRow("4", "Delete all channels from server", "DELETE_CHANNELS")
                    .AddRow("5", "Nuke server", "Administrator");



                table.Write();

                while(true)
                {
                    Console.WriteLine("Please put the number for the action.");

                    int number = Convert.ToInt32(Console.ReadLine());
                    var Guild = _client.GetGuild(Convert.ToUInt64(guildid));
                    switch (number)
                    {
                        case 1:
                            foreach(var users in Guild.Users)
                            {
                                bool checkPermission = users.GuildPermissions.Administrator || users.GuildPermissions.ManageGuild;
                                if (checkPermission)
                                {
                                    Console.WriteLine(users.Username + " ( " + users.Id + " )" +" has administrator/manageguild privileges!");                                    
                                } else
                                {
                                    try
                                    {
                                        Console.WriteLine(users.Username + " ( " + users.Id + " ) has been kicked.");
                                        await users.KickAsync();
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Failed to kicked to: " + e);
                                    }
                                }



                            }
                        break;

                        case 2:
                            foreach (var users in Guild.Users)
                            {
                                bool checkPermission = users.GuildPermissions.Administrator || users.GuildPermissions.ManageGuild;
                                if (checkPermission)
                                {
                                    Console.WriteLine(users.Username + " ( " + users.Id + " ) cannot banned. He have administrator privileges.");
                                } else
                                {

                                    _ = users.BanAsync();
                                    Console.WriteLine(users.Username + " (" + users.Id + " ) has been banned.");

                                }
                            }
                            break;

                        case 3:
                            foreach(var roles in Guild.Roles)
                            {
                                try
                                {
                                    _ = roles.DeleteAsync();
                                    Console.WriteLine(roles.Name + " role, has deleted.");
                                } catch (Exception e)
                                {
                                    Console.WriteLine("We could not delete the role.You probably don't have permission.");
                                }
                            }
                            break;
                        case 4:
                            foreach (var Channels in Guild.Channels)
                            {
                                try
                                {
                                    _ = Channels.DeleteAsync();
                                    Console.WriteLine(Channels.Name + " channel, has deleted.");
                                } catch (Exception e)
                                {
                                    Console.WriteLine("We could not delete the channels.You probably don't have permission.");
                                }
                            }
                            break;
                        case 5:
                            foreach(var Channels in Guild.Channels)
                            {
                                try
                                {
                                    _ = Channels.DeleteAsync();
                                    Console.WriteLine(Channels.Name + " channel, has deleted.");

                                } catch (Exception e)
                                {
                                    Console.WriteLine("We could not delete the channels.You probably don't have permission.");
                                }
                            }

                            foreach(var users in Guild.Users)
                            {
                                try
                                {
                                    bool checkPermission = users.GuildPermissions.Administrator || users.GuildPermissions.ManageGuild;
                                    if (!checkPermission)
                                    {
                                        _ = users.KickAsync();
                                        Console.WriteLine(users.Username + " ( " + users.Id + " ) has kicked from the server.");
                                    }
                                } catch (Exception e)
                                {
                                    Console.WriteLine("We could not delete the users.You probably don't have permission.");
                                }
                            }

                            foreach(var Roles in Guild.Roles)
                            {
                                try
                                {
                                    Roles.DeleteAsync();
                                    Console.WriteLine(Roles.Name + " role, has deleted.");
                                } catch (Exception e)
                                {
                                    Console.WriteLine("We could not delete the rank.You probably don't have permission.");
                                }
                            }

                            

                            break;
                    }

                }
                

            }

            

        }

        public class async
        {
        }
    }

}

