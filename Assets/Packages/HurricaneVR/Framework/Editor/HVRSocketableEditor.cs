using System;
using System.Collections.Generic;
using System.Linq;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.Sockets;
using HurricaneVR.Framework.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace HurricaneVR.Editor
{
    [CustomEditor(typeof(HVRSocketable), editorForChildClasses: true)]
    public class HVRSocketableEditor : UnityEditor.Editor
    {
        private bool _expand;
        private HVRSocket _socket;
        private HVRSocketable component;

        private Vector3 _pos;
        private Quaternion _rot;

        private SocketPose _socketPose;
        private Vector3 _originalPos;
        private Quaternion _originalRot;
        private bool movedToSocket;

        private static Dictionary<HVRSocketable, HVRSocket> _cache = new Dictionary<HVRSocketable, HVRSocket>();

        private void OnEnable()
        {
            component = target as HVRSocketable;
            if (_cache.TryGetValue(component, out var socket))
            {
                _socket = socket;
            }
        }

        private void OnDisable()
        {
            if (movedToSocket)
            {
                component.transform.position = _originalPos;
                component.transform.rotation = _originalRot;
            }
        }

        public override void OnInspectorGUI()
        {
            // _expand = EditorGUILayout.Foldout(_expand, "Posing Interface");
            EditorGUILayout.LabelField("Posing:");
            //if (_expand)
            {
                var temp = _socket;
                HVREditorExtensions.ObjectField("Socket", ref _socket);

                if (_socket)
                {
                    if (string.IsNullOrWhiteSpace(_socket.PoseTag))
                    {
                        EditorGUILayout.HelpBox("Socket's PoseTag field is not assigned.", MessageType.Warning);
                    }
                    else
                    {
                        if (_socket.ScaleGrabbable)
                        {
                            if (GUILayout.Button("Apply Socket Scale"))
                            {
                                Undo.RecordObject(component.transform, "Editor Socket Scale");
                                component.transform.localScale = _socket.ComputeScale(component);
                            }
                            
                            if (GUILayout.Button("Reset Scale"))
                            {
                                Undo.RecordObject(component.transform, "Editor Socket Scale");
                                component.transform.localScale = Vector3.one;
                            }
                        }
                        
                        if (GUILayout.Button("Move to Socket (Centered)"))
                        {
                            Undo.RecordObject(component.transform, "Move to Socket (Centered)");
                            _originalPos = component.transform.position;
                            _originalRot = component.transform.rotation;
                            component.transform.position = _socket.transform.position;
                            component.transform.rotation = _socket.transform.rotation;
                            movedToSocket = true;
                        }
                        
                        if (GUILayout.Button("Move to Socket (Posed)"))
                        {
                            Debug.LogError("Implement");
                        }

                        if (GUILayout.Button("Save Pose"))
                        {
                            var poses = serializedObject.FindProperty("Poses");
                            var i = component.Poses.FindIndex(e => e.SocketTag == _socket.PoseTag);
                            if (i < 0)
                            {
                                i = poses.arraySize;
                                poses.InsertArrayElementAtIndex(i);
                            }

                            var pose = poses.GetArrayElementAtIndex(i);
                            pose.FindPropertyRelative("Position").vector3Value = _socket.transform.InverseTransformPoint(component.transform.position);
                            pose.FindPropertyRelative("EulerAngles").vector3Value = (Quaternion.Inverse(_socket.transform.rotation) * component.transform.rotation).eulerAngles;
                            pose.FindPropertyRelative("SocketTag").stringValue = _socket.PoseTag;

                            serializedObject.ApplyModifiedProperties();
                        }

                        if (GUILayout.Button("Restore Position and Rotation"))
                        {
                            Debug.LogError("Implement");
                        }
                    }
                }
                else
                {
                    if(GUILayout.Button("Find Closest Socket"))
                    {
                        var dist = float.MaxValue;
                        HVRSocket closest = null;
                        foreach (var socket in FindObjectsOfType<HVRSocket>())
                        {
                            if (socket.transform.IsChildOf(component.transform))
                            {
                                continue;
                            }
                            
                            var d = Vector3.Distance(component.transform.position, socket.transform.position);
                            if (d < dist)
                            {
                                dist = d;
                                closest = socket;
                            }
                        }

                        _socket = closest;
                    }
                }
                
                if (temp && !_socket)
                {
                    _cache[component] = null;
                }
                else if (!temp && _socket)
                {
                    _cache[component] = _socket;
                }
            }

            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }
}