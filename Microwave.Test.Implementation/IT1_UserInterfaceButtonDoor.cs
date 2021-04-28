using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace Microwave.Test.Implementation
{
    public class Tests
    {
        private IButton _startCancelBtn;
        private IButton _timeBtn;
        private IButton _powerBtn;
        private IDoor _door;
        private IDisplay _fakeDisplay;
        private ILight _fakeLight;
        private ICookController _fakeCookController;
        private IUserInterface _userInterface;

        [SetUp]
        public void Setup()
        {
            _startCancelBtn = new Button();
            _timeBtn = new Button();
            _powerBtn = new Button();
            _door = new Door();
            _fakeDisplay = Substitute.For<IDisplay>();
            _fakeLight = Substitute.For<ILight>();
            _fakeCookController = Substitute.For<ICookController>();
            _userInterface = new UserInterface(
                _powerBtn,
                _timeBtn,
                _startCancelBtn,
                _door,
                _fakeDisplay,
                _fakeLight,
                _fakeCookController);

        }

        [Test]
        public void DoorOpen_LightTurnOn_RecievedCall()
        {
            _door.Open();
            _fakeLight.Received(1).TurnOn();
        }

        [Test]
        public void PowerBtnPress_DisplayShowPower_RecievedCall()
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _fakeDisplay.Received(1).ShowPower(50); // requires knowledge that power increases with 50W per press
        }

        [Test]
        public void TimerShowTime_RecievedCall()
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _timeBtn.Press();
            _fakeDisplay.Received(1).ShowTime(1, 0); // requires knowledge of how much time increases per press
        }

        [Test]
        public void StartCancelBtnPress_CookControllerStartCooking_RecievedCall()
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _timeBtn.Press();
            _fakeCookController.Received(1).StartCooking(50, 1*60);
        }
    }
}