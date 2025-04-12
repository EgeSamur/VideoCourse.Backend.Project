//using Amazon.Rekognition;
//using Amazon.Rekognition.Model;
//using Microsoft.AspNetCore.Http;
//using VideoCourse.Backend.Application.Abstractions.Services;
//using VideoCourse.Backend.Application.Features.Users.DTOs;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//public class RekognitionImageVerificationService : IImageVerificationService
//{
//    private readonly IAmazonRekognition _rekognitionClient;

//    public RekognitionImageVerificationService(IAmazonRekognition rekognitionClient)
//    {
//        _rekognitionClient = rekognitionClient;
//    }

//    public async Task<bool> VerifyImageAsync(ImageVerificationDto dto)
//    {
//        //byte[] profileImageBytes;
//        //byte[] liveImageBytes;

//        //// Convert profileImage to byte array
//        //using (var profileImageStream = new MemoryStream())
//        //{
//        //    await dto.ProfileImage.CopyToAsync(profileImageStream);
//        //    profileImageBytes = profileImageStream.ToArray();
//        //}

//        //// Convert liveImage to byte array
//        //using (var liveImageStream = new MemoryStream())
//        //{
//        //    await dto.LiveImage.CopyToAsync(liveImageStream);
//        //    liveImageBytes = liveImageStream.ToArray();
//        //}

//        //// Prepare AWS Rekognition requests
//        //var profileImageRequest = new Image
//        //{
//        //    Bytes = new MemoryStream(profileImageBytes)
//        //};

//        //var liveImageRequest = new Image
//        //{
//        //    Bytes = new MemoryStream(liveImageBytes)
//        //};

//        //var compareFacesRequest = new CompareFacesRequest
//        //{
//        //    SourceImage = profileImageRequest,
//        //    TargetImage = liveImageRequest,
//        //    SimilarityThreshold = 70 // Minimum similarity threshold
//        //};

//        //// Call AWS Rekognition CompareFaces API
//        //var response = await _rekognitionClient.CompareFacesAsync(compareFacesRequest);

//        //return response.FaceMatches.Any();
//        throw new Exception();
//    }

//    public async Task<bool> VerifyImageWithGestureCheckAsync(ImageVerificationDto dto , string mainImageUrl)
//    {
//        byte[] liveImageBytes;
//        // Convert liveImage to byte array
//        using (var liveImageStream = new MemoryStream())
//        {
//            await dto.LiveImage.CopyToAsync(liveImageStream);
//            liveImageBytes = liveImageStream.ToArray();
//        }
//        var detectLabelsRequest = new DetectLabelsRequest
//        {
//            Image = new Amazon.Rekognition.Model.Image
//            {
//                Bytes = new MemoryStream(liveImageBytes)
//            },
//            MaxLabels = 10,
//            MinConfidence = 70 
//        };
//        var detectLabelsResponse = await _rekognitionClient.DetectLabelsAsync(detectLabelsRequest);
//        // Check if a hand or gesture is detected
//        var fingerDetected = detectLabelsResponse.Labels.Any(label =>
//            label.Name.Equals("Finger", StringComparison.OrdinalIgnoreCase) &&
//            label.Parents.Any(parent => parent.Name.Equals("Hand", StringComparison.OrdinalIgnoreCase)));
//        if (!fingerDetected)
//        {
//            // If no hand gesture is detected, skip face comparison
//            return false;
//        }

//        // S3 URL'sinden profil resmini indir
//        byte[] profileImageBytes;
//        using (var httpClient = new HttpClient())
//        {
//            profileImageBytes = await httpClient.GetByteArrayAsync(mainImageUrl);
//        }
//        var compareFacesRequest = new CompareFacesRequest
//        {
//            SourceImage = new Amazon.Rekognition.Model.Image
//            {
//                Bytes = new MemoryStream(profileImageBytes)
//            },
//            TargetImage = new Amazon.Rekognition.Model.Image
//            {
//                Bytes = new MemoryStream(liveImageBytes)
//            },
//            SimilarityThreshold = 70
//        };
//        var compareFacesResponse = await _rekognitionClient.CompareFacesAsync(compareFacesRequest);

//        return compareFacesResponse.FaceMatches.Any();
//    }

//}
