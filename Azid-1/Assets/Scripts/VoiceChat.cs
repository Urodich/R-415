using UnityEngine;
using agora_gaming_rtc;


public class VoiceChat : MonoBehaviour
{
    public int InputRate;
    public int OutputRate;
    private string AppID = "4d1f7e658bdf454780ba1238fe2b3225";
    const string startChannelName = "R-415:";
    private IRtcEngine mRtcEngine = null;
    private uint currentUid;
    private int dataStreamId;
    public static VoiceChat instance;
    private bool isConnected=false;
    void Awake(){
        if (instance){
            Destroy(gameObject);
        }
        else{
            instance=this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start(){
        mRtcEngine = IRtcEngine.GetEngine(AppID);
        if (mRtcEngine==null) Debug.Log("no rtc");

        mRtcEngine.OnJoinChannelSuccess += (string channelName, uint uid, int elapsed) =>{
            Debug.Log("ConnectedSuccess");
            currentUid=uid;
            dataStreamId = mRtcEngine.CreateDataStream(false, false);
            byte[] bytes = new byte[12];  
            System.BitConverter.GetBytes(InputRate).CopyTo(bytes, 0);
            System.BitConverter.GetBytes(OutputRate).CopyTo(bytes, 4);
            System.BitConverter.GetBytes(0).CopyTo(bytes, 8);
            mRtcEngine.SendStreamMessage(dataStreamId, bytes);
        };
        mRtcEngine.OnStreamMessage += (uint senderId, int streamId, byte[] data, int length) =>{
            Debug.Log("GetMessage");
            int[] intData = new int[] { System.BitConverter.ToInt32(data, 0), System.BitConverter.ToInt32(data, 4), System.BitConverter.ToInt32(data, 8) }; 

            if(intData[1]!=InputRate) mRtcEngine.MuteRemoteAudioStream(senderId, true);
            if(intData[2]==1) return;
            if(intData[0]!=OutputRate) {
                byte[] bytes = new byte[12];  
                System.BitConverter.GetBytes(InputRate).CopyTo(bytes, 0);
                System.BitConverter.GetBytes(OutputRate).CopyTo(bytes, 4);
                System.BitConverter.GetBytes(1).CopyTo(bytes, 8);
                mRtcEngine.SendStreamMessage(dataStreamId, bytes);
            }
        };
        mRtcEngine.OnWarning += (int warn, string msg) =>
        {
            string description = IRtcEngine.GetErrorDescription(warn);
            string warningMessage = string.Format("onWarning callback {0} {1} {2}", warn, msg, description);
            Debug.Log(warningMessage);
        };

        mRtcEngine.OnError += (int error, string msg) =>
        {
            string description = IRtcEngine.GetErrorDescription(error);
            string errorMessage = string.Format("onError callback {0} {1} {2}", error, msg, description);
            Debug.Log(errorMessage);
        };
        
        mRtcEngine.OnRtcStats += (RtcStats stats) =>
        {
            string rtcStatsMessage = string.Format("onRtcStats callback duration {0}, tx: {1}, rx: {2}, tx kbps: {3}, rx kbps: {4}, tx(a) kbps: {5}, rx(a) kbps: {6} users {7}",
                stats.duration, stats.txBytes, stats.rxBytes, stats.txKBitRate, stats.rxKBitRate, stats.txAudioKBitRate, stats.rxAudioKBitRate, stats.userCount);
            Debug.Log(rtcStatsMessage);
        };
        mRtcEngine.OnConnectionInterrupted += () =>
        {
            string interruptedMessage = string.Format("OnConnectionInterrupted");
            Debug.Log(interruptedMessage);
        };

        mRtcEngine.OnConnectionLost += () =>
        {
            string lostMessage = string.Format("OnConnectionLost");
            Debug.Log(lostMessage);
        };

    }
    public void SelfCheck(bool isOn){
        if (mRtcEngine==null) return;

        Debug.Log("selfCheck "+isOn);
        if(isOn) mRtcEngine.StartEchoTest(intervalInSeconds: 2);
        else mRtcEngine.StopEchoTest();
    }
    public void MuteMe(bool mute){
        if (mRtcEngine==null) return;

        Debug.Log("Mute "+ mute);
        mRtcEngine.MuteLocalAudioStream(mute);
    }

    public void JoinChannel(string channelName){
        if (mRtcEngine==null) return;

        if(mRtcEngine.StopEchoTest()==0) Debug.Log("StopEchoTest");
        else Debug.Log("Can't StopEchoTest");
        mRtcEngine.EnableAudio(); 
        if(isConnected){
            mRtcEngine.LeaveChannel();
        }
        isConnected=(mRtcEngine.JoinChannel(startChannelName+channelName, "extra", 0)==0);
        if(isConnected) Debug.Log("Joined Channel");
    }

    public void LeaveChannel()
    {
        if (mRtcEngine==null) return;
        Debug.Log("disconnected");
        if(isConnected)mRtcEngine.LeaveChannel();
        mRtcEngine.DisableAudio(); 
        isConnected=false;
    }

    public void ChangeOutputVolume(int value){
        if (mRtcEngine==null) return;
        if(value>100) value=100;
        if(value<0) value=0;
        mRtcEngine.AdjustRecordingSignalVolume(value);
    }
    
    public void ChangeInputVolume(int value){
        if (mRtcEngine==null) return;
        if(value>100) value=100;
        if(value<0) value=0;
        mRtcEngine.AdjustPlaybackSignalVolume(value);
    }

    void OnApplicationQuit(){    	
        Debug.Log("OnApplicationQuit");
    	if (mRtcEngine != null){
        	mRtcEngine.LeaveChannel();
        	mRtcEngine.DisableAudio();        	
            IRtcEngine.Destroy();
    	}
	}

    void OnDestroy(){
        if(Application.isEditor) OnApplicationQuit();
    }
}
