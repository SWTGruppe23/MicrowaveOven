using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace Microwave.Test.Implementation
{
    [TestFixture]
    public class IT1_UserInterfaceButtonDoor
    {
        private Button _startCancelBtn;
        private Button _timeBtn;
        private Button _powerBtn;
        private Door _door;
        private UserInterface _userInterface;

        private IDisplay _fakeDisplay;
        private ILight _fakeLight;
        private ICookController _fakeCookController;

        [SetUp]
        public void Setup()
        {
            _fakeDisplay = Substitute.For<IDisplay>();
            _fakeLight = Substitute.For<ILight>();
            _fakeCookController = Substitute.For<ICookController>();
            _startCancelBtn = new Button();
            _timeBtn = new Button();
            _powerBtn = new Button();
            _door = new Door();
            _userInterface = new UserInterface(
                _powerBtn,
                _timeBtn,
                _startCancelBtn,
                _door,
                _fakeDisplay,
                _fakeLight,
                _fakeCookController);

        }

        // Test af event handler OnDoorOpened() i UserInterface
        [Test]
        public void DoorOpen_LightTurnOn_RecievedCall() 
        {
            _door.Open();
            _fakeLight.Received(1).TurnOn();
        }

        // Test af event handler OnDoorClosed() i UserInterface
        [Test]
        public void DoorClosed_LightTurnOff_RecievedCall() 
        {
            _door.Open();
            _door.Close();
            _fakeLight.Received(1).TurnOff();
        }

        // Test af event handler OnPowerPressed() i UserInterface
        [Test]
        public void PowerBtnPress_DisplayShowPower_RecievedCall() 
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _fakeDisplay.Received(1).ShowPower(50); // requires knowledge that power increases with 50W per press
        }

        // Test af event handler OnTimePressed() i UserInterface
        [Test]
        public void TimerShowTime_RecievedCall() 
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _timeBtn.Press();
            _fakeDisplay.Received(1).ShowTime(1, 0); // requires knowledge of how much time increases per press
        }

        // Test af event handler OnStartCancelPressed() i UserInterface
        [Test]
        public void StartCancelBtnPress_CookControllerStartCooking_RecievedCall() 
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _timeBtn.Press();
            _startCancelBtn.Press();
            _fakeCookController.Received(1).StartCooking(50, 60);
        }
    }
}