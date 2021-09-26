using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBackgourndTaskScheduler
    {
        void EnqueTask(BackgroundTask task);
        BackgroundTask Take();
    }

    public class BackgroundTask
    {
        public Func<IServiceScope, Task> Task { get; set; }
        public IPrincipal Principal { get; set; }
    }
}
