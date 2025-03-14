using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{

[CustomPropertyDrawer(typeof(Sequence))]
public class SequenceDrawer : PropertyDrawer
{
    float _height = EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
    float _buttonWidth = 38;

    Rect _backgroundRectExtra = new Rect(19, -3, 33, 2);

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if(!property.FindPropertyRelative("IsUnfolded").boolValue)
        {
            property.FindPropertyRelative("PropertyRectHeight").floatValue = _height;
            return _height;
        }


        //
        SequenceType sequenceType = (SequenceType)property.FindPropertyRelative("SequenceType").enumValueIndex;
        SequenceObjectType targetType = (SequenceObjectType)property.FindPropertyRelative("TargetType").enumValueIndex;
        if(sequenceType == SequenceType.Animation)
        {
            SequenceObjectType objectType = (SequenceObjectType)property.FindPropertyRelative("TargetType").enumValueIndex;
            float totalHeight = 0;

            if(objectType == SequenceObjectType.UnityEventDynamic)
            {
                totalHeight = _height*5 + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("EventDynamic")) + EditorGUIUtility.standardVerticalSpacing;
                
                property.FindPropertyRelative("PropertyRectHeight").floatValue = totalHeight;
                return totalHeight;
            }

            if(property.FindPropertyRelative("TargetComp").GetSerializedValue<Component>() == null)return _height*6;
            
            if(objectType == SequenceObjectType.RectTransform)
            {
                SequenceRectTransformTask rtTask = (SequenceRectTransformTask)property.FindPropertyRelative("TargetRtTask").enumValueFlag;
                if(rtTask.HasFlag(SequenceRectTransformTask.AnchoredPosition))totalHeight+=_height*3;
                if(rtTask.HasFlag(SequenceRectTransformTask.LocalScale))totalHeight+=_height*3;
                if(rtTask.HasFlag(SequenceRectTransformTask.LocalEulerAngles))totalHeight+=_height*3;
                if(rtTask.HasFlag(SequenceRectTransformTask.SizeDelta))totalHeight+=_height*3;
                if(rtTask.HasFlag(SequenceRectTransformTask.AnchorMax))totalHeight+=_height*3;
                if(rtTask.HasFlag(SequenceRectTransformTask.AnchorMin))totalHeight+=_height*3;
                if(rtTask.HasFlag(SequenceRectTransformTask.Pivot))totalHeight+=_height*3;
            }
            else if(objectType == SequenceObjectType.Transform)
            {
                SequenceTransformTask transTask = (SequenceTransformTask)property.FindPropertyRelative("TargetTransTask").enumValueFlag;
                if(transTask.HasFlag(SequenceTransformTask.LocalPosition))totalHeight+=_height*3;
                if(transTask.HasFlag(SequenceTransformTask.LocalScale))totalHeight+=_height*3;
                if(transTask.HasFlag(SequenceTransformTask.LocalEulerAngles))totalHeight+=_height*3;
            }
            else if(objectType == SequenceObjectType.Image)
            {
                SequenceImageTask imgTask = (SequenceImageTask)property.FindPropertyRelative("TargetImgTask").enumValueFlag;
                if(imgTask.HasFlag(SequenceImageTask.Color))totalHeight+=_height*3;
                if(imgTask.HasFlag(SequenceImageTask.FillAmount))totalHeight+=_height*3;
            }
            else if(objectType == SequenceObjectType.CanvasGroup)
            {
                Sequence.CgTask cgTask = (Sequence.CgTask)property.FindPropertyRelative("TargetCgTask").enumValueFlag;
                if(cgTask.HasFlag(Sequence.CgTask.Alpha))totalHeight+=_height*3;
            }
            else if(objectType == SequenceObjectType.Camera)
            {
                SequenceCameraTask camTask = (SequenceCameraTask)property.FindPropertyRelative("TargetCamTask").enumValueFlag;
                if(camTask.HasFlag(SequenceCameraTask.BackgroundColor))totalHeight+=_height*3;
                if(camTask.HasFlag(SequenceCameraTask.OrthographicSize))totalHeight+=_height*3;
            }
            else if(objectType == SequenceObjectType.TextMeshPro)
            {
                SequenceTextMeshProTask textMeshProTask = (SequenceTextMeshProTask)property.FindPropertyRelative("TargetTextMeshProTask").enumValueFlag;
                if(textMeshProTask.HasFlag(SequenceTextMeshProTask.Color))totalHeight+=_height*3;
                if(textMeshProTask.HasFlag(SequenceTextMeshProTask.MaxVisibleCharacters))totalHeight+=_height*3;
            }




            property.FindPropertyRelative("PropertyRectHeight").floatValue = _height*6 + totalHeight;
            return _height*6 + totalHeight +1;
        }
#region others
        else if(sequenceType == SequenceType.Wait)
        {
            property.FindPropertyRelative("PropertyRectHeight").floatValue = _height * 3;
            return _height * 3;
        }
        else if(sequenceType == SequenceType.SetActiveAllInput)
        {
            property.FindPropertyRelative("PropertyRectHeight").floatValue = _height * 3;
            return _height * 3;
        }
        else if(sequenceType == SequenceType.SetActive)
        {
            property.FindPropertyRelative("PropertyRectHeight").floatValue = _height * 4;
            return _height * 4;
        }
        else if(sequenceType == SequenceType.SFX)
        {
            property.FindPropertyRelative("PropertyRectHeight").floatValue = _height * 4;
            return _height * 4;
        }
        else if(sequenceType == SequenceType.LoadScene)
        {
            property.FindPropertyRelative("PropertyRectHeight").floatValue = _height * 3;
            return _height * 3;
        }
        else if(sequenceType == SequenceType.UnityEvent)
        {
            property.FindPropertyRelative("PropertyRectHeight").floatValue = _height + (EditorGUI.GetPropertyHeight(property.FindPropertyRelative("Event"))+EditorGUIUtility.singleLineHeight);
            return _height + (EditorGUI.GetPropertyHeight(property.FindPropertyRelative("Event"))+EditorGUIUtility.singleLineHeight);
        }

        property.FindPropertyRelative("PropertyRectHeight").floatValue = _height * 7;
        return _height * 7; //Not gonna happen

#endregion others
    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.FindPropertyRelative("PropertyRectY").floatValue = position.y;

        Rect nextPosition = new Rect(position.x, position.y, position.width, _height);
        
        Rect backgroundRect = new Rect(_backgroundRectExtra.x, position.y+_backgroundRectExtra.y, position.width+_backgroundRectExtra.width, GetPropertyHeight(property, label)+_backgroundRectExtra.height);
        
        SequenceType sequenceType = (SequenceType)property.FindPropertyRelative("SequenceType").enumValueIndex;
        
#region label
        if(sequenceType == SequenceType.Animation)
        {
            EditorGUI.DrawRect(backgroundRect, new Color(1, 0, 0, 0.1f));
            // Special labeling case for UnityEvent
            if((SequenceObjectType)property.FindPropertyRelative("TargetType").enumValueIndex == SequenceObjectType.UnityEventDynamic)
            {
                float time = property.FindPropertyRelative("StartTime").floatValue;
                
                property.FindPropertyRelative("IsUnfolded").boolValue //Fix for when the label is wrong
                    = EditorGUI.Foldout(nextPosition, property.FindPropertyRelative("IsUnfolded").boolValue, 
                    "At "+time.ToString()+"s [UnityEventDynamic]");
            }
        }
        else if(sequenceType == SequenceType.Wait)
            EditorGUI.DrawRect(backgroundRect, new Color(0, 0, 1, 0.1f));
        else if(sequenceType == SequenceType.SetActive)
            EditorGUI.DrawRect(backgroundRect, new Color(0, 1, 0, 0.1f));
        else if(sequenceType == SequenceType.SetActiveAllInput)
            EditorGUI.DrawRect(backgroundRect, new Color(1, 0, 1, 0.1f));
        else if(sequenceType == SequenceType.SFX)
            EditorGUI.DrawRect(backgroundRect, new Color(1, 1, 0, 0.1f));
        else if(sequenceType == SequenceType.LoadScene)
            EditorGUI.DrawRect(backgroundRect, new Color(0.6f, 0.3f, 0f, 0.1f));
        else if(sequenceType == SequenceType.UnityEvent)
        {
            float time = property.FindPropertyRelative("StartTime").floatValue;
            EditorGUI.DrawRect(backgroundRect, new Color(0, 1, 1, 0.1f));
            property.FindPropertyRelative("IsUnfolded").boolValue //Fix for when the label is wrong
                = EditorGUI.Foldout(nextPosition, property.FindPropertyRelative("IsUnfolded").boolValue, "At "+time.ToString()+"s [UnityEvent]");
        }
        if(sequenceType != SequenceType.UnityEvent && ((SequenceObjectType)property.FindPropertyRelative("TargetType").enumValueIndex != SequenceObjectType.UnityEventDynamic))
        {
            property.FindPropertyRelative("IsUnfolded").boolValue
                = EditorGUI.Foldout(nextPosition, property.FindPropertyRelative("IsUnfolded").boolValue, label);
        }
        
#endregion label
        #region preview button
        if(sequenceType != SequenceType.LoadScene)
        if(GUI.Button(new Rect(position.x+position.width-_buttonWidth*2, position.y-3, _buttonWidth, _height), "Start"))
        {
            property.FindPropertyRelative("TriggerStart").boolValue = true;
        }
        else if(GUI.Button(new Rect(position.x+position.width-_buttonWidth, position.y-3, _buttonWidth, _height), "End"))
        {
            property.FindPropertyRelative("TriggerEnd").boolValue = true;
        }
#endregion preview button

        if(!property.FindPropertyRelative("IsUnfolded").boolValue)return;


        // Type
        nextPosition.y += _height;
        EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("SequenceType"), new GUIContent("Type"));
        //
        if(sequenceType == SequenceType.Animation)
        {
#region setup animation
            nextPosition.y += _height;
            EditorGUI.PropertyField(new Rect(nextPosition.x, nextPosition.y, nextPosition.width*(0.61f), nextPosition.height), 
                property.FindPropertyRelative("EaseType"), new GUIContent("Ease")
            );

            EditorGUI.PropertyField(new Rect(nextPosition.x+nextPosition.width*(0.61f), nextPosition.y, nextPosition.width*(0.39f), nextPosition.height), 
                property.FindPropertyRelative("EasePower"), GUIContent.none
            );

            nextPosition.y += _height;
            EditorGUI.PropertyField(new Rect(nextPosition.x, nextPosition.y, nextPosition.width, nextPosition.height), 
                property.FindPropertyRelative("Duration")
            );
            // EditorGUI.LabelField(new Rect(nextPosition.x+nextPosition.width-10, nextPosition.y, 10, nextPosition.height),
            //     "s"
            // );

            nextPosition.y += _height;
            EditorGUI.PropertyField(new Rect(nextPosition.x, nextPosition.y, nextPosition.width*(0.61f), nextPosition.height), 
                property.FindPropertyRelative("TargetType"), new GUIContent("Target")
            ); 

            SequenceObjectType objectType = (SequenceObjectType)property.FindPropertyRelative("TargetType").enumValueIndex;
            if(objectType == SequenceObjectType.UnityEventDynamic)
            {
                nextPosition.y += _height;
                EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("EventDynamic"));
                return;
            }
            
            EditorGUI.PropertyField(new Rect(nextPosition.x+nextPosition.width*(0.61f), nextPosition.y, nextPosition.width*(0.39f), nextPosition.height), 
                property.FindPropertyRelative("TargetComp"), GUIContent.none
            );
#endregion setup animation
            
            
            //objectType, sequenceType
            nextPosition.y += _height;
            GUIContent startContent = new GUIContent("Start");
            GUIContent endContent = new GUIContent("End");
            

            if(property.FindPropertyRelative("TargetComp").GetSerializedValue<Component>() == null)return;
            
            if(objectType == SequenceObjectType.RectTransform)
            {
                EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("TargetRtTask"), new GUIContent("Task"));
                void DrawRtTask(string name)
                {
                    nextPosition.y += _height;
                    EditorGUI.LabelField(new Rect(nextPosition.x, nextPosition.y, nextPosition.width, _height),
                        new GUIContent(name)
                    );
                
                    nextPosition.y += _height;
                    if(GUI.Button(new Rect(nextPosition.x, nextPosition.y, nextPosition.width/4-5, _height),"Set Start"))
                    {
                        if(name == "AnchoredPosition")property.FindPropertyRelative(name+"Start").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().anchoredPosition;
                        else if(name == "LocalScale")property.FindPropertyRelative(name+"Start").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().localScale;
                        else if(name == "LocalEulerAngles")property.FindPropertyRelative(name+"Start").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().localEulerAngles;
                        else if(name == "SizeDelta")property.FindPropertyRelative(name+"Start").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().sizeDelta;
                        else if(name == "AnchorMin")property.FindPropertyRelative(name+"Start").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().anchorMin;
                        else if(name == "AnchorMax")property.FindPropertyRelative(name+"Start").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().anchorMax;
                        else if(name == "Pivot")property.FindPropertyRelative(name+"Start").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().pivot;
                    }
                    EditorGUI.PropertyField(
                        new Rect(nextPosition.x+nextPosition.width/4, nextPosition.y, nextPosition.width*3/4, _height),
                        property.FindPropertyRelative(name+"Start"), GUIContent.none
                    );

                    nextPosition.y += _height;
                    if(GUI.Button(new Rect(nextPosition.x, nextPosition.y, nextPosition.width/4-5, _height),"Set End"))
                    {
                        if(name == "AnchoredPosition")property.FindPropertyRelative(name+"End").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().anchoredPosition;
                        else if(name == "LocalScale")property.FindPropertyRelative(name+"End").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().localScale;
                        else if(name == "LocalEulerAngles")property.FindPropertyRelative(name+"End").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().localEulerAngles;
                        else if(name == "SizeDelta")property.FindPropertyRelative(name+"End").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().sizeDelta;
                        else if(name == "AnchorMin")property.FindPropertyRelative(name+"End").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().anchorMin;
                        else if(name == "AnchorMax")property.FindPropertyRelative(name+"End").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().anchorMax;
                        else if(name == "Pivot")property.FindPropertyRelative(name+"End").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().pivot;
                    }
                    EditorGUI.PropertyField(
                        new Rect(nextPosition.x+nextPosition.width/4, nextPosition.y, nextPosition.width*3/4, _height),
                        property.FindPropertyRelative(name+"End"), GUIContent.none
                    );
                }
                SequenceRectTransformTask rtTask = (SequenceRectTransformTask)property.FindPropertyRelative("TargetRtTask").enumValueFlag;
                if(rtTask.HasFlag(SequenceRectTransformTask.AnchoredPosition))DrawRtTask("AnchoredPosition");
                if(rtTask.HasFlag(SequenceRectTransformTask.LocalScale))DrawRtTask("LocalScale");
                if(rtTask.HasFlag(SequenceRectTransformTask.LocalEulerAngles))DrawRtTask("LocalEulerAngles");
                if(rtTask.HasFlag(SequenceRectTransformTask.SizeDelta))DrawRtTask("SizeDelta");
                if(rtTask.HasFlag(SequenceRectTransformTask.AnchorMin))DrawRtTask("AnchorMin");
                if(rtTask.HasFlag(SequenceRectTransformTask.AnchorMax))DrawRtTask("AnchorMax");
                if(rtTask.HasFlag(SequenceRectTransformTask.Pivot))DrawRtTask("Pivot");
            }

            else if(objectType == SequenceObjectType.Transform)
            {
                EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("TargetTransTask"), new GUIContent("Task"));
                void DrawTransTask(string name)
                {
                    nextPosition.y += _height;
                    EditorGUI.LabelField(new Rect(nextPosition.x, nextPosition.y, nextPosition.width, _height),
                        new GUIContent(name)
                    );
                
                    nextPosition.y += _height;
                    if(GUI.Button(new Rect(nextPosition.x, nextPosition.y, nextPosition.width/4-5, _height),"Set Start"))
                    {
                        if(name == "LocalPosition")property.FindPropertyRelative(name+"Start").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().localPosition;
                        else if(name == "LocalScale")property.FindPropertyRelative(name+"Start").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().localScale;
                        else if(name == "LocalEulerAngles")property.FindPropertyRelative(name+"Start").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().localEulerAngles;
                    }
                    EditorGUI.PropertyField(
                        new Rect(nextPosition.x+nextPosition.width/4, nextPosition.y, nextPosition.width*3/4, _height),
                        property.FindPropertyRelative(name+"Start"), GUIContent.none
                    );

                    nextPosition.y += _height;
                    if(GUI.Button(new Rect(nextPosition.x, nextPosition.y, nextPosition.width/4-5, _height),"Set End"))
                    {
                        if(name == "LocalPosition")property.FindPropertyRelative(name+"End").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().localPosition;
                        else if(name == "LocalScale")property.FindPropertyRelative(name+"End").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().localScale;
                        else if(name == "LocalEulerAngles")property.FindPropertyRelative(name+"End").vector3Value = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().localEulerAngles;
                    }
                    EditorGUI.PropertyField(
                        new Rect(nextPosition.x+nextPosition.width/4, nextPosition.y, nextPosition.width*3/4, _height),
                        property.FindPropertyRelative(name+"End"), GUIContent.none
                    );
                }
                SequenceTransformTask transTask = (SequenceTransformTask)property.FindPropertyRelative("TargetTransTask").enumValueFlag;
                if(transTask.HasFlag(SequenceTransformTask.LocalPosition))DrawTransTask("LocalPosition");
                if(transTask.HasFlag(SequenceTransformTask.LocalScale))DrawTransTask("LocalScale");
                if(transTask.HasFlag(SequenceTransformTask.LocalEulerAngles))DrawTransTask("LocalEulerAngles");
            }

            else if(objectType == SequenceObjectType.Image)
            {
                EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("TargetImgTask"), new GUIContent("Task"));
                void DrawImgTask(string name)
                {
                    nextPosition.y += _height;
                    EditorGUI.LabelField(new Rect(nextPosition.x, nextPosition.y, nextPosition.width, _height),
                        new GUIContent(name)
                    );
                
                    nextPosition.y += _height;
                    if(GUI.Button(new Rect(nextPosition.x, nextPosition.y, nextPosition.width/4-5, _height),"Set Start"))
                    {
                        if(name == "Color")property.FindPropertyRelative(name+"Start").colorValue = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().GetComponent<Image>().color;
                        else if(name == "FillAmount")property.FindPropertyRelative(name+"Start").floatValue = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().GetComponent<Image>().fillAmount;
                    }
                    EditorGUI.PropertyField(
                        new Rect(nextPosition.x+nextPosition.width/4, nextPosition.y, nextPosition.width*3/4, _height),
                        property.FindPropertyRelative(name+"Start"), GUIContent.none
                    );

                    nextPosition.y += _height;
                    if(GUI.Button(new Rect(nextPosition.x, nextPosition.y, nextPosition.width/4-5, _height),"Set End"))
                    {
                        if(name == "Color")property.FindPropertyRelative(name+"End").colorValue = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().GetComponent<Image>().color;
                        else if(name == "FillAmount")property.FindPropertyRelative(name+"End").floatValue = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().GetComponent<Image>().fillAmount;
                    }
                    EditorGUI.PropertyField(
                        new Rect(nextPosition.x+nextPosition.width/4, nextPosition.y, nextPosition.width*3/4, _height),
                        property.FindPropertyRelative(name+"End"), GUIContent.none
                    );
                }
                SequenceImageTask imgTask = (SequenceImageTask)property.FindPropertyRelative("TargetImgTask").enumValueFlag;
                if(imgTask.HasFlag(SequenceImageTask.Color))DrawImgTask("Color");
                if(imgTask.HasFlag(SequenceImageTask.FillAmount))DrawImgTask("FillAmount");
            }

            else if(objectType == SequenceObjectType.CanvasGroup)
            {
                EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("TargetCgTask"), new GUIContent("Task"));
                void DrawCgTask(string name)
                {
                    nextPosition.y += _height;
                    EditorGUI.LabelField(new Rect(nextPosition.x, nextPosition.y, nextPosition.width, _height),
                        new GUIContent(name)
                    );
                
                    nextPosition.y += _height;
                    if(GUI.Button(new Rect(nextPosition.x, nextPosition.y, nextPosition.width/4-5, _height),"Set Start"))
                    {
                        if(name == "Alpha")property.FindPropertyRelative(name+"Start").floatValue = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().GetComponent<CanvasGroup>().alpha;
                    }
                    EditorGUI.PropertyField(
                        new Rect(nextPosition.x+nextPosition.width/4, nextPosition.y, nextPosition.width*3/4, _height),
                        property.FindPropertyRelative(name+"Start"), GUIContent.none
                    );

                    nextPosition.y += _height;
                    if(GUI.Button(new Rect(nextPosition.x, nextPosition.y, nextPosition.width/4-5, _height),"Set End"))
                    {
                        if(name == "Alpha")property.FindPropertyRelative(name+"End").floatValue = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<RectTransform>().GetComponent<CanvasGroup>().alpha;
                    }
                    EditorGUI.PropertyField(
                        new Rect(nextPosition.x+nextPosition.width/4, nextPosition.y, nextPosition.width*3/4, _height),
                        property.FindPropertyRelative(name+"End"), GUIContent.none
                    );
                }
                Sequence.CgTask cgTask = (Sequence.CgTask)property.FindPropertyRelative("TargetCgTask").enumValueFlag;
                if(cgTask.HasFlag(Sequence.CgTask.Alpha))DrawCgTask("Alpha");
            }

            else if(objectType == SequenceObjectType.Camera)
            {
                EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("TargetCamTask"), new GUIContent("Task"));
                void DrawCamTask(string name)
                {
                    nextPosition.y += _height;
                    EditorGUI.LabelField(new Rect(nextPosition.x, nextPosition.y, nextPosition.width, _height),
                        new GUIContent(name)
                    );
                
                    nextPosition.y += _height;
                    if(GUI.Button(new Rect(nextPosition.x, nextPosition.y, nextPosition.width/4-5, _height),"Set Start"))
                    {
                        if(name == "BackgroundColor")property.FindPropertyRelative(name+"Start").colorValue = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().GetComponent<Camera>().backgroundColor;
                        else if(name == "OrthographicSize")property.FindPropertyRelative(name+"Start").floatValue = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().GetComponent<Camera>().orthographicSize;
                    }
                    EditorGUI.PropertyField(
                        new Rect(nextPosition.x+nextPosition.width/4, nextPosition.y, nextPosition.width*3/4, _height),
                        property.FindPropertyRelative(name+"Start"), GUIContent.none
                    );

                    nextPosition.y += _height;
                    if(GUI.Button(new Rect(nextPosition.x, nextPosition.y, nextPosition.width/4-5, _height),"Set End"))
                    {
                        if(name == "BackgroundColor")property.FindPropertyRelative(name+"End").colorValue = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().GetComponent<Camera>().backgroundColor;
                        else if(name == "OrthographicSize")property.FindPropertyRelative(name+"End").floatValue = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().GetComponent<Camera>().orthographicSize;
                    }
                    EditorGUI.PropertyField(
                        new Rect(nextPosition.x+nextPosition.width/4, nextPosition.y, nextPosition.width*3/4, _height),
                        property.FindPropertyRelative(name+"End"), GUIContent.none
                    );
                }
                SequenceCameraTask camTask = (SequenceCameraTask)property.FindPropertyRelative("TargetCamTask").enumValueFlag;
                if(camTask.HasFlag(SequenceCameraTask.BackgroundColor))DrawCamTask("BackgroundColor");
                if(camTask.HasFlag(SequenceCameraTask.OrthographicSize))DrawCamTask("OrthographicSize");
            }    
            else if(objectType == SequenceObjectType.TextMeshPro)
            {
                EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("TargetTextMeshProTask"), new GUIContent("Task"));
                void DrawTextMeshProTask(string name)
                {
                    nextPosition.y += _height;
                    EditorGUI.LabelField(new Rect(nextPosition.x, nextPosition.y, nextPosition.width, _height),
                        new GUIContent(name)
                    );
                
                    nextPosition.y += _height;
                    if(GUI.Button(new Rect(nextPosition.x, nextPosition.y, nextPosition.width/4-5, _height),"Set Start"))
                    {
                        if(name == "TextMeshProColor")property.FindPropertyRelative(name+"Start").colorValue = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().GetComponent<TMP_Text>().color;
                        else if(name == "MaxVisibleCharacters")
                        {
                            int maxVisibleCharactersStart = property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().GetComponent<TMP_Text>().maxVisibleCharacters;
                            int maxCharacters = property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().GetComponent<TMP_Text>().text.Length;
                            property.FindPropertyRelative(name+"Start").intValue = Mathf.Clamp(maxVisibleCharactersStart, 0, maxCharacters+1);
                        }
                    }
                    EditorGUI.PropertyField(
                        new Rect(nextPosition.x+nextPosition.width/4, nextPosition.y, nextPosition.width*3/4, _height),
                        property.FindPropertyRelative(name+"Start"), GUIContent.none
                    );

                    nextPosition.y += _height;
                    if(GUI.Button(new Rect(nextPosition.x, nextPosition.y, nextPosition.width/4-5, _height),"Set End"))
                    {
                        if(name == "TextMeshProColor")property.FindPropertyRelative(name+"End").colorValue = 
                            property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().GetComponent<TMP_Text>().color;
                        else if(name == "MaxVisibleCharacters")
                        {
                            int maxVisibleCharactersEnd = property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().GetComponent<TMP_Text>().maxVisibleCharacters;
                            int maxCharacters = property.FindPropertyRelative("TargetComp").GetSerializedValue<Transform>().GetComponent<TMP_Text>().text.Length;
                            property.FindPropertyRelative(name+"End").intValue = Mathf.Clamp(maxVisibleCharactersEnd, 0, maxCharacters+1);
                        }
                    }
                    EditorGUI.PropertyField(
                        new Rect(nextPosition.x+nextPosition.width/4, nextPosition.y, nextPosition.width*3/4, _height),
                        property.FindPropertyRelative(name+"End"), GUIContent.none
                    );
                }
                SequenceTextMeshProTask textMeshProTask = (SequenceTextMeshProTask)property.FindPropertyRelative("TargetTextMeshProTask").enumValueFlag;
                if(textMeshProTask.HasFlag(SequenceTextMeshProTask.Color))DrawTextMeshProTask("TextMeshProColor");
                if(textMeshProTask.HasFlag(SequenceTextMeshProTask.MaxVisibleCharacters))DrawTextMeshProTask("MaxVisibleCharacters");
            }        

        }
#region others
        else if(sequenceType == SequenceType.Wait)
        {
            nextPosition.y += _height;
            EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("Duration"));
        }
        else if(sequenceType == SequenceType.SetActiveAllInput)
        {
            nextPosition.y += _height;
            EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("IsActivating"));
        }
        else if(sequenceType == SequenceType.SetActive)
        {
            nextPosition.y += _height;
            EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("Target"), new GUIContent("GameObject"));
            nextPosition.y += _height;
            EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("IsActivating"));
        }
        else if(sequenceType == SequenceType.SFX)
        {
            nextPosition.y += _height;
            EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("PlaySFXBy"));
            
            nextPosition.y += _height;
            if((SequenceSFXMethod)property.FindPropertyRelative("PlaySFXBy").enumValueIndex == SequenceSFXMethod.File)
                EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("SFXFile"));
            else
                EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("SFXIndex"));
        }
        else if(sequenceType == SequenceType.LoadScene)
        {
            nextPosition.y += _height;
            EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("SceneToLoad"));
        }
        else if(sequenceType == SequenceType.UnityEvent)
        {
            nextPosition.y += _height;
            EditorGUI.PropertyField(nextPosition, property.FindPropertyRelative("Event"));
        }
#endregion others
    }
}

}