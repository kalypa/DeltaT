using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : SingleMonobehaviour<Note>
{
    
    double timeInstantiated;
    public float assignedTime;
    public float endTime;
    public NoteType type;
    private LineRenderer lineRenderer;
    public Transform longNotePos;
    public enum NoteType
    { 
        Long,
        Normal,
    }

    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
        lineRenderer = GetComponent<LineRenderer>();
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
        if (this.type == NoteType.Long)
        {
            LongNoteTail();
        }
    }

    public void LongNoteTail()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, new Vector3(transform.parent.position.x, transform.parent.position.y, transform.position.z));
        lineRenderer.SetPosition(1, new Vector3(transform.parent.position.x, transform.parent.position.y, longNotePos.position.z));
    }
}