using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserArtifact : MonoBehaviour, IEquatable<LesserArtifact>
{
    public int lesserArtifactId;
    bool collected;

    public void Collect()
    {
        collected = true;
        GameManager.instance.Add(this);
        gameObject.SetActive(false);
    }

    public void Restore()
    {
        if (collected)
        {
            collected = false;
            GameManager.instance.Remove(this);
            gameObject.SetActive(true);
        }
    }

    public override bool Equals(object other)
    {
        return Equals((LesserArtifact)other);
    }

    public override int GetHashCode()
    {
        return lesserArtifactId.GetHashCode();
    }

    bool IEquatable<LesserArtifact>.Equals(LesserArtifact other)
    {
        return lesserArtifactId.Equals(other.lesserArtifactId);
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position, "Lesser artifact #" + lesserArtifactId);
#endif
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }
}
