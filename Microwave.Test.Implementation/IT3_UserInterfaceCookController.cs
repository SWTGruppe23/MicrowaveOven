using System;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NSubstitute.Routing.Handlers;
using NUnit.Framework;

namespace Microwave.Test.Implementation
{
    [TestFixture]
    public class IT2_UserInterfaceCookController
    {
        private CookController _cookController;
        private Button _startCancelBtn;
        private Button _powerBtn;
        private Button _timeBtn;
        private Door _door;
        private UserInterface _userInterface;

        private IPowerTube _fakePowerTube;
        private IDisplay _fakeDisplay;
        private ILight _fakeLight;
        private ITimer _fakeTimer;

        [SetUp]
        public void Setup()
        {
            _fakePowerTube = Substitute.For<IPowerTube>();
            _fakeDisplay = Substitute.For<IDisplay>();
            _fakeLight = Substitute.For<ILight>();
            _fakeTimer = Substitute.For<ITimer>();
            
            _startCancelBtn = new Button();
            _timeBtn = new Button();
            _powerBtn = new Button();
            _door = new Door();
            _cookController = new CookController(_fakeTimer, _fakeDisplay, _fakePowerTube);

            _userInterface = new UserInterface(_powerBtn,
                _timeBtn,
                _startCancelBtn,
                _door,
                _fakeDisplay,
                _fakeLight,
                _cookController);

            // Completing double association
            _cookController.UI = _userInterface;
        }

        [Test]
        public void StartCancelBtnPress_TimerStart_RecievedStart60()
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _timeBtn.Press();
            _startCancelBtn.Press();
            _fakeTimer.Received(1).Start(60);
        }

        [Test]
        public void CookControllerOnTimerTick_LightTurnOff_RecievedCall()
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _timeBtn.Press();
            _startCancelBtn.Press();
            _fakeLight.ClearReceivedCalls();
            _fakeTimer.Expired += Raise.Event();
            _fakeLight.Received(1).TurnOff();
        }


    }
}