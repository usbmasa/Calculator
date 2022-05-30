using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace ModelSpace
{
    public class ModelDisplay : MonoBehaviour
    {
        string viewText = "";
        bool isEqual = false;
        //Display表示更新
        public string UpdateView(string updateText, bool updateIsEqual = false,  string type = "Add"){
            isEqual = updateIsEqual;
            switch(type)
            {
                case "Add":
                    viewText += updateText;
                    break;

                case "Minus":
                    viewText = viewText.Remove(viewText.Length-updateText.Length, updateText.Length);
                    break;

                case "Equal":
                    viewText = updateText;
                    break;

            }  
            return viewText;
        }
        //現在ディスプレイが式を表示してるか解を表示してるかを返す
        public bool ShowIsEqual(){
            return isEqual;
        }

    }
}
