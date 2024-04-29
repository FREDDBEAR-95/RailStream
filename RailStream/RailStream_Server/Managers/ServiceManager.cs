using RailStream_Server_Backend.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server_Backend.Managers
{
    internal class ServiceManager
    {
        private IList<IServiceBase> Services { get; set; }
        private IList<Thread> Threads { get; } = new List<Thread>();

        public ServiceManager(IList<IServiceBase> services)
        {
            Services = services;
        }

        public void AddService(IServiceBase service)
        {
            Services.Add(service);
        }

        public void RemoveService(IServiceBase service)
        {
            Services.Remove(service);
        }

        public void StartServices()
        {
            foreach (var service in Services)
            {
                Thread thread = new Thread(service.Start);
                Threads.Add(thread);
                thread.Start();
            }
        }

        public void StopServices()
        {
            foreach (var item in Services.Zip(Threads, (s, t) => new {service = s, thread = t}))
            {
                item.service.Stop();
                item.thread.Join();
            }
        }
    }
}