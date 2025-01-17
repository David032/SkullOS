﻿using Moq;
using skullOS.HardwareServices.Interfaces;
using skullOS.Modules;

namespace ModuleTests
{
    public class CameraTest
    {
        Mock<ICameraService> camMock = new();
        Mock<IMicrophoneService> micMock = new();
        Mock<IBuzzerService> buzzerMock = new();
        Mock<ISpeakerService> speakerMock = new();
        Mock<ILedService> LedMock = new();

        [Fact]
        public void CanCreate()
        {
            Camera sut = new Camera(camMock.Object, micMock.Object, speakerMock.Object, LedMock.Object, buzzerMock.Object, @"TestData/CameraSettings.txt");
            Assert.NotNull(sut);
        }

        [Fact]
        public async void CanTakePicture()
        {
            camMock.Setup(camera => camera.TakePictureAsync(It.IsAny<string>())).Returns(Task.FromResult("Pass")).Verifiable();

            Camera sut = new Camera(camMock.Object, micMock.Object, speakerMock.Object, LedMock.Object, buzzerMock.Object, @"TestData/CameraSettings.txt");

            await sut.TakePicture();
            Mock.Verify(camMock);
        }
        [Fact]
        public async void CanRecordVideo()
        {
            camMock.Setup(camera =>
                camera.RecordShortVideoAsync(It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.FromResult("Pass")).Verifiable();

            Camera sut = new Camera(camMock.Object, micMock.Object, speakerMock.Object, LedMock.Object, buzzerMock.Object, @"TestData/CameraSettings.txt");

            await sut.RecordShortVideo();
            Mock.Verify(camMock);
        }
    }
}
