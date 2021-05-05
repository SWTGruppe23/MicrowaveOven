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

        // Test af light ved at åbne lågen til mikro ovnen og teste på lights output at den skriver det korrekte udfald.
        [Test]
        public void DoorOpen_OutputLogLine_RecievedLightOn() 
        {
            _door.Open();
            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => 
                s.ToLower().Contains("light") 
                && 
                s.ToLower().Contains("on")));
        }

        // Test af light ved at lukke lågen til mikro ovnen og teste på lights output at den skriver det korrekte udfald.
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

        // Test af power-outputtet ved at kontrollere ved hvert tryk at det korrekte power vises 
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

        // Test af outputtet fra timer når timerBtn trykkes én gang.
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

        // Test af om når tiden fra timer er udløbet så stopper mikro ovnen og Clear() kaldes.
        [Test]
        public void CookControllerOnTimerExpired_OutputLogLine_RecievedCleared()
        {
            // If all methods must be tested, Display.Clear() must be tested here
            // but it cannot be done without a real CookController og by calling UserInterface.CookingIsDone() directly
        }
    }
}
