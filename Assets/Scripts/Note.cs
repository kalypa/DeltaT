using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : SingleMonobehaviour<Note>
{
    double timeInstantiated;
    public float assignedTime;
    public float endTime;
    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));


        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.forward * SongManager.Instance.noteSpawnY, Vector3.forward * SongManager.Instance.noteDespawnY, t);
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}