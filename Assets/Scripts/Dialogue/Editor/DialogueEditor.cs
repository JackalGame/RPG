using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OpenDialogue(int instanceID, int line)
        {

            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if(dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if(newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if(selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected.");
            }
            else
            {
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.LabelField("Node: ");
                    string newUniqueID = EditorGUILayout.TextField(node.uniqueID);
                    string newText = EditorGUILayout.TextField(node.sentence);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
                        node.sentence = newText;
                        node.uniqueID = newUniqueID;
                    }
                }
            } 
        }
    }
}
