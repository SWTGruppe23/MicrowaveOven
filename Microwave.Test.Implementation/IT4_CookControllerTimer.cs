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

        [Test]
        public void StartCancelBtnPress_DisplayShowTime_RecievedCall()
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _timeBtn.Press();
            //_fakeDisplay.ClearReceivedCalls();
            _startCancelBtn.Press();
            _fakeDisplay.Received(1).ShowTime(1,0);
        }
    }
}
