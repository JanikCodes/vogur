using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component that can receive tasks and queue them. 
/// </summary>
public class TaskQueue : MonoBehaviour
{
    public Queue<Task> Tasks { get; private set; }

    private void Start()
    {
        // Initialize the queue
        Tasks = new();

        // TODO: remove below
        // Temporary add hard-coded tasks
        Task task1 = new();
        task1.TaskType = TaskType.Wait;
        QueueTask(task1);
    }

    public Task GetCurrentTask()
    {
        return Tasks.Peek();
    }

    public void QueueTask(Task task)
    {
        Tasks.Enqueue(task);
    }

    public void CompleteCurrentTask()
    {
        if (Tasks.Count > 0)
        {
            Tasks.Dequeue();

            Task taskLoop = new();
            taskLoop.TaskType = TaskType.Roam;
            QueueTask(taskLoop);
        }
    }
}
