using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Numlock))
        {
            /*
            while ( i < 20)
            {
                i++;   
                Debug.Log(i);
            }
            */
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            /*
            for (int before = 1 ,after = 1,i=1;before + after < 100;)
            {
                if(before ==1 && after == 1)
                {
                    Debug.Log(before);
                    Debug.Log(after);
                }
                i = before + after;
                before = after;
                after = i;
                Debug.Log(i);
            }
            */
            for (int i = 1; i <= 100; i++)
            {
                int num = 0; //約数の数
                for (int j = 1; j < i; j++)
                {
                    if (i % j == 0)
                    {
                        num = num + 1;
                    }
                }
                if (num == 1)
                {
                    Debug.Log(i);
                }
            }
        }
    }
}
