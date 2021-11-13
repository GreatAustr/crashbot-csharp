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
            Console.WriteLine("Дайте токен");

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

            Console.WriteLine("Вставьте айди сервера");
            string guildid = Convert.ToString(Console.ReadLine());
            Console.WriteLine(guildid.Length);

            if (guildid.Length < 10)
            {
                Console.WriteLine("Введите корректный айди сервера(этот слишком короткий)");
                await reader();
                
            } else if (guildid.Length > 18)
            {
                Console.WriteLine("слишком длинный ID сервера(у вас ошибка)");
                await reader();
            }
            else if (guildid.Length == 18)
            {
                Console.WriteLine("Выберите цифру и вставьте ее в консоль");
                Console.WriteLine("");
                var table = new ConsoleTables.ConsoleTable("Number", "Action", "Required Permission");
                table
                    .AddRow("1", "Кикнуть всех", "KICK_MEMBERS")
                    .AddRow("2", "Забанить всех", "BAN_MEMBERS")
                    .AddRow("3", "Удалить все роли на сервере", "ADMINISTRATOR")
                    .AddRow("4", "Удалить все каналы на сервере", "DELETE_CHANNELS")
                    .AddRow("5", "Ебнуть сервер", "Administrator");



                table.Write();

                while(true)
                {
                    Console.WriteLine("Выберите цифру и вставьте ее в консоль");

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
                                    Console.WriteLine(users.Username + " ( " + users.Id + " )" +" имеет права");                                    
                                } else
                                {
                                    try
                                    {
                                        Console.WriteLine(users.Username + " ( " + users.Id + " ) кикнут");
                                        await users.KickAsync();
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("не удалось кикнуть " + e);
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
                                    Console.WriteLine(users.Username + " ( " + users.Id + " ) Не забанен  у него админка");
                                } else
                                {

                                    _ = users.BanAsync();
                                    Console.WriteLine(users.Username + " (" + users.Id + " ) забанен");

                                }
                            }
                            break;

                        case 3:
                            foreach(var roles in Guild.Roles)
                            {
                                try
                                {
                                    _ = roles.DeleteAsync();
                                    Console.WriteLine(roles.Name + " удалена");
                                } catch (Exception e)
                                {
                                    Console.WriteLine("не могу удалить роль");
                                }
                            }
                            break;
                        case 4:
                            foreach (var Channels in Guild.Channels)
                            {
                                try
                                {
                                    _ = Channels.DeleteAsync();
                                    Console.WriteLine(Channels.Name + " канал был удален");
                                } catch (Exception e)
                                {
                                    Console.WriteLine("не удалось удалить канал");
                                }
                            }
                            break;
                        case 5:
                            foreach(var Channels in Guild.Channels)
                            {
                                try
                                {
                                    _ = Channels.DeleteAsync();
                                    Console.WriteLine(Channels.Name + " канал удален");

                                } catch (Exception e)
                                {
                                    Console.WriteLine("я немгу удалить канал");
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
                                        Console.WriteLine(users.Username + " ( " + users.Id + " ) кикнут с сервера");
                                    }
                                } catch (Exception e)
                                {
                                    Console.WriteLine("ошибка в исключении пользователя");
                                }
                            }

                            foreach(var Roles in Guild.Roles)
                            {
                                try
                                {
                                    Roles.DeleteAsync();
                                    Console.WriteLine(Roles.Name + " роль удалена");
                                } catch (Exception e)
                                {
                                    Console.WriteLine("не смог удалить роль");
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
