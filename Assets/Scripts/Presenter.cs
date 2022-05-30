using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace PresenterSpace
{
    public class Presenter : MonoBehaviour
    {
        //Controller参照
        public ControllerSpace.Controller _controller;
        //Model参照
        public ModelSpace.Model _model;
        //PresenterのReactiveProperty _viewText
        private ReactiveProperty<string> _viewText = new ReactiveProperty<string>("");
        public IObservable<string> ObservableViewText => _viewText;
        //Controllerからのキー入力受け取りとModelへ受け渡すための変数
        private Subject<string> buttonRelaySub = new Subject<string>();
        public IObservable<string> ObservableKey{
            get { return buttonRelaySub; }
        }

        void Start()
        {
            //Controllerから来たキー入力をModelに渡す
            _controller.ObservableKey.Subscribe(key => buttonRelaySub.OnNext(key));
            //Model_viewText監視
            _model.ObservableViewText.Subscribe(viewText => _viewText.Value = viewText);
        }
    }
}
