using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot
{
    public class Observer
    {
        public long UserId { get; }
        private ITelegramBotClient botClient;

        public Observer(long userId, ITelegramBotClient botClient)
        {
            UserId = userId;
            this.botClient = botClient;
        }

        public async Task Notify(string message)
        {
            try
            {
                await botClient.SendTextMessageAsync(UserId, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when sending a message to the user {UserId}. Error: {ex.Message}");
            }
        }
    }
}
