using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AnimatorController = UnityEditor.Animations.AnimatorController;
using AnimatorStateMachine = UnityEditor.Animations.AnimatorStateMachine;
using AnimatorState = UnityEditor.Animations.AnimatorState;
using AnimatorStateTransition = UnityEditor.Animations.AnimatorStateTransition;

public class animatorGenertaor : EditorWindow
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [MenuItem("Window/Animator Genertaor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(animatorGenertaor)); //显示该窗口
    }

    string SavePath;
    private void Generate()
    {
        if (string.IsNullOrEmpty(SavePath))
        {
            SavePath = "GenertedAnimator";
        }
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath("Assets/" + SavePath + ".controller");
        controller.AddLayer("Base");
        AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;
        //设置entry子节点
        AnimatorState lastState = stateMachine.AddState(AnimationClips[0].name);
        lastState.motion = AnimationClips[0];
        //设置后面的节点
        for (int i = 1; i < AnimationClips.Count; i++)
        {
            AnimatorState newState = stateMachine.AddState(AnimationClips[i].name);
            newState.motion = AnimationClips[i];
            AnimatorStateTransition transition = lastState.AddTransition(newState);
            transition.duration = 2;
            lastState = newState;
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private List<AnimationClip> AnimationClips = new List<AnimationClip>();
    bool bPickingAnimation = false;
    private void AddAnimationButton()
    {
        int controlID = EditorGUIUtility.GetControlID(FocusType.Passive);
        EditorGUIUtility.ShowObjectPicker<AnimationClip>(null, false, "", controlID); //显示资源选择框
        bPickingAnimation = true;
    }
    private void DoAddAnimation()
    {
        AnimationClip clip = EditorGUIUtility.GetObjectPickerObject() as AnimationClip;
        if (clip != null)
        {
            AnimationClips.Add(clip);
        }
        bPickingAnimation = false;
    }

    private void ShowAnimationNames()
    {
        GUILayout.Label("动画序列", EditorStyles.boldLabel);
        if (AnimationClips.Count > 0)
        {
            for (int i = 0; i < AnimationClips.Count; i++)
            {
                GUILayout.Label(AnimationClips[i].name, EditorStyles.label);
            }
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("添加动画"))
        {
            AddAnimationButton();
        }

        if (bPickingAnimation)//添加动画的资源选择事件循环
        {
            Event ObjectPickerEvent = Event.current;
            if (ObjectPickerEvent.commandName == "ObjectSelectorUpdated") //资源选择完成
            {
                DoAddAnimation();
            }
        }

        ShowAnimationNames();

        if (GUILayout.Button("删除最后一个"))
        {
            AnimationClips.RemoveAt(AnimationClips.Count - 1);
        }

        GUILayout.Label("生成文件名", EditorStyles.boldLabel);
        SavePath = GUILayout.TextField("");

        if (GUILayout.Button("生成"))
        {
            Generate();
        }
    }
}
