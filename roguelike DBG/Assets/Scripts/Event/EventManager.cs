using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.ClassExtension;
using Utility.CustomClass;
using Utility.Interface;

namespace Event
{
    public class EventManager : Singleton<EventManager>
    {
        private readonly Dictionary<int, List<Action<IEventMessage>>> _eventHandler = 
            new Dictionary<int, List<Action<IEventMessage>>>();
    
        private readonly Dictionary<int, List<Action<IEventMessage>>> _eventHandlerAsync =
            new Dictionary<int, List<Action<IEventMessage>>>();
    
        #region 订阅事件

        private int GetIDFromType<T>()
        {
            var eventType = typeof(T);
            var eventID = eventType.GetHashCode();
            
            return eventID;
        }
        
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="action"></param>
        /// <param name="isAsync"></param>
        /// <typeparam name="T"></typeparam>
        public void SubscribeEvent<T>(Action<IEventMessage> action, bool isAsync = false) where T : IEventMessage
        {
            var eventID = GetIDFromType<T>();

            if (isAsync)
            {
                _eventHandlerAsync.TryAdd(eventID, action);
            }
            else
            {
                _eventHandler.TryAdd(eventID, action);
            }
        }

        #endregion

        #region 取消订阅

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public void UnsubscribeEvent<T>(Action<IEventMessage> action) where T : IEventMessage
        {
            var eventID = GetIDFromType<T>();

            _eventHandler.TryRemove(eventID, action);
            _eventHandlerAsync.TryRemove(eventID, action);
        }

        #endregion

        #region 调用事件

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="message"></param>
        /// <typeparam name="T"></typeparam>
        public async Task InvokeEvent<T>(T message) where T : IEventMessage
        {
            var eventID = GetIDFromType<T>();

            if (_eventHandler.TryGetValue(eventID, out var value))
            {
                await Task.WhenAll(value.Select(async handler => await Task.Run(() => handler.InvokeAsync(message))));
            }
        }

        public void Invoke<T>(T message) where T : IEventMessage
        {
            var eventID = GetIDFromType<T>();

            if (_eventHandler.TryGetValue(eventID, out var value))
            {
                foreach (var handler in value)
                {
                    handler.Invoke(message);
                }
            }
        }

        #endregion
    }
}
