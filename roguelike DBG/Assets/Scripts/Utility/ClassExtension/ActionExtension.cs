using System;
using System.Threading.Tasks;

namespace Utility.ClassExtension
{
    public static class ActionExtension
    {
        public static Task InvokeAsync<TMessage>(this Action<TMessage> handler, TMessage message)
        {
            return Task.Run(() => handler.Invoke(message));
        }
    }
}