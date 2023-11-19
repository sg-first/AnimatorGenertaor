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
        EditorWindow.GetWindow(typeof(animatorGenertaor)); //��ʾ�ô���
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
        //����entry�ӽڵ�
        AnimatorState lastState = stateMachine.AddState(AnimationClips[0].name);
        lastState.motion = AnimationClips[0];
        //���ú���Ľڵ�
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
        EditorGUIUtility.ShowObjectPicker<AnimationClip>(null, false, "", controlID); //��ʾ��Դѡ���
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
        GUILayout.Label("��������", EditorStyles.boldLabel);
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
        if (GUILayout.Button("��Ӷ���"))
        {
            AddAnimationButton();
        }

        if (bPickingAnimation)//��Ӷ�������Դѡ���¼�ѭ��
        {
            Event ObjectPickerEvent = Event.current;
            if (ObjectPickerEvent.commandName == "ObjectSelectorUpdated") //��Դѡ�����
            {
                DoAddAnimation();
            }
        }

        ShowAnimationNames();

        if (GUILayout.Button("ɾ�����һ��"))
        {
            AnimationClips.RemoveAt(AnimationClips.Count - 1);
        }

        GUILayout.Label("�����ļ���", EditorStyles.boldLabel);
        SavePath = GUILayout.TextField("");

        if (GUILayout.Button("����"))
        {
            Generate();
        }
    }
}
