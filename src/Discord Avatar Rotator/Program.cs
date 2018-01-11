using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading;

namespace Discord_Avatar_Rotator
{
    class Program
    {
        static DiscordClient discord;
        static CommandsNextModule commands;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        class Botcommands
        {
            [Command("rotate"), Description("Rotate your avatar manually.")]
            public async Task Rotate()
            {


                    var rand = new Random();
                    var files = Directory.GetFiles("avatars");

                    if (files.Length != 0)
                    {
                        Stream avatar = File.OpenRead(files[rand.Next(files.Length)]);
                        await discord.EditCurrentUserAsync(avatar: avatar);
                    }

                    else
                    {
                        Console.WriteLine("It appears your avatars folder is empty. Please put any images in the avatars folder before running this bot.");
                    }
                }

            

            [Command("repeat"), Description("Rotate your avatar manually.")]
            public async Task Repeater()
            {
                try
                {
                    Console.WriteLine("What do you want to set your avatar refresh interval to? (in minutes, setting a value under 10 could get you rate-limited.");
                    var interval = Convert.ToInt32(Console.ReadLine());

                    while (true)
                    {
                        await Rotate();
                        Thread.Sleep(interval * 60000);
                    }
                }

                catch
                {
                    Console.WriteLine("Please choose a numeric value.");
                    await Repeater();
                }
            }
        }



    static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = System.IO.File.ReadAllText("token.txt"),
                TokenType = TokenType.User,
                AutoReconnect = true,
            });


            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = ";"
            });

            commands.RegisterCommands<Botcommands>();

            await discord.ConnectAsync();
            var instance = new Botcommands();
            await instance.Repeater();
            await Task.Delay(-1);


        }
    }
}
