using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public bool gamePaused = false;

    public Boss boss;
    public Ghost[] ghosts;
    public Artifact[] artifacts;
    public LesserArtifact[] lesserArtifacts;

    public HashSet<LesserArtifact> savedLesserArtifacts = new HashSet<LesserArtifact>();
    public List<Artifact> collectedArtifacts = new List<Artifact>();

    public List<LesserArtifact> collectedLesserArtifacts = new List<LesserArtifact>();

    public int currentCheckpoint;

    public float fadeTime;

    public bool lastArtifactCollected = false;

    public bool bossFightReady = false;

    public static GameManager instance;

    public LastArtifact lastArtifact;

    public bool usingKeyboardMouse = true;

    [Header("Cheats")]
    public bool godModeEnabled = false;
    public bool fallDamageEnabled = true;

    [Header("UI")]
    public Image fadePanel;

    public Text collectedArtifactsText;
    public Text totalArtifactsText;

    public Image uiArtifact0;
    public Image uiArtifact1;
    public Image uiArtifact2;
    public Image uiArtifact3;

    public PauseCanvas pauseCanvas;

    int totalLesserArtifacts;


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

    public Artifact GetArtifact(int artifactId)
    {
        for (int i = 0; i < artifacts.Length; i++)
        {
            if (artifacts[i].artifactId == artifactId)
            {
                return artifacts[i];
            }
        }
        return null;
    }

    private void Start()
    {
        boss = FindObjectOfType<Boss>();
        ghosts = FindObjectsOfType<Ghost>();
        lesserArtifacts = FindObjectsOfType<LesserArtifact>();
        lastArtifact = FindObjectOfType<LastArtifact>();

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);
            ghosts[i].defaultPosition = ghosts[i].transform.position;
        }

        artifacts = FindObjectsOfType<Artifact>();

        Color c = fadePanel.color;
        fadePanel.color = new Color(c.r, c.g, c.b, 0f);

        totalLesserArtifacts = FindObjectsOfType<LesserArtifact>().Length;
        UpdateGUI();

        pauseCanvas.gameObject.SetActive(false);
    }

    public void FadeOutGame()
    {
        StartCoroutine(FadeOutGameCoroutine());
    }

    public void FadeInGame()
    {
        StartCoroutine(FadeInGameCoroutine());
    }

    public void ClearCollectedLesserArtifacts()
    {
        collectedArtifacts.Clear();
        UpdateGUI();
    }

    public void CollectLastArtifact()
    {
        lastArtifactCollected = true;
        UpdateGUI();
    }

    IEnumerator FadeOutGameCoroutine()
    {
        float alpha = fadePanel.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, 1f, t));
            fadePanel.color = newColor;
            yield return null;
        }
        fadePanel.color = new Color(0, 0, 0, 1f);
    }

    IEnumerator FadeInGameCoroutine()
    {
        float alpha = fadePanel.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, 0f, t));
            fadePanel.color = newColor;
            yield return null;
        }
        fadePanel.color = new Color(0, 0, 0, 0f);
    }

    public void Add(LesserArtifact artifact)
    {
        collectedLesserArtifacts.Add(artifact);
        UpdateGUI();
    }

    void UpdateGUI()
    {
        collectedArtifactsText.text = collectedLesserArtifacts.Count.ToString();
        totalArtifactsText.text = totalLesserArtifacts.ToString();

        for (int i = 0; i < artifacts.Length; i++)
        {
            Image uiArtifact =
            artifacts[i].artifactId == 0 ? uiArtifact0 :
            artifacts[i].artifactId == 1 ? uiArtifact1 :
            artifacts[i].artifactId == 2 ? uiArtifact2 :
            uiArtifact3;

            if (artifacts[i].collected)
            {
                uiArtifact.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                uiArtifact.color = new Color(0f, 0f, 0f, 1f);
            }
        }
        if (lastArtifactCollected)
        {
            uiArtifact3.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            uiArtifact3.color = new Color(0f, 0f, 0f, 1f);
        }

    }

    public void Remove(LesserArtifact artifact)
    {
        collectedLesserArtifacts.Remove(artifact);
        UpdateGUI();
    }

    public void Add(Artifact artifact)
    {
        collectedArtifacts.Add(artifact);
        SetCurrentCheckpoint(artifact.artifactId);
        SaveLesserArtifacts();
        UpdateGUI();
    }

    void SaveLesserArtifacts()
    {
        for (int i = 0; i < collectedLesserArtifacts.Count; i++)
        {
            savedLesserArtifacts.Add(collectedLesserArtifacts[i]);
        }
        UpdateGUI();
    }

    public void SwitchPauseState()
    {
        gamePaused = !gamePaused;
        if (gamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void SetCurrentCheckpoint(int checkpointId)
    {
        currentCheckpoint = checkpointId;
    }
}
