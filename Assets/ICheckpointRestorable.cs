using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckpointRestorable
{
    void RestoreToCheckpoint(int checkpointId);
}
