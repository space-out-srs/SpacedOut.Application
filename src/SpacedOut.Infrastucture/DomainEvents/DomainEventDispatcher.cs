using SpacedOut.SharedKernal.Interfaces;
using SpacedOut.SharedKernel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpacedOut.Infrastucture.DomainEvents
{
    // https://gist.github.com/jbogard/54d6569e883f63afebc7
    // http://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern/
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _services;

        public DomainEventDispatcher(IServiceProvider services)
        {
            _services = services;
        }

        public async Task Dispatch(BaseDomainEvent domainEvent)
        {
            var handlers = GetWrappedHandlers<DomainEventHandler>(
                domainEvent,
                typeof(IHandle<>),
                typeof(DomainEventHandler<>)
            );

            foreach (DomainEventHandler? handler in handlers)
            {
                if (handler != null)
                {
                    await handler.Handle(domainEvent).ConfigureAwait(false);
                }
            }
        }

        private abstract class DomainEventHandler
        {
            public abstract Task Handle(BaseDomainEvent domainEvent);
        }

        private class DomainEventHandler<T> : DomainEventHandler where T : BaseDomainEvent
        {
            private readonly IHandle<T> _handler;

            public DomainEventHandler(IHandle<T> handler)
            {
                _handler = handler;
            }

            public override Task Handle(BaseDomainEvent domainEvent)
            {
                return _handler.Handle((T)domainEvent);
            }
        }

        private IEnumerable<T?> GetWrappedHandlers<T>(BaseDomainEvent domainEvent, Type handlerType, Type wrappedHandlerType)
        {
            var genericHandlerType = handlerType.MakeGenericType(domainEvent.GetType());
            var handlers = (IEnumerable?)_services.GetService(typeof(IEnumerable<>).MakeGenericType(genericHandlerType));

            if (handlers != null)
            {
                var wrapperType = wrappedHandlerType.MakeGenericType(domainEvent.GetType());
                var wrappedHandlers = handlers
                    .Cast<object>()
                    .Select(handler => (T?)Activator.CreateInstance(wrapperType, handler));

                return wrappedHandlers;
            }

            return new List<T>();
        }
    }
}