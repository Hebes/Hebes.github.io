using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Tool
{
    public static class TransformExpand
    {

        /// <summary>
        /// 未知层级,查找后代指定名称挂在的组件。
        /// </summary>
        /// <param name="currentTF">当前变换组件</param>
        /// <param name="childName">后代物体名称</param>
        /// <returns></returns>
        public static T OnFindAnyComponent<T>(this Transform transform, string childName) where T : Component
        {
            //递归:方法内部又调用自身的过程。
            //1.在子物体中查找
            return tfGet(transform, childName) != null ? tfGet(transform, childName).GetComponent<T>() : null;
        }

        /// <summary>
        /// 未知层级,查找后代指定名称的变换组件。
        /// </summary>
        /// <param name="currentTF">当前变换组件</param>
        /// <param name="childName">后代物体名称</param>
        /// <returns></returns>
        private static Transform tfGet(this Transform transform, string childName)
        {
            //递归:方法内部又调用自身的过程。
            //1.在子物体中查找
            Transform childTF = transform.Find(childName);
            if (childTF != null)
                return childTF;
            for (int i = 0; i < transform.childCount; i++)
            {
                // 2.将任务交给子物体
                childTF = tfGet(transform.GetChild(i), childName);
                if (childTF != null)
                    return childTF;
            }
            return null;
        }

        /// <summary>
        /// 获取transform组件
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name">需要获取的组件名字</param>
        /// <returns></returns>
        public static Transform OnGetTransform(this Transform transform, string name)
        {
            return transform.OnFindAnyComponent<Transform>(name);
        }

        /// <summary>
        /// 获取transform组件
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name">需要获取的组件名字</param>
        /// <returns></returns>
        public static Text OnGetText(this Transform transform, string name)
        {
            return transform.OnFindAnyComponent<Text>(name);
        }

        /// <summary>
        /// 获取InputField组件
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name">需要获取的组件名字</param>
        /// <returns></returns>
        public static InputField OnGetInputField(this Transform transform, string name)
        {
            return transform.OnFindAnyComponent<InputField>(name);
        }

        /// <summary>
        /// 获取InputField组件
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name">需要获取的组件名字</param>
        /// <returns></returns>
        public static Toggle OnGetToggle(this Transform transform, string name)
        {
            return transform.OnFindAnyComponent<Toggle>(name);
        }

        /// <summary>
        /// 获取Image组件
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name">需要获取的组件名字</param>
        /// <returns></returns>
        public static Image OnGetImage(this Transform transform, string name)
        {
            return transform.OnFindAnyComponent<Image>(name);
        }

        /// <summary>
        /// 获取Dropdown组件
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name">需要获取的组件名字</param>
        /// <returns></returns>
        public static Dropdown OnGetDropdown(this Transform transform, string name)
        {
            return transform.OnFindAnyComponent<Dropdown>(name);
        }

        /// <summary>
        /// 获取Button组件
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name">需要获取的组件名字</param>
        /// <returns></returns>
        public static Button OnGetButton(this Transform transform, string name)
        {
            return transform.OnFindAnyComponent<Button>(name);
        }
    }
}

