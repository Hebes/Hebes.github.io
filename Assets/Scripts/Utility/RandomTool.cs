using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTool
{
    /// <summary>
    /// 根据概率生成随机数
    /// </summary>
    /// <param name="probs"></param>
    /// <returns></returns>
    public static int RandomByProb(int[] probs)
    {
        //概率总和
        int total =0;
        for(int i=0;i<probs.Length;i++)
        {
            total += probs[i];
        }
        int ran=Random.Range(0,total);
        int t = 0;
        for(int i=0;i<probs.Length;i++)
        {
            t += probs[i];
            if(ran<t)
            {
                return i;
            }
        }
        return 0;
    }
}
