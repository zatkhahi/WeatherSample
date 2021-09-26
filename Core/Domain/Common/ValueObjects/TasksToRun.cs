using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class TasksToRun
    {
        private readonly BlockingCollection<MediatorJob> _tasks;

        public TasksToRun() => _tasks = new BlockingCollection<MediatorJob>();

        public void Enqueue(MediatorJob settings) => _tasks.Add(settings);

        public MediatorJob Dequeue(CancellationToken token) => _tasks.Take(token);
    }
}
