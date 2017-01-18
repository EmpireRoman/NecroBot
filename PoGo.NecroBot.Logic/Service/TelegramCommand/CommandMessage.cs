﻿using System;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using Telegram.Bot.Types;

namespace PoGo.NecroBot.Logic.Service.TelegramCommand
{
    public abstract class CommandMessage : ICommandGenerify<string>
    {
        protected readonly TelegramUtils TelegramUtils;

        protected CommandMessage(TelegramUtils telegramUtils)
        {
            TelegramUtils = telegramUtils;
        }

        public abstract string Command { get; }
        public abstract string Description { get; }
        public abstract bool StopProcess { get; }

        public abstract Task<bool> OnCommand(ISession session, string cmd, Action<string> callback);

        public Task<bool> OnCommand(ISession session, string cmd, Message telegramMessage)
        {
            Action<string> callback = async msg =>
            {
                try
                {
                    await TelegramUtils.SendMessage(msg, telegramMessage.Chat.Id);
                }
                catch (Exception ex)
                {
                    session.EventDispatcher.Send(new ErrorEvent {Message = ex.Message});
                    session.EventDispatcher.Send(new ErrorEvent {Message = "Unkown Telegram Error occured. "});
                }
            };

            return OnCommand(session, cmd, callback);
        }
    }
}