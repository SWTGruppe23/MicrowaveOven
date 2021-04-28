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

        [Test]
        public void DoorOpen_OutputLogLine_RecievedLightOn()
        {
            _door.Open();
            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => 
                s.ToLower().Contains("light") 
                && 
                s.ToLower().Contains("on")));
        }

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

        [Test]
        public void PowerBtnPress_OutputLogLine_RecievedPower50()
        {
            _door.Open();
            _door.Close();
            _powerBtn.Press();
            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s =>
                s.ToLower().Contains("display")
                &&
                s.ToLower().Contains("50")));
        }

        [Test]
        public void TimeBtnPress_OutputLogLine_RecievedTime1()
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

        [Test]
        public void CookControllerCookingIsDone_OutputLogLine_RecievedCleared()
        {
            // If all methods must be tested, Display.Clear() must be tested here
            // but it cannot be done without a real CookController og by calling UserInterface.CookingIsDone() directly
        }
    }
}
