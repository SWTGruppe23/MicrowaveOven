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

        [Test]
        public void DoorOpen_LightTurnOn_RecievedCall() // Test af om lyset tænder i mikro ovnen når lågen åbnes
        {
            _door.Open();
            _fakeLight.Received(1).TurnOn();
        }

        [Test]
        public void DoorClosed_LightTurnOff_RecievedCall() // Test af om lyset slukker i mikro ovnen når lågen lukkes
        {
            _door.Open();
            _door.Close();
            _fakeLight.Received(1).TurnOff();
        }

        [Test]
        public void PowerBtnPress_DisplayShowPower_RecievedCall() // Test af når der trykkes på powerBtn at displayet på mikro ovnen viser ønsket antal watt på med ét tryk (50W)
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _fakeDisplay.Received(1).ShowPower(50); // requires knowledge that power increases with 50W per press
        }

        [Test]
        public void TimerShowTime_RecievedCall() // Test af når der trykkes på timerBtn at Display viser ønsket tid som mikro ovnen kører ved ét tryk (1 min. 0 s.).
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _timeBtn.Press();
            _fakeDisplay.Received(1).ShowTime(1, 0); // requires knowledge of how much time increases per press
        }

        [Test]
        public void StartCancelBtnPress_CookControllerStartCooking_RecievedCall() // Test af når der trykkes på startCancelBtn at mikro ovnen går i gang med 50W og i 1 minut. 
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