using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Windows;

[System.Serializable]
public class Resolutions{
    public int w;
    public int h;

    public override string ToString()
    {
        return w + " X " + h;
    }
}
/*************************************
 화면 해상도를 설정하기 위한 클래스 입니다.
 싱글톤(게임 내에서 1개만 생성)으로 생성됩니다.
*************************************/
public class ResolutionFixer : MonoBehaviour
{

    private static ResolutionFixer _instance;   // 싱글톤을 위한 인스턴스
                                                // 싱글톤 장점 : 다른 클래스에서 ResolutionFixer.instance 로 접근하기 편함
    public static ResolutionFixer instance
    {
        get
        {
           return _instance;
        }
    }
    public List<Resolutions> resolutions; // 미리 정의된 해상도 비율들 선언
    private bool mode; // 전체화면, 창모드 토글
    private int index = -1; // resolution의 인덱스
    private int indexLimit = 0; // 사용자 모니터 1번(주 모니터) 기준 최대 해상도

    void Awake()
    {
        if(_instance == null){
            _instance = this; // 싱글톤 인스턴스를 해당 인스턴스로 설정
            DontDestroyOnLoad(gameObject); // 한번 생성되면 삭제 되지 않음
        }
        else{
            Destroy(gameObject); // 만약 이미 이 인스턴스가 있을 경우 삭제
            return;
        }

        var m  = PlayerPrefs.GetInt("ScreenMode", -1); // PlayerPrefs에서 ScreenMode를 읽어오되 없으면 -1 반환
        index = PlayerPrefs.GetInt("DisplayIndex", -1);// PlayerPrefs에서 DisplayIndex를 읽어오되 없으면 -1 반환
        indexLimit = findFitResolution();       // 사용자 모니터 1번(주 모니터) 기준 최대 해상도 설정
        if(index < 0){
            setIndex(indexLimit);               // 인덱스 못읽었으면 인덱스에 대치
        }

        if(m < 0){
            mode = Screen.fullScreen;                  // m 이 -1 이면 현재 화면 설정 불러 옴
        }
        else{
            mode = (m == 1);                           // m 이 1이면 fullscreen
        }

        SetResolution();                               // mode 와 index를 이용해 해상도 설정
        SceneManager.sceneLoaded += ChangedActiveScene;// 씬 바뀔 때마다 ChangedActiveScene 호출        
    }

    public void setIndex(int i){ // 인덱스 바뀔 때 바로바로 저장하기 위함 (index 는 private)
        index = i;
        PlayerPrefs.SetInt("DisplayIndex", index);
    }

    public void setMode(bool m){ // 모드 바뀔 때 바로바로 저장하기 위함 (index 는 private)
        mode = m;
        PlayerPrefs.SetInt("ScreenMode", m ? 1 : 0);
    }

    public int getIndex(){
        return index;
    }

    public int getLimit(){
        return indexLimit;
    }

    public bool getMode(){
        return mode;
    }

    public int findFitResolution(){ // 화면 리미트 확인하고 저장
        for(int i = resolutions.Count - 1; i >= 0; i--){
            if(resolutions[i].w <= Display.main.systemWidth && resolutions[i].h <= Display.main.systemHeight) {
                return i;
            }
        }
        return 0;
    }

    private void ChangedActiveScene(Scene scene, LoadSceneMode mode) // 씬이 바뀔 때 마다 호출 (없어도 될 수 있음)
    {
        SetResolution();
    }

    public void SetResolution() //설정한 Index 를 따라 해상도 설정
    {
        int setWidth = resolutions[index].w;
        int setHeight = resolutions[index].h;

        int deviceHeight = Display.main.systemHeight; // 기기 너비 저장
        int deviceWidth = Display.main.systemWidth; // 기기 높이 저장
        PlayerPrefs.SetInt("Screenmanager Resolution Width", setWidth);
        PlayerPrefs.SetInt("Screenmanager Resolution Height", setHeight);
        PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", (int)(mode ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed));

        Screen.SetResolution(setWidth, setHeight, mode ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
        
        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }

        Debug.Log("device resolution : " + deviceWidth + ", " + deviceHeight);
        Debug.Log("setting resolution : " + setWidth + ", " + setHeight);
        Debug.Log("actual set resolution : " + Screen.width + ", " + Screen.height);
    }
}
