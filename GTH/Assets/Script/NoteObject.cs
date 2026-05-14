using UnityEngine;


public enum NoteType { TapNote, AvoidNote }
public class NoteObject : MonoBehaviour
{
    public NoteType type;
    public float speed = 500f;

    private void Update()
    {
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
