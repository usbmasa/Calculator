using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UnityEngine.UI;

namespace ControllerSpace
{
    public class Controller : MonoBehaviour
    {
        //一つに一つの機能のあるキー
        [SerializeField]
        private Button[] simplekeys;
        //Presenter参照
        [SerializeField]
        private PresenterSpace.Presenter _presenter;
        //UIのDisplay
        [SerializeField]
        private InputField display;

        //Viewからのキー入力受け取りとControllerへ受け渡すための変数
        private Subject<string> buttonClickSub = new Subject<string>();
        public IObservable<string> ObservableKey{
            get { return buttonClickSub; }
        }

        private bool isPower = false;
        private bool isClearEntry = false;

        void Start()
        {
            //キー0
            simplekeys[0].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[0].name));
            //キー1
            simplekeys[1].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[1].name));
            //キー2
            simplekeys[2].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[2].name));
            //キー3
            simplekeys[3].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[3].name));
            //キー4
            simplekeys[4].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[4].name));
            //キー5
            simplekeys[5].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[5].name));
            //キー6
            simplekeys[6].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[6].name));
            //キー7
            simplekeys[7].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[7].name));
            //キー8
            simplekeys[8].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[8].name));
            //キー9
            simplekeys[9].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[9].name));
            //キー.
            simplekeys[10].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[10].name));
            //キー=
            simplekeys[11].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[11].name));
            //キー+
            simplekeys[12].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[12].name));
            //キー-
            simplekeys[13].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[13].name));
            //キー×
            simplekeys[14].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[14].name));
            //キー÷
            simplekeys[15].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[15].name));
            //キー√
            simplekeys[16].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[16].name));
            //キー%
            simplekeys[17].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[17].name));
            //キーMRC
            simplekeys[18].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[18].name));
            //キーM+
            simplekeys[19].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[19].name));
            //キーM-
            simplekeys[20].onClick.AsObservable ().Subscribe(count => OnClick(simplekeys[20].name));
            //キーOFF
            simplekeys[21].onClick.AsObservable ()
            .Subscribe(count => {
                isPower=false;
                OnPower();
                });  
            //キーC/CE
            simplekeys[22].onClick.AsObservable ()
            .Subscribe(count => {
                isPower=true;
                OnPower();
                });

            //Presenter_viewText監視
            _presenter.ObservableViewText.Subscribe(viewText => Show(viewText));
        }
        //UIボタン入力
        private void OnClick(string key)
        {
            if(isPower){
                isClearEntry = false;
                //ボタン入力値をPresenterに渡す
                buttonClickSub.OnNext(key);
            }
            
        }

        private void OnPower(){
            if(isPower){
                display.GetComponent<Image>().color = new Color(0.7f,0.9f,0.9f,1.0f);
                if(isClearEntry){
                    buttonClickSub.OnNext("C");
                }else{
                    buttonClickSub.OnNext("CE");
                    isClearEntry = true;
                }
            }else{
                display.GetComponent<Image>().color = new Color(0.3f,0.3f,0.3f,1.0f);
                buttonClickSub.OnNext("OFF");
            }
        }

        //UIのDisplayに表示
        private void Show(string viewText){
            display.text = viewText;
        }

    }
}

