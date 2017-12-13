using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatCodeManager : MonoBehaviour
{
    public static CheatCodeManager instance;

    public Image consolePanel;
    public Text consoleText;

    string cheatCodeBuffer = "";

    const int MAX_CHEATCODE_BUFFER_LENGTH = 20;
    const string CHEAT_GOTO_FIRST_CHECKPOINT = "go2van";
    const string CHEAT_GOTO_SECOND_CHECKPOINT = "go2lavalamp";
    const string CHEAT_GOTO_THIRD_CHECKPOINT = "go2gamekid";
    const string CHEAT_GOTO_FOURTH_CHECKPOINT = "go2walkman";
    const string CHEAT_GOTO_LAST_CHECKPOINT = "go2boss";
    const string CHEAT_DEACTIVATE_FALL_DAMAGE = "longfallshoes";
    const string CHEAT_GODMODE = "2spooky4me";
    const string CHEAT_ALL_SECRETS = "gimmeallpls";

    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckForCheatCodes(char c)
    {
        cheatCodeBuffer += c;
        if (cheatCodeBuffer.Length > MAX_CHEATCODE_BUFFER_LENGTH)
        {
            cheatCodeBuffer = cheatCodeBuffer.Substring(1, MAX_CHEATCODE_BUFFER_LENGTH);
        }

        if (cheatCodeBuffer.Contains(CHEAT_GOTO_FIRST_CHECKPOINT))
        {
            GameManager.instance.currentCheckpoint = -1;
            player.KillIgnoreCheats(false);
            cheatCodeBuffer = "";
        }
        else if (cheatCodeBuffer.Contains(CHEAT_GOTO_SECOND_CHECKPOINT))
        {
            GameManager.instance.currentCheckpoint = 0;
            player.KillIgnoreCheats(false);
            cheatCodeBuffer = "";
        }
        else if (cheatCodeBuffer.Contains(CHEAT_GOTO_THIRD_CHECKPOINT))
        {
            GameManager.instance.currentCheckpoint = 1;
            player.KillIgnoreCheats(false);
            cheatCodeBuffer = "";
        }
        else if (cheatCodeBuffer.Contains(CHEAT_GOTO_FOURTH_CHECKPOINT))
        {
            GameManager.instance.currentCheckpoint = 2;
            player.KillIgnoreCheats(false);
            cheatCodeBuffer = "";
        }
        else if (cheatCodeBuffer.Contains(CHEAT_GOTO_LAST_CHECKPOINT))
        {
            GameManager.instance.lastArtifactCollected = true;
            GameManager.instance.bossFightReady = true;
            player.KillIgnoreCheats(false);
            cheatCodeBuffer = "";
        }
        else if (cheatCodeBuffer.Contains(CHEAT_DEACTIVATE_FALL_DAMAGE))
        {
            GameManager.instance.fallDamageEnabled = !GameManager.instance.fallDamageEnabled;
            UpdateConsole("FALL DAMAGE", GameManager.instance.fallDamageEnabled);
            cheatCodeBuffer = "";
        }
        else if (cheatCodeBuffer.Contains(CHEAT_GODMODE))
        {
            GameManager.instance.godModeEnabled = !GameManager.instance.godModeEnabled;
            UpdateConsole("GODMODE", GameManager.instance.godModeEnabled);
            cheatCodeBuffer = "";
        }
        else if (cheatCodeBuffer.Contains(CHEAT_ALL_SECRETS))
        {
            LesserArtifact[] artifacts = GameManager.instance.lesserArtifacts;
            for (int i = 0; i < artifacts.Length; i++)
            {
                artifacts[i].Collect();
                GameManager.instance.savedLesserArtifacts.Add(artifacts[i]);
            }

            UpdateConsole("ALL SECRETS UNLOCKED");
            cheatCodeBuffer = "";
        }
    }

    Coroutine hideConsoleCoroutine;

    void UpdateConsole(string cheatCode)
    {
        consoleText.text = cheatCode;
        consolePanel.gameObject.SetActive(true);

        if (hideConsoleCoroutine != null)
        {
            StopCoroutine(hideConsoleCoroutine);
        }
        hideConsoleCoroutine = StartCoroutine(HideConsole());
    }

    void UpdateConsole(string cheatCode, bool active)
    {
        string onOff = active ? "ON" : "OFF";
        UpdateConsole(cheatCode + ":" + onOff);
    }

    IEnumerator HideConsole()
    {
        yield return new WaitForSeconds(2f);
        consolePanel.gameObject.SetActive(false);
    }
}
