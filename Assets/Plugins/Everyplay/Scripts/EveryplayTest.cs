using UnityEngine;
using System.Collections;

public class EveryplayTest : MonoBehaviour {
	
	public bool showUploadStatus = true;

	private bool isRecording = false;
	private bool isPaused = false;
	private bool isRecordingFinished = false;

	private GUIText uploadStatusLabel;
	private Texture2D previousThumbnail;

	void Awake() {
		if(enabled && showUploadStatus) {
			CreateUploadStatusLabel();
		}
	}

	void Start() {
		if(Everyplay.SharedInstance != null) {
			if(uploadStatusLabel != null) {
				Everyplay.SharedInstance.UploadDidStart += UploadDidStart;
				Everyplay.SharedInstance.UploadDidProgress += UploadDidProgress;
				Everyplay.SharedInstance.UploadDidComplete += UploadDidComplete;
			}

			Everyplay.SharedInstance.RecordingStarted += RecordingStarted;
			Everyplay.SharedInstance.RecordingStopped += RecordingStopped;

			Everyplay.SharedInstance.ThumbnailReadyAtFilePath += ThumbnailReadyAtFilePath;
		}
	}

	void Destroy() {
		if(Everyplay.SharedInstance != null) {
			if(uploadStatusLabel != null) {
				Everyplay.SharedInstance.UploadDidStart -= UploadDidStart;
				Everyplay.SharedInstance.UploadDidProgress -= UploadDidProgress;
				Everyplay.SharedInstance.UploadDidComplete -= UploadDidComplete;
			}

			Everyplay.SharedInstance.RecordingStarted -= RecordingStarted;
			Everyplay.SharedInstance.RecordingStopped -= RecordingStopped;

			Everyplay.SharedInstance.ThumbnailReadyAtFilePath -= ThumbnailReadyAtFilePath;
		}
	}

	private void RecordingStarted() {
		isRecording = true;
		isPaused = false;
		isRecordingFinished = false;
	}

	private void RecordingStopped() {
		isRecording = false;
		isRecordingFinished = true;
	}

	private void CreateUploadStatusLabel() {
		GameObject uploadStatusLabelObj = new GameObject("UploadStatus", typeof(GUIText));

		if(uploadStatusLabelObj) {
			uploadStatusLabelObj.transform.parent = transform;
			uploadStatusLabel = uploadStatusLabelObj.GetComponent<GUIText>();

			if(uploadStatusLabel != null) {
				uploadStatusLabel.anchor = TextAnchor.LowerLeft;
				uploadStatusLabel.alignment = TextAlignment.Left;
				uploadStatusLabel.text = "Not uploading";
			}
		}
	}

	private void UploadDidStart(int videoId) {
		uploadStatusLabel.text = "Upload " + videoId + " started.";
	}

	private void UploadDidProgress(int videoId, float progress) {
		uploadStatusLabel.text = "Upload " + videoId + " is " + Mathf.RoundToInt( (float) progress * 100) + "% completed.";
	}

	private void UploadDidComplete(int videoId) {
		uploadStatusLabel.text = "Upload " + videoId + " completed.";

		StartCoroutine(ResetUploadStatusAfterDelay(2.0f));
	}

	private IEnumerator ResetUploadStatusAfterDelay(float time) {
		yield return new WaitForSeconds(time);
		uploadStatusLabel.text = "Not uploading";
	}

	private void ThumbnailReadyAtFilePath(string path) {
		// We are loading the thumbnail during the recording for demonstration purposes only.
		// Normally you should start the load after you have stopped the recording to make sure the rendering does not stutter.
		Everyplay.SharedInstance.LoadThumbnailFromFilePath(path, ThumbnailSuccess, ThumbnailError);
	}

	private void ThumbnailSuccess(Texture2D texture) {
		if(texture != null) {
			previousThumbnail = texture;
		}
	}

	private void ThumbnailError(string error) {
		Debug.Log("Thumbnail loading failed: " + error);
	}

	void OnGUI() {
		if(GUI.Button(new Rect(10, 10, 138, 48), "Everyplay")) {
			Everyplay.SharedInstance.Show();
#if UNITY_EDITOR
			Debug.Log("Everyplay view is not available in the Unity editor. Please compile and run on a device.");
#endif
		}
		
		if(isRecording && GUI.Button(new Rect(10, 64, 138, 48), "Stop Recording")) {
			Everyplay.SharedInstance.StopRecording();
#if UNITY_EDITOR
			Debug.Log("The video recording is not available in the Unity editor. Please compile and run on a device.");
#endif
		}
		else if(!isRecording && GUI.Button(new Rect(10, 64, 138, 48), "Start Recording")) {
			Everyplay.SharedInstance.StartRecording();	
#if UNITY_EDITOR
			Debug.Log("The video recording is not available in the Unity editor. Please compile and run on a device.");
#endif
		}

		if(isRecording) {
			if(!isPaused && GUI.Button(new Rect(10+150, 64, 138, 48), "Pause Recording")) {
				Everyplay.SharedInstance.PauseRecording();
				isPaused = true;
#if UNITY_EDITOR
				Debug.Log("The video recording is not available in the Unity editor. Please compile and run on a device.");
#endif
			}
			else if(isPaused && GUI.Button(new Rect(10+150, 64, 138, 48), "Resume Recording")) {
				Everyplay.SharedInstance.ResumeRecording();
				isPaused = false;
#if UNITY_EDITOR
				Debug.Log("The video recording is not available in the Unity editor. Please compile and run on a device.");
#endif
			}
		}

		if(isRecordingFinished && GUI.Button(new Rect(10, 118, 138, 48), "Play Last Recording")) {
			Everyplay.SharedInstance.PlayLastRecording();
#if UNITY_EDITOR
			Debug.Log("The video playback is not available in the Unity editor. Please compile and run on a device.");
#endif
		}

		if(isRecording && GUI.Button(new Rect(10, 118, 138, 48), "Take Thumbnail")) {
			Everyplay.SharedInstance.TakeThumbnail();
#if UNITY_EDITOR
			Debug.Log("Everyplay take thumbnail is not available in the Unity editor. Please compile and run on a device.");
#endif
		}

		if(isRecordingFinished && GUI.Button(new Rect(10, 172, 138, 48), "Show sharing modal")) {
			Everyplay.SharedInstance.ShowSharingModal();
#if UNITY_EDITOR
			Debug.Log("The sharing modal is not available in the Unity editor. Please compile and run on a device.");
#endif
		}

		if(previousThumbnail != null) {
			int xPos = Screen.width - previousThumbnail.width - 10;
			int yPos = Screen.height - previousThumbnail.height - 10;

			GUI.DrawTexture(new Rect(xPos, yPos, previousThumbnail.width, previousThumbnail.height), previousThumbnail, ScaleMode.ScaleToFit, false, 0);
		}
	}
}