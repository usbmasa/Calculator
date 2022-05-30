using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using System.Collections.Generic;
using Calculator;

namespace ModelSpace
{
    public class Model : MonoBehaviour
    {
        //Presenter参照
        public PresenterSpace.Presenter _presenter;

        //ModelDisplay参照
        public ModelDisplay _modelDisplay;

        //ModelのReactiveProperty _viewText
        private ReactiveProperty<string> _viewText = new ReactiveProperty<string>("");
        public IObservable<string> ObservableViewText => _viewText;

        private Subject<string> filterSub = new Subject<string>();

        private string tmpNumber = "";
        private string[] tmpNumbers = new string[0];
        private double answerNumber = 0;
        private string[] tmpSymbols = new string[0];
        private int mathCount = 0;
        private double memoryNumber;

        void Start()
        {
            //キー入力受け取り
            _presenter.ObservableKey.Subscribe(key => filterSub.OnNext(key));

            //キー入力フィルタリング
            var branch = filterSub.Publish();

            var clearEntryFilter = branch
                .Where(key => key=="CE")
                .Subscribe(key => ClearEntryKey());

            var clearFilter = branch
                .Where(key => key=="C" || key=="OFF")
                .Subscribe(key => ClearKey());

            var numberFilter = branch
                .Where(key => key=="." || key=="0" || key=="1" || key=="2" || key=="3" || key=="4" || key=="5" || key=="6" || key=="7" || key=="8" || key=="9")
                .Subscribe(key => NumberKey(key));

            var mathFilter = branch
                .Where(key => key=="+" || key=="-" || key=="×" || key=="÷")
                .Subscribe(key => MathKey(key));

            var parcentFilter = branch
                .Where(key => key=="%")
                .Subscribe(key => ParcentKey(key));

            var sqrtFilter = branch
                .Where(key => key=="√")
                .Subscribe(key => SqrtKey(key));

            var equalFilter = branch
                .Where(key => key=="=")
                .Subscribe(key => EqualKey());

            var memoryFilter = branch
                .Where(key => key=="M+" || key=="M-" || key=="MRC")
                .Subscribe(key => MemoryKey(key));

            branch.Connect();
        }

        //Clearキー入力
        void ClearKey(){
            tmpNumbers = new string[0];
            tmpSymbols = new string[0];
            mathCount = 0;
            answerNumber = 0;
            tmpNumber = "";
            _viewText.Value = _modelDisplay.UpdateView("",false,"Equal");
            Debug.Log("C");
        }
        //ClearEntryキー入力
        void ClearEntryKey(){
            if(_modelDisplay.ShowIsEqual()){
                _viewText.Value = _modelDisplay.UpdateView("",true,"Equal");
                tmpNumber = "";
            }else{
                _viewText.Value = _modelDisplay.UpdateView(tmpNumber,false,"Minus");
                tmpNumber = "";
            }
        }
        //数字キー入力
        void NumberKey(string key)
        {
            if(_modelDisplay.ShowIsEqual()){
                ClearKey();
            }
            //入力した数字をディスプレイに追加
            tmpNumber += key;
            _viewText.Value = _modelDisplay.UpdateView(key);
        }
        //四則演算キー入力
        void MathKey(string key)
        {
            if(tmpNumber != ""){
                if(_modelDisplay.ShowIsEqual()){
                    //前回の計算の解を一時的に保存
                    string tmpAnswerNumber = answerNumber.ToString();
                    //初期化
                    ClearKey();
                    //前回の計算の解を使って計算を開始する
                    tmpNumber = tmpAnswerNumber;
                    _viewText.Value = _modelDisplay.UpdateView(tmpNumber);
                }
                //入力した符号をディスプレイに追加
                _viewText.Value = _modelDisplay.UpdateView(key);
                //配列に入力した数字と符号を保存
                Array.Resize(ref tmpNumbers, mathCount+1);
                tmpNumbers[mathCount] += tmpNumber;
                //一時保存した数字は削除
                tmpNumber = "";
                //押した符号を配列に保存
                Array.Resize(ref tmpSymbols, mathCount+1);
                tmpSymbols[mathCount] = key;
                mathCount += 1;
            } 
        }
        //%キー入力
        void ParcentKey(string key)
        {
            if(tmpNumber != ""){
                if(_modelDisplay.ShowIsEqual()){
                    //前回の計算の解を一時的に保存
                    string tmpAnswerNumber = answerNumber.ToString();
                    //初期化
                    ClearKey();
                    //前回の計算の解を使って計算を開始する
                    tmpNumber = tmpAnswerNumber;
                    _viewText.Value = _modelDisplay.UpdateView(tmpNumber);
                }

                //百分率する数値をディスプレイから削除
                _viewText.Value = _modelDisplay.UpdateView(tmpNumber,false,"Minus");
                //入力した数字を百分率で計算
                tmpNumber = Calculator.Calculate.DoMath(key,ToDouble(tmpNumber)).ToString();
                //百分率した数値を表示
                _viewText.Value = _modelDisplay.UpdateView(tmpNumber);
                //一時保存した数字は削除
            }
        }
        //平方根キー入力
        void SqrtKey(string key)
        {
            if(tmpNumber != ""){
                if(_modelDisplay.ShowIsEqual()){
                    //前回の計算の解を一時的に保存
                    string tmpAnswerNumber = answerNumber.ToString();
                    //初期化
                    ClearKey();
                    //前回の計算の解を使って計算を開始する
                    tmpNumber = tmpAnswerNumber;
                    _viewText.Value = _modelDisplay.UpdateView(tmpNumber);
                }

                //平方根を計算する数値をディスプレイから削除
                _viewText.Value = _modelDisplay.UpdateView(tmpNumber,false,"Minus");
                //入力した数字を平方根で計算
                tmpNumber = Calculator.Calculate.DoMath(key,ToDouble(tmpNumber)).ToString();
                //平方根を計算した数値を表示
                _viewText.Value = _modelDisplay.UpdateView(tmpNumber);
                //一時保存した数字は削除
            }
        }
        //=キー入力
        void EqualKey()
        {
            if(_modelDisplay.ShowIsEqual()){
                //解を出した後再度=を押した時配列の最後の数字と符号を使って計算
                answerNumber = Calculator.Calculate.DoMath(tmpSymbols[mathCount-1],answerNumber,ToDouble(tmpNumbers[mathCount]));
                Debug.Log("再Equal");
            }else{
                //=押す前の最後の数字を配列の最後に入れる
                Array.Resize(ref tmpNumbers, mathCount+1);
                tmpNumbers[mathCount] += tmpNumber;
                //配列の最初の数字を解に入れる
                Debug.Log(tmpNumbers[0]);
                answerNumber = ToDouble(tmpNumbers[0]);
                //配列に入れた数字と符号を元に順番に計算
                for(int i = 0; i < tmpSymbols.Length; i++){
                    Debug.Log(tmpSymbols[i]);
                    answerNumber = Calculator.Calculate.DoMath(tmpSymbols[i],answerNumber,ToDouble(tmpNumbers[i+1]));   
                }
            }
            //UpdateView(answerNumber.ToString(),"Equal");
            _viewText.Value = _modelDisplay.UpdateView(answerNumber.ToString(),true,"Equal");
        }
        //Memoryキー入力
        void MemoryKey(string key)
        {
            string tmpMemoryNumber;
            if(_modelDisplay.ShowIsEqual()){
                //解を保存
                tmpMemoryNumber = answerNumber.ToString();
            }else{
                //入力した数値を保存
                tmpMemoryNumber = tmpNumber;
            }
            switch(key)
            {
                case "M+":
                    //メモリーに足す
                    memoryNumber += ToDouble(tmpMemoryNumber);
                    _viewText.Value = _modelDisplay.UpdateView(_viewText.Value,true,"Equal");
                    break;
                case "M-":
                    //メモリーから引く
                    memoryNumber += ToDouble(tmpMemoryNumber);
                    _viewText.Value = _modelDisplay.UpdateView(_viewText.Value,true,"Equal");
                    break;
                case "MRC":
                    //メモリーの数値を呼び出す
                    tmpNumber = memoryNumber.ToString(); 
                    _viewText.Value = _modelDisplay.UpdateView(tmpNumber);
                    break;
            }
        }

        //文字列をdoubleに変換
        double ToDouble(string changeText){
            try
            {  
                return double.Parse(changeText);
            }
            catch(FormatException)
            {
                return 0;
                Debug.Log("不正な値だよ");
            } 
        }
    }
}
