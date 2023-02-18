//using UnityEditor;
//using UnityEngine;



//[CustomEditor(typeof(EnemyController.BaseEnemy))]
//public class BaseEnemyEditor: Editor
//{
    
//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();

//        EnemyController.BaseEnemy myMonoBehaviour = (EnemyController.BaseEnemy)target;
//            if (myMonoBehaviour.Stats == null)
//            {
//                return;
//            }

//            EditorGUILayout.LabelField("Age:", myMonoBehaviour.Stats.Age.ToString());

//        if (EditorApplication.isPlaying)
//        {
//            EditorUtility.SetDirty(target);
//            EditorGUILayout.LabelField("Age:", myMonoBehaviour.Age.ToString());
//        }
//    }
    
//}
