using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    public class QuitTask
    {
        public Stack<Action> QuitTasks = new Stack<Action>();

        public void AddTask(Action action)
        {
            QuitTasks.Push(action);
        }

        public void ClearTask()
        {
            QuitTasks.Clear();
        }

        public void BackButtonHandler()
        {
            if (QuitTasks.Count <= 0)
            {
                GameManager.Instance.QuitGame();
                return;
            }
            QuitTasks.Pop().Invoke();
        }
    }

    QuitTask quitTask = null;

    public QuitTask Quit
    {
        get
        {
            if (quitTask == null) quitTask = new QuitTask();
            return quitTask;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Quit.BackButtonHandler();
    }
}
