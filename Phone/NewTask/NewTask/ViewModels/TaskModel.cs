using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Foundation.Collections;

namespace NewTask.ViewModels
{
    public class TaskModel : INotifyPropertyChanged
    {
        // save file name
        private const string JSONFILENAME = "taskData.json";

        private ObservableCollection<TaskItem> _taskList;
        public ObservableCollection<TaskItem> taskList
        {
            get { return _taskList; }
            set
            {
                NotifyPropertyChanging("taskList");
                _taskList = value;
                NotifyPropertyChanged("taskList");
            }
        }


        public TaskModel()
        {
            _taskList = new ObservableCollection<TaskItem>();
            taskList = new ObservableCollection<TaskItem>();
        }

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanged Members
        // notifies that a property has been changed
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        /// <summary>
        /// Add the task item to the task list and save the list to storage
        /// </summary>
        /// <param name="newItem"></param>
        public async void AddTaskItem(TaskItem newItem)
        {
            if(_taskList == null)
            {
                _taskList = new ObservableCollection<TaskItem>();
                taskList = new ObservableCollection<TaskItem>();
            }

            _taskList.Add(newItem);
            
            // save the data
            await writeData();
        }

        /// <summary>
        /// Delete the task and save the changes
        /// </summary>
        /// <param name="taskToDelete"></param>
        public async void DeleteTaskItem(TaskItem taskToDelete)
        {
            _taskList.Remove(taskToDelete);

            // save the data
            await writeData();
        }


        /// <summary>
        /// Write changes to the json file
        /// </summary>
        public async Task writeData()
        {
            try
            {
                // using a json serializer
                var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<TaskItem>));

                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                                                                                        JSONFILENAME,
                                                                                        CreationCollisionOption.ReplaceExisting))
                {
                    serializer.WriteObject(stream, taskList);
                }
            }
            catch(Exception e)
            {
                string message = e.Message;
            }

            try
            {
                await loadData();
            }
            catch (Exception e)
            {
                string message2 = e.Message;
            }
            
        }

        /// <summary>
        /// Load from the json file
        /// </summary>
        /// <returns></returns>
        public async Task loadData()
        {
            string content = String.Empty;

            try
            {
                var myStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(JSONFILENAME);
                using (StreamReader reader = new StreamReader(myStream))
                {
                    //content = await reader.ReadToEndAsync();

                    // replace the list with the info that was read
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<TaskItem>));

                    taskList = (ObservableCollection<TaskItem>)serializer.ReadObject(myStream);
                }
            }
            catch (Exception e)
            {
                string message = e.Message;

                // create the file...
            }

            
        }


    }
}
