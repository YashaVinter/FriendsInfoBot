using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types.ReplyMarkups;

namespace EntityFrameworkApp.Data
{
    internal class FrontendInterfaces
    {
    }
    public interface IFrontendData
    {
        Dictionary<string, IButtonData> buttonsByState { get; init; }
        Dictionary<string, IEventData> caseEventByState { get; init; }
    }
    public interface IButtonData
    {
        string text { get; init; }
        public IReplyMarkup buttons { get; init; }
    }
    public interface IEventData
    {
        string eventText { get; init; }
    }
}
