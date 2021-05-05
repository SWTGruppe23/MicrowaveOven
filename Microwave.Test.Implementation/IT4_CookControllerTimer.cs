using System;
using System.Threading;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NSubstitute.Routing.Handlers;
using NUnit.Framework;
using Timer = Microwave.Classes.Boundary.Timer;

namespace Microwave.Test.Implementation
{
    [TestFixture]
    public class IT4_CookControllerTimer
    {
        private Button _startCancelBtn;
        private Button _powerBtn;
        private Button _timeBtn;
        private Door _door;
        private Timer _timer;
        private CookController _cookController;
        private UserInterface _userInterface;

        private IPowerTube _fakePowerTube;
        private IDisplay _fakeDisplay;
        private ILight _fakeLight;

        [SetUp]
        public void Setup()
        {
            _fakePowerTube = Substitute.For<IPowerTube>();
            _fakeDisplay = Substitute.For<IDisplay>();
            _fakeLight = Substitute.For<ILight>();

            _startCancelBtn = new Button();
            _timeBtn = new Button();
            _powerBtn = new Button();
            _door = new Door();
            _timer = new Timer();
            _cookController = new CookController(_timer, _fakeDisplay, _fakePowerTube);

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

        // Test of method Start() in Timer, property TimeRemaining in Timer and event handler OnTimerTick() in CookController
        [Test]
        public void StartCancelBtnPress_DisplayShowTime_RecievedCall()
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _timeBtn.Press();
            _startCancelBtn.Press();
            _fakeDisplay.Received(1).ShowTime(1,0);
        }

        // Test of event handler OnTimerExpired() in CookController
        [Test]
        public void StartCancelBtnPress_DisplayClear_RecievedCall()
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _timeBtn.Press();
            _startCancelBtn.Press();

            Thread.Sleep(61000);
            _fakeDisplay.Received(1).Clear();
        }

        // Test of method Stop() in Timer
        [Test]
        public void StartCancelBtnPress_LightTurnOff_RecievedNoCall()
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _timeBtn.Press();
            _startCancelBtn.Press();
            _fakeLight.ClearReceivedCalls();

            _door.Open();

            Thread.Sleep(61000);
            _fakeLight.Received(0).TurnOff();
        }
    }
}
