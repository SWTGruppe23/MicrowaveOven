using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace Microwave.Test.Implementation
{
    [TestFixture]
    public class IT2_UserInterfaceDisplayLight
    {
        private Button _startCancelBtn;
        private Button _timeBtn;
        private Button _powerBtn;
        private Door _door;
        private Display _display;
        private Light _light;
        private UserInterface _userInterface;

        private IOutput _fakeOutput;
        private ICookController _fakeCookController;

        [SetUp]
        public void Setup()
        {
            _fakeCookController = Substitute.For<ICookController>();
            _fakeOutput = Substitute.For<IOutput>();
            _startCancelBtn = new Button();
            _timeBtn = new Button();
            _powerBtn = new Button();
            _door = new Door();
            _display = new Display(_fakeOutput);
            _light = new Light(_fakeOutput);
            _userInterface = new UserInterface(
                _powerBtn,
                _timeBtn,
                _startCancelBtn,
                _door,
                _display,
                _light,
                _fakeCookController);

        }

        // Test af metoden TurnOn() i Light
        [Test]
        public void DoorOpen_OutputLogLine_RecievedLightOn() 
        {
            _door.Open();
            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => 
                s.ToLower().Contains("light") 
                && 
                s.ToLower().Contains("on")));
        }

        // Test af metoden TurnOff() i Light
        [Test]
        public void DoorClosed_OutputLogLine_RecievedLightOff() 
        {
            _door.Open();
            _door.Close();
            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => 
                s.ToLower().Contains("light")
                &&
                s.ToLower().Contains("off")));
        }

        // Test af metoden ShowPower() i Display 
        [TestCase(1)]
        [TestCase(5)]
        public void PowerBtnPress_OutputLogLine_RecievedCorrectPower(int number) 
        {
            _door.Open();
            _door.Close();

            for (int i = 0; i < number; i++)
                _powerBtn.Press();

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s =>
            s.ToLower().Contains("display")
            &&
            s.ToLower().Contains($"{number*50}")));
        }

        // Test af metoden ShowTime() i Display
        [Test]
        public void TimeBtnPress_OutputLogLine_RecievedCorrectTime()
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _timeBtn.Press();
            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s =>
                s.ToLower().Contains("display")
                &&
                s.ToLower().Contains("1")));
        }

        // Test af metoden Clear() i Display
        [Test]
        public void DoorOpen_OutputLogLine_RecievedCleared()
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _timeBtn.Press();
            _startCancelBtn.Press();

            _door.Open();

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s =>
                s.ToLower().Contains("display")
                &&
                s.ToLower().Contains("cleared")));
        }
    }
}
