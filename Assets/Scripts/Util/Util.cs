using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Util
{
    public static Dictionary<float, WaitForSeconds> _delayDic = new Dictionary<float, WaitForSeconds>();
    /// <summary>
    /// 코루틴 딜레이 가져오기
    /// </summary>
    public static WaitForSeconds GetDelay(this float time)
    {
        if (_delayDic.ContainsKey(time) == false)
        {
            _delayDic.Add(time, new WaitForSeconds(time));
        }
        return _delayDic[time];
    }


    private static StringBuilder _sb = new StringBuilder();
    /// <summary>
    /// TMP를 위한 StringBuilder 반환 함수
    /// </summary>
    public static StringBuilder GetText<T>(this T value)
    {
        _sb.Clear();
        _sb.Append(value);
        return _sb;
    }
}