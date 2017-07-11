using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Common.Contract;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using Plugin.Connectivity;
using AVFoundation;
using Foundation;
using UIKit;
using System.Drawing;

namespace CITS
{
    public partial class ProblemPage : ContentPage
    {
        public event EventHandler SolutionSubmitted;
        public event EventHandler HintRequested;
        private int currentHintNumber = 0;
        private readonly EmotionServiceClient emotionServiceClient;
       
		bool flashOn = false;

		AVCaptureSession captureSession;
		AVCaptureDeviceInput captureDeviceInput;
		AVCaptureStillImageOutput stillImageOutput;
		AVCaptureVideoPreviewLayer videoPreviewLayer;

        public String ProblemNumber
        {
            get;
            set;
        }
        public String Problem
        {
            get;
            set;
        }

        public String Solution
        {
            get;
            set;
        }
        public Entry SolutionTextField
        {
            get;
            set;
        }
        		
        public ProblemPage()
        {
            InitializeComponent();
            this.emotionServiceClient = new EmotionServiceClient("8b05986a8a55476783fed041df890036");
           
		    AuthorizeCameraUse();

			SetupLiveCameraStream();
			
			var device = GetCameraForOrientation(AVCaptureDevicePosition.Front);
			ConfigureCameraForDevice(device);

			captureSession.BeginConfiguration();
			captureSession.RemoveInput(captureDeviceInput);
			captureDeviceInput = AVCaptureDeviceInput.FromDevice(device);
			captureSession.AddInput(captureDeviceInput);
			captureSession.CommitConfiguration();


           
		}

       
        public List<String> Hints
        {
            get;
            set;
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            //   throw new NotImplementedException();
        }

        async void OnSubmitButtonClicked(object sender, EventArgs args)
        {
            this.Solution = SolutionEntry.Text;
            //TakePictureAndPostIt();
            //    this.SolutionEntry.Unfocus();
            //      this.SolutionEntry.Unfocus();
            //      this.SolutionEntry.Unfocus();
            //	this.Focus();
         
            TakePictureAndShrinkIt();

            SolutionSubmitted(this, EventArgs.Empty);
            //await Navigation.PopModalAsync();
        }

        async private void TakePictureAndPostIt()
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
                PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
        }

        async private void TakePictureAndShrinkIt()
        {

			/*Uncomment this block of code to revert back*/
			//var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
			//{
			//    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
			//};
			//var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(mediaOptions);



			//test manual


			//var device = GetCameraForOrientation(AVCaptureDevicePosition.Front);
			//ConfigureCameraForDevice(device);

			//captureSession.BeginConfiguration();
			//captureSession.RemoveInput(captureDeviceInput);
			//captureDeviceInput = AVCaptureDeviceInput.FromDevice(device);
			//captureSession.AddInput(captureDeviceInput);
			//captureSession.CommitConfiguration();


			var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
			var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);

			var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
            var uiImage = UIImage.LoadFromData(jpegImageAsNsData);
            var resizedImage = ScaledImage(uiImage, 500, 500);

            //var newMediaFile = new MediaFile(resizedImage.)
            	//var jpegAsByteArray = jpegImageAsNsData.ToArray();
           // jpegAsByteArray
            //var photo = resizedImage.AsJPEG((System.nfloat)1.0).AsStream();
          
           // var photo = new MemoryStream(jpegAsByteArray);

            //end test manual



            ClearEmotionResults("{Analyzing...}");
            //if (photo != null)
            //PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
            PhotoImage.Source = ImageSource.FromStream(() => { return resizedImage.AsJPEG(1.0f).AsStream();  });
            //  await analyseImage(photo.GetStream());
            var emotions = await DetectEmotionsManuallyAsync(jpegImageAsNsData);
          //  photo.Dispose();
            this.AngerLabel.Text = $"Anger: {emotions.Anger.ToString()}";
            this.ContemptLabel.Text = $"Contempt: {emotions.Contempt.ToString()}";
            this.DisgustLabel.Text = $"Disgust: {emotions.Disgust.ToString()}";
            this.FearLabel.Text = $"Fear: {emotions.Fear.ToString()}";
            this.HappinessLabel.Text = $"Happiness: {emotions.Happiness.ToString()}";
            this.NeutralLabel.Text = $"Neutral: {emotions.Neutral.ToString()}";
            this.SadnessLabel.Text = $"Sadness: {emotions.Sadness.ToString()}";
            this.SurpriseLabel.Text = $"Surprise: {emotions.Surprise.ToString()}";

           








            KeyValuePair<string, float> e = new KeyValuePair<string, float>("zero", 0);
            using (IEnumerator<KeyValuePair<string, float>> enumer = emotions.ToRankedList().GetEnumerator())
            {
                if (enumer.MoveNext()) e = enumer.Current;
            }
          
            string highestEmotion = e.Key;
            var highlightedColor = Color.Red;
            switch (highestEmotion)
            {
                case "Anger":
                    this.AngerLabel.TextColor = highlightedColor;
                    break;
                case "Contempt":
                    this.ContemptLabel.TextColor = highlightedColor;
                    break;
                case "Disgust":
                    this.DisgustLabel.TextColor = highlightedColor;
                    break;
                case "Fear":
                    this.FearLabel.TextColor = highlightedColor;
                    break;

                case "Happiness":
                    this.HappinessLabel.TextColor = highlightedColor;
                    break;

                case "Neutral":
                    this.NeutralLabel.TextColor = highlightedColor;
                    break;

                case "Sadness":

                    this.SadnessLabel.TextColor = highlightedColor;
                    break;


                case "Surprise":

                    this.SurpriseLabel.TextColor = highlightedColor;
                    break;
                default: break;

            }


        }

        private void ClearEmotionResults(string status)
        {
            this.AngerLabel.Text = $"Anger: {status}";
            this.ContemptLabel.Text = $"Contempt: {status}";
            this.DisgustLabel.Text = $"Disgust: {status}";
            this.FearLabel.Text = $"Fear: {status}";
            this.HappinessLabel.Text = $"Happiness: {status}";
            this.NeutralLabel.Text = $"Neutral: {status}";
            this.SadnessLabel.Text = $"Sadness: {status}";
            this.SurpriseLabel.Text = $"Surprise: {status}";

            var defaultTextColor = Color.Black;
            this.AngerLabel.TextColor = defaultTextColor;
            this.ContemptLabel.TextColor = defaultTextColor;
            this.DisgustLabel.TextColor = defaultTextColor;
            this.FearLabel.TextColor = defaultTextColor;
            this.HappinessLabel.TextColor = defaultTextColor;
            this.NeutralLabel.TextColor = defaultTextColor;
            this.SadnessLabel.TextColor = defaultTextColor;
            this.SurpriseLabel.TextColor = defaultTextColor;
        }
        //UIImage ScaledImage(UIImage image, nfloat maxWidth, nfloat maxHeight)
        //{
        //	var maxResizeFactor = Math.Min(maxWidth / image.Size.Width, maxHeight / image.Size.Height);
        //	var width = maxResizeFactor * image.Size.Width;
        //	var height = maxResizeFactor * image.Size.Height;
        //	return image.Scale(new CoreGraphics.CGSize(width, height));
        //}

        private async Task<EmotionScores> DetectEmotionsAsync(MediaFile inputFile)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Network error",
                  "Please check your network connection and retry.", "OK");
                return null;
            }

            try
            {
                // Get emotions from the specified stream
                Emotion[] emotionResult = await
                  emotionServiceClient.RecognizeAsync(inputFile.GetStream());
                // Assuming the picture has one face, retrieve emotions for the
                // first item in the returned array
                var faceEmotion = emotionResult[0]?.Scores;

                return faceEmotion;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                return null;
            }
        }
        //async System.Threading.Tasks.Task analyseImage(Stream imageStream)
        //{
        //	try
        //	{
        //		VisionServiceClient visionClient = new VisionServiceClient("<<YOUR API KEY HERE>>");
        //		VisualFeature[] features = { VisualFeature.Tags, VisualFeature.Categories, VisualFeature.Description };
        //		var analysisResult = await visionClient.AnalyzeImageAsync(imageStream, features.ToList(), null);
        //		AnalysisLabel.Text = string.Empty;
        //		analysisResult.Description.Tags.ToList().ForEach(tag => AnalysisLabel.Text = AnalysisLabel.Text + tag + "\n");
        //	}
        //	catch (Microsoft.ProjectOxford.Vision.ClientException ex)
        //	{
        //		AnalysisLabel.Text = ex.Error.Message;
        //	}
        //}

        public void UpdateProblemPage(string status)
        {
            ProblemNumberLabel.Text = ProblemNumber;
            ProblemLabel.Text = Problem;
            SolutionEntry.Text = "";
            currentHintNumber = 0;
            SolutionTextField = SolutionEntry;
            ClearEmotionResults(status);
            //    SolutionEntry.Focus();
        }

        void OnHintButtonClicked(object sender, EventArgs args)
        {
            if (currentHintNumber < Hints.Count)
            {
                DisplayAlert("Hint", this.Hints[currentHintNumber], "OK");
                currentHintNumber++;
                HintRequested(this, EventArgs.Empty);
            }
            else
            {
                DisplayAlert("Hint", "There are no more hints for this problem", "OK");
            }
        }


		//Manual
		async void TakePhotoButtonTapped()
		{
			var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
			var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);

			var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
			var jpegAsByteArray = jpegImageAsNsData.ToArray();

			// TODO: Send this to local storage or cloud storage such as Azure Storage.
		}

		

		async Task AuthorizeCameraUse()
		{
			var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);

			if (authorizationStatus != AVAuthorizationStatus.Authorized)
			{
				await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
			}
		}

		public void SetupLiveCameraStream()
		{
			captureSession = new AVCaptureSession();

		//	var viewLayer = liveCameraStream.Layer;
		//	videoPreviewLayer = new AVCaptureVideoPreviewLayer(captureSession)
		//	{
		//		Frame = this.View.Frame
		//	};
		//	liveCameraStream.Layer.AddSublayer(videoPreviewLayer);

			var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
			ConfigureCameraForDevice(captureDevice);
			captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);
			captureSession.AddInput(captureDeviceInput);

			var dictionary = new NSMutableDictionary();
			dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
			stillImageOutput = new AVCaptureStillImageOutput()
			{
				OutputSettings = new NSDictionary()
			};

			captureSession.AddOutput(stillImageOutput);
			captureSession.StartRunning();
		}

		void ConfigureCameraForDevice(AVCaptureDevice device)
		{
			var error = new NSError();
			if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
			{
				device.LockForConfiguration(out error);
				device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
				device.UnlockForConfiguration();
			}
			else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
			{
				device.LockForConfiguration(out error);
				device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
				device.UnlockForConfiguration();
			}
			else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
			{
				device.LockForConfiguration(out error);
				device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
				device.UnlockForConfiguration();
			}
		}

		private async Task<EmotionScores> DetectEmotionsManuallyAsync(NSData inputFile)
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				await DisplayAlert("Network error",
				  "Please check your network connection and retry.", "OK");
				return null;
			}

			try
			{
				// Get emotions from the specified stream
				Emotion[] emotionResult = await
                    emotionServiceClient.RecognizeAsync(new MemoryStream(inputFile.ToArray()));
				
				// Assuming the picture has one face, retrieve emotions for the
				// first item in the returned array
				var faceEmotion = emotionResult[0]?.Scores;

				return faceEmotion;
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", ex.Message, "OK");
				return null;
			}
		}

		public AVCaptureDevice GetCameraForOrientation(AVCaptureDevicePosition orientation)
		{
			var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
			foreach (var device in devices)
			{
				if (device.Position == orientation)
				{
					return device;
				}
			}

			return null;
		}



UIImage ScaledImage(UIImage image, nfloat maxWidth, nfloat maxHeight)
{
    var maxResizeFactor = Math.Min(maxWidth / image.Size.Width, maxHeight / image.Size.Height);
    var width = maxResizeFactor * image.Size.Width;
    var height = maxResizeFactor * image.Size.Height;
    return image.Scale(new CoreGraphics.CGSize(width, height));
}
		//// resize the image to be contained within a maximum width and height, keeping aspect ratio
		//public UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
		//{
		//	var sourceSize = sourceImage.Size;
  //          float maxResizeFactor = (float)Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
		//	if (maxResizeFactor > 1) return sourceImage;
  //          float width = maxResizeFactor * (float)sourceSize.Width;
  //          float height = maxResizeFactor * (float)sourceSize.Height;
		//	UIGraphics.BeginImageContext(new SizeF(width, height));
		//	sourceImage.Draw(new RectangleF(0, 0, width, height));
		//	var resultImage = UIGraphics.GetImageFromCurrentImageContext();
		//	UIGraphics.EndImageContext();
		//	return resultImage;
		//}

		//// resize the image (without trying to maintain aspect ratio)
		//public UIImage ResizeImage(UIImage sourceImage, float width, float height)
		//{
		//	UIGraphics.BeginImageContext(new SizeF(width, height));
		//	sourceImage.Draw(new RectangleF(0, 0, width, height));
		//	var resultImage = UIGraphics.GetImageFromCurrentImageContext();
		//	UIGraphics.EndImageContext();
		//	return resultImage;
		//}

		//// crop the image, without resizing
		//private UIImage CropImage(UIImage sourceImage, int crop_x, int crop_y, int width, int height)
		//{
		//	var imgSize = sourceImage.Size;
		//	UIGraphics.BeginImageContext(new SizeF(width, height));
		//	var context = UIGraphics.GetCurrentContext();
		//	var clippedRect = new RectangleF(0, 0, width, height);
		//	context.ClipToRect(clippedRect);
		//	var drawRect = new RectangleF(-crop_x, -crop_y, imgSize.Width, imgSize.Height);
		//	sourceImage.Draw(drawRect);
		//	var modifiedImage = UIGraphics.GetImageFromCurrentImageContext();
		//	UIGraphics.EndImageContext();
		//	return modifiedImage;
		//}
        //End Manual
    }
}
