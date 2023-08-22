using System;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class ProcessManager
	{
		public static bool IsRunning;
		public ProcessManager()
		{
		}

		public bool CanInvoke()
		{
			if (IsRunning)
			{
				return false;
			}
			else
			{
				IsRunning = true;
				return true;
			}
		}
		public void OnComplete()
		{
			Task.Run(async () =>
			{
				// Delayしないと連打できてしまう
				await Task.Delay(250);
				IsRunning = false;
			});
		}

		private List<RunningTask> _RunningTasks;
		public List<RunningTask> RunningTasks
		{
			get
			{
				if (_RunningTasks == null)
				{
					_RunningTasks = new List<RunningTask>();
				}
				return _RunningTasks;
			}
			set
			{
				_RunningTasks = value;
				if (value.Count == 0)
				{
					App.customNavigationPage.IsRunning = false;
				}
				else
				{
					App.customNavigationPage.IsRunning = true;
				}
			}
		}

		RunningTask runningTask;

		//pageなどから生成したIDで管理する場合
		public Guid AddTasksSetID(Task<object> task, Guid guid)
		{
			try
			{
				System.Diagnostics.Debug.WriteLine("Guid.NewGuid().ToString()  :" + guid.ToString());
				runningTask = new RunningTask()
				{
					Task = task,
					TaskID = guid,
				};
				RunningTasks.Add(runningTask);
			}
			catch (Exception ex)
			{
				return Guid.Empty;
			}
			return runningTask.TaskID;
		}


		//ProcessManagerで生成したIDで管理する場合
		public Guid AddTasksGetID(Task<object> task)
		{
			try
			{
				var guid = Guid.NewGuid();
				System.Diagnostics.Debug.WriteLine("Guid.NewGuid().ToString()  :" + guid);
				runningTask = new RunningTask()
				{
					Task = task,
					TaskID = guid,
				};
				RunningTasks.Add(runningTask);
			}
			catch (Exception ex)
			{
				return Guid.Empty;
			}
			return runningTask.TaskID;
		}


		//Pageで管理する場合
		public void AddTasks(Task<object> task, Microsoft.Maui.Controls.Page page)
		{
			try
			{
				System.Diagnostics.Debug.WriteLine("page id   :" + page.Id.ToString());
				runningTask = new RunningTask()
				{
					Task = task,
					TaskID = page.Id,
				};
				RunningTasks.Add(runningTask);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("ex :" + ex);
			}
		}
		public void RemoveTask(Guid guid)
		{
			for (int i = 0; i < RunningTasks.Count; i++)
			{
				if (RunningTasks[i].TaskID == guid)
				{
					RunningTasks.RemoveAt(i);
				}
			}
		}

		public void CanselTasks(Guid guid)
		{
			for (int i = 0; i < RunningTasks.Count; i++)
			{
				if (RunningTasks[i].TaskID == guid)
				{
					RunningTasks.RemoveAt(i);
				}
			}
			//var a = RunningTasks
			//	.FirstOrDefault(i => i.TaskID == guid);
		}
		public class RunningTask
		{
			public Task<object> Task { get; set; }
			public Guid TaskID { get; set; }
		}

	}
}