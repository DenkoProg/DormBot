using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot
{
    class Program
    {
        private static TelegramBotClient client;
        private static Subject subject;
        private static List<string> shoppingList = new List<string>();

        static void Main(string[] args)
        {
            try
            {
                client = new TelegramBotClient("6177636296:AAFPJMLlNRUPqMuG2OUM4vQohm-2ACSTeGE");
                subject = new Subject(client);
                client.StartReceiving(Update, Error);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during initialization. Error: {ex.Message}");
            }
        }

        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            try
            {
                var message = update.Message;

                if (message != null && message.From != null)
                {
                    subject.AddObserver(message.From.Id);
                }

                if (message != null && message.Text != null)
                {
                    var text = message.Text.ToLower();

                    if (text.Contains("/hello"))
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Hello, nice to see you!");
                    }

                    else if (text.StartsWith("/add "))
                    {
                        var item = text.Substring(5);
                        shoppingList.Add(item);
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Added {item} to the shopping list.");
                    }
                    else if (text.StartsWith("/delete "))
                    {
                        var item = text.Substring(8);
                        if (shoppingList.Remove(item))
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, $"Removed {item} from the shopping list.");
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, $"{item} is not in the shopping list.");
                        }
                    }
                    else if (text == "/list")
                    {
                        if (shoppingList.Count == 0)
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, "The shopping list is empty.");
                            subject.NotifyAll("The shopping list is empty.");
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, string.Join("\n", shoppingList));
                            subject.NotifyAll(string.Join("\n", shoppingList));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during update. Error: {ex.Message}");
            }
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            Console.WriteLine($"An error occurred: {arg2.Message}");
            return Task.CompletedTask;
        }
    }
}
