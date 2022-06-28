using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuHandler : MonoBehaviour
{
    private PlayerData _playerData;
    private InputField _inputField;

    private void Start()
    {
        _playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
        _inputField = GetComponentInChildren<InputField>();
        _inputField.onValueChanged.AddListener(_playerData.UpdatePlayerName);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
