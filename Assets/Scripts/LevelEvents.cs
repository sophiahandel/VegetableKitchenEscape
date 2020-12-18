using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class LevelEvents : MonoBehaviour
{
    string path = "Assets/Scripts/GameData/GameData.json";

    public GameObject WallEventObject1;
    public GameObject WallEventObject2;

    public Text deathCountText;
    public Text timerText;

    private PlayerController pc;
    public GameObject[] checkpoints;

    [Serializable]
    public struct LevelState
    {
        public int deathCount;
        public float gameTime;
        public int currentCheckpoint;
        public bool WallEventActive;
    }

    [SerializeField]
    private LevelState state;
    

    private void Awake()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        state = JsonUtility.FromJson<LevelState>(this.Load());
        deathCountText.text = "Death Count: " + state.deathCount;
    }

    private void FixedUpdate()
    {
        float time = state.gameTime + Time.time;
        int minutes = (int) Math.Floor(time / 60);
        int seconds = (int) time % 60;
        timerText.text = "Time: " + minutes + ":" + seconds;
    }

    private void LateUpdate()
    {
        WallEventObject1.SetActive(!state.WallEventActive);
        WallEventObject2.SetActive(!state.WallEventActive);
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    this.nextCheckpoint();
        //    this.ReloadScene();
        //}
        //else if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    this.prevCheckpoint();
        //    this.ReloadScene();
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    this.ReloadScene();
        //}
    }

    private void OnApplicationQuit()
    {
        this.ResetLevelState();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(state);
        File.WriteAllText(path, json);
    }

    public string Load()
    {
        if (File.Exists(path))
        {
            string loadData = File.ReadAllText(path);
            return loadData;
        }
        else
        {
            return null;
        }
    }

    public void ReloadScene()
    {
        state.gameTime = Time.time;
        this.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetLevelState()
    {
        this.ChangeWallState(false);
        state.currentCheckpoint = 0;
        state.deathCount = 0;
        state.gameTime = -Time.time;
        this.Save();
    }

    public void nextCheckpoint()
    {
        if (state.currentCheckpoint < checkpoints.Length - 1)
        {
            state.currentCheckpoint++;
        }
    }

    public void prevCheckpoint()
    {
        if (state.currentCheckpoint > 0)
        {
            state.currentCheckpoint--;
        }
    }

    public void ChangeWallState(bool wallState)
    {
        state.WallEventActive = wallState;
    }

    public Vector3 getCheckpointPosition()
    {
        return checkpoints[state.currentCheckpoint].transform.position;
    }

    public void IncreaseDeathCount()
    {
        state.deathCount++;
        deathCountText.text = "Death Count: " + state.deathCount;
    }

    public LevelState getState()
    {
        return state;
    }
}
