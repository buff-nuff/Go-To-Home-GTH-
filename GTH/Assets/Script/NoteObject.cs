using UnityEngine;


public enum NoteType { TapNote, AvoidNote }
public class NoteObject : MonoBehaviour
{
    public NoteType type;
    public float speed = 500f;
    public float beatTime;
    public float targetx;
    public float moveSpeed;
    public bool isInitialized = false;

    public void Initialize(float startx, float targetx, float scrollTime)
    {
        this.targetx = targetx;
        moveSpeed = (startx - targetx) / scrollTime;
        isInitialized = true;
    }
    private void Update()
    {
        if (!isInitialized) return;
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        
    }

    public void SetType(NoteType newType)
    {
        type = newType;
        GetComponent<SpriteRenderer>().color = (type == NoteType.TapNote) ? Color.blue : Color.red;
        // <bool> ? <ture value> : <false value>
        // <bool> ? <ture value> :
        // <bool> ? <ture value> : <false value>
        // »ïÇ× ¿¬»ê
    }
}
