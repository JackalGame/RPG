using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Vector2 scrollPosition;
        Vector2 canvasSize;
        Dialogue selectedDialogue = null;

        const float BACKGROUND_SIZE = 50f;
        
        [NonSerialized]
        GUIStyle nodeStyle = null;

        [NonSerialized]
        DialogueNode draggingNode = null;

        [NonSerialized]
        Vector2 draggingOffset;

        [NonSerialized]
        DialogueNode creatingNode = null;        
        
        [NonSerialized]
        DialogueNode deletingNode = null;

        [NonSerialized]
        DialogueNode linkingParentNode = null;

        [NonSerialized]
        bool draggingCanvas = false;

        [NonSerialized]
        Vector2 draggingCanvasOffset;

        
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

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if(newDialogue != null)
            {
                selectedDialogue = newDialogue;
                canvasSize = selectedDialogue.getCanvasSize();
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
                ProcessEvents();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                Rect canvasRect = GUILayoutUtility.GetRect(canvasSize.x, canvasSize.y);
                Texture2D backgroundTex = Resources.Load("background") as Texture2D;
                Rect texCoords = new Rect(0, 0, canvasSize.x / BACKGROUND_SIZE, canvasSize.y / BACKGROUND_SIZE);
                GUI.DrawTextureWithTexCoords(canvasRect, backgroundTex, texCoords);

                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }

                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }
                
                EditorGUILayout.EndScrollView();

                if (creatingNode != null)
                {
                    Undo.RecordObject(selectedDialogue, "Added Dialogue Node");
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }

                if(deletingNode != null)
                {
                    Undo.RecordObject(selectedDialogue, "Deleted Dialogue Node");
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            } 
        }

        private void ProcessEvents()
        {
            Event e = Event.current;

            //Drag Node using left mouse button

            if(e.type == EventType.MouseDown && draggingNode == null && e.button == 0)
            {
                draggingNode = GetNodeAtPoint(e.mousePosition + scrollPosition);
                if(draggingNode != null)
                {
                    draggingOffset = draggingNode.rect.position - e.mousePosition;
                }
            }
            else if (e.type == EventType.MouseDrag && draggingNode != null && e.button == 0)
            {
                Undo.RecordObject(selectedDialogue, "Move Dialogue Node");
                draggingNode.rect.position = e.mousePosition + draggingOffset;
                GUI.changed = true;
            }
            else if(e.type == EventType.MouseUp && draggingNode != null && e.button == 0)
            {
                draggingNode = null;
            }


            //Drag Canvas using middle mouse button

            if (e.type == EventType.MouseDown && e.button == 2)
            {
                draggingCanvas = true;
                draggingCanvasOffset = e.mousePosition + scrollPosition;
            }
            else if (e.type == EventType.MouseDrag && draggingCanvas && e.button == 2)
            {
                Undo.RecordObject(selectedDialogue, "Move Dialogue Node");
                scrollPosition = draggingCanvasOffset - e.mousePosition;
                GUI.changed = true;
            }
            else if (e.type == EventType.MouseUp && draggingCanvas && e.button == 2)
            {
                draggingCanvas = false;
            }

        }

        private void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.rect, nodeStyle);
            EditorGUI.BeginChangeCheck();

            string newText = EditorGUILayout.TextField(node.sentence);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
                node.sentence = newText;
            }

            GUILayout.BeginHorizontal();

            DrawButtons(node);

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawButtons(DialogueNode node)
        {
            //Styles
                        var styleRed = new GUIStyle(GUI.skin.button);
            styleRed.hover.textColor = Color.red;

            var styleGreen = new GUIStyle(GUI.skin.button);
            styleGreen.hover.textColor = Color.green;

            var styleBlue = new GUIStyle(GUI.skin.button);
            styleBlue.hover.textColor = Color.blue;

            //Cancel Button
            if (GUILayout.Button("x", styleRed))
            {
                deletingNode = node;
            }

            //Link Buttons
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("link", styleBlue))
                {
                    linkingParentNode = node;
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("cancel", styleBlue))
                {
                    linkingParentNode = null;
                }
            }
            else if (linkingParentNode.children.Contains(node.uniqueID))
            {
                if (GUILayout.Button("unlink", styleBlue))
                {
                    Undo.RecordObject(selectedDialogue, "Removed Dialogue Link");
                    linkingParentNode.children.Remove(node.uniqueID);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("child", styleBlue))
                {
                    Undo.RecordObject(selectedDialogue, "Add Dialogue Link");
                    linkingParentNode.children.Add(node.uniqueID);
                    linkingParentNode = null;
                }
            }

            //Add Button
            if (GUILayout.Button("+", styleGreen))
            {
                creatingNode = node;
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.rect.xMax, node.rect.center.y);

            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.rect.xMin, childNode.rect.center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;

                Handles.DrawBezier(
                    startPosition, endPosition, 
                    startPosition + controlPointOffset, endPosition - controlPointOffset, 
                    Color.white, null, 5f);
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.rect.Contains(point))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }
    }
}
