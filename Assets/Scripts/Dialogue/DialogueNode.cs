using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private string sentence;
        [SerializeField] private List<string> children = new List<string>();
        [SerializeField] private Rect rect = new Rect(0, 0, 200, 100);

        public string GetSentence()
        {
            return sentence;
        }

        public List<string> GetChildren()
        {
            return children;
        }
        
        public Rect GetRect()
        {
            return rect;
        }

#if UNITY_EDITOR
        public void SetSentence (string newSentence)
        {
            if(newSentence != sentence)
            {
                Undo.RecordObject(this, "Sentence Changed");
                sentence = newSentence;
                EditorUtility.SetDirty(this);
            }
        }        
        
        public void SetPosition(Vector2 newPos)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPos;
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }        
        
        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Removed Dialogue Link");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
