using UnityEngine;
using System.Collections;

public class EveryplayThumbnailPool : MonoBehaviour {
	public int thumbnailCount = 4;
	public int thumbnailWidth = 128;
	public bool pixelPerfect = false;
	public bool takeRandomShots = true;

	public TextureFormat textureFormat = TextureFormat.RGBA32;
	
	public Texture2D[] thumbnailTextures {get; private set; }
	public int availableThumbnailCount {get; private set; }
	public float aspectRatio {get; private set; }
	public Vector2 thumbnailScale {get; private set; }
	
	private int currentThumbnailTextureIndex;
	private float nextRandomShotTime;
	
	void Start() {
		if(enabled) {
			// Limit width between 32 and 2048
			thumbnailWidth = Mathf.Clamp(thumbnailWidth, 32, 2048);

			// Thumbnails are always stored landscape in memory
			aspectRatio = (float) Mathf.Min(Screen.width, Screen.height) / (float) Mathf.Max(Screen.width, Screen.height);
			
			// Calculate height based on aspect ratio
			int thumbnailHeight = (int)(thumbnailWidth * aspectRatio);

			// Check for npot support, always use pot textures for older Unitys versions and if npot support is not available
			bool npotSupported = false;

#if !(UNITY_3_5	|| UNITY_4_0 || UNITY_4_0_1)
			npotSupported = (SystemInfo.npotSupport != NPOTSupport.None);
#endif
			int thumbnailPotWidth = Mathf.NextPowerOfTwo(thumbnailWidth);
			int thumbnailPotHeight = Mathf.NextPowerOfTwo(thumbnailHeight);

			// Create empty textures for requested amount of thumbnails
			thumbnailTextures = new Texture2D[thumbnailCount];
	
			for(int i=0; i<thumbnailCount; i++) {
				thumbnailTextures[i] = new Texture2D(npotSupported ? thumbnailWidth : thumbnailPotWidth, npotSupported ? thumbnailHeight : thumbnailPotHeight, textureFormat, false);
				// Always use clamp to assure texture completeness when npot support is restricted
				thumbnailTextures[i].wrapMode = TextureWrapMode.Clamp;
			}

			// Set thumbnail render target to the first texture
			currentThumbnailTextureIndex = 0;

			Everyplay.SharedInstance.SetThumbnailTargetTextureId(thumbnailTextures[currentThumbnailTextureIndex].GetNativeTextureID());

			if(npotSupported) {
				Everyplay.SharedInstance.SetThumbnailTargetTextureWidth(thumbnailWidth);
				Everyplay.SharedInstance.SetThumbnailTargetTextureHeight(thumbnailHeight);

				thumbnailScale = new Vector2(1, 1);
			}
			else {
				if(pixelPerfect) {
					Everyplay.SharedInstance.SetThumbnailTargetTextureWidth(thumbnailWidth);
					Everyplay.SharedInstance.SetThumbnailTargetTextureHeight(thumbnailHeight);				

					thumbnailScale = new Vector2((float) thumbnailWidth / (float) thumbnailPotWidth, (float) thumbnailHeight / (float) thumbnailPotHeight);
				}
				else {
					Everyplay.SharedInstance.SetThumbnailTargetTextureWidth(thumbnailPotWidth);
					Everyplay.SharedInstance.SetThumbnailTargetTextureHeight(thumbnailPotHeight);

					thumbnailScale = new Vector2(1, 1);
				}
			}

			// Add thumbnail take listener
			Everyplay.SharedInstance.ThumbnailReadyAtTextureId += OnThumbnailReady;
			Everyplay.SharedInstance.RecordingStarted += OnRecordingStarted;
		}
	}

	private void OnRecordingStarted() {
		availableThumbnailCount = 0;
		currentThumbnailTextureIndex = 0;
		
		Everyplay.SharedInstance.SetThumbnailTargetTextureId(thumbnailTextures[currentThumbnailTextureIndex].GetNativeTextureID());
	
		if(takeRandomShots) {
			Everyplay.SharedInstance.TakeThumbnail();
			nextRandomShotTime = Time.time + Random.Range(3.0f, 15.0f);
		}
	}
		
	void Update() {
		if(takeRandomShots && Everyplay.SharedInstance.IsRecording() && !Everyplay.SharedInstance.IsPaused()) {
			if(Time.time > nextRandomShotTime) {
				Everyplay.SharedInstance.TakeThumbnail();
				nextRandomShotTime = Time.time + Random.Range(3.0f, 15.0f);
			}
		}
	}

	private void OnThumbnailReady(int id, bool portrait) {
		if(thumbnailTextures[currentThumbnailTextureIndex].GetNativeTextureID() == id) {
			currentThumbnailTextureIndex++;
			
			if(currentThumbnailTextureIndex >= thumbnailTextures.Length) {
				currentThumbnailTextureIndex = 0;
			}
			
			if(availableThumbnailCount < thumbnailTextures.Length) {
				availableThumbnailCount++;
			}

			Everyplay.SharedInstance.SetThumbnailTargetTextureId(thumbnailTextures[currentThumbnailTextureIndex].GetNativeTextureID());
		}
	}

	void Destroy() {
		// Set Everyplay not to render to a texture anymore and remove event handlers
		if(Everyplay.SharedInstance) {
			Everyplay.SharedInstance.SetThumbnailTargetTextureId(0);
			Everyplay.SharedInstance.RecordingStarted -= OnRecordingStarted;
			Everyplay.SharedInstance.ThumbnailReadyAtTextureId -= OnThumbnailReady;
		}

		// Destroy thumbnail textures
		foreach(Texture2D texture in thumbnailTextures) {
			Destroy(texture);
		}
	}
}
