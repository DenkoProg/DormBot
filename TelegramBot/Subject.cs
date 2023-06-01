using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;


namespace TelegramBot
{
    public class Subject
    {
        private readonly ITelegramBotClient botClient;
        private List<Observer> observers = new List<Observer>();

        public Subject(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
        }

        public async Task NotifyAll(string message)
        {
            foreach (var observer in observers)
            {
                await observer.Notify(message);
            }
        }

        public void AddObserver(long userId)
        {
            if (!observers.Any(o => o.UserId == userId))
            {
                observers.Add(new Observer(userId, botClient));
            }
        }
    }

}
