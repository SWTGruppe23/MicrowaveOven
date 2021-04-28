using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace Microwave.Test.Implementation
{
    [TestFixture]
    public class IT2_UserInterfaceCookController
    {
        private CookController _cookController;
        private Button _startCancelBtn;
        private Door _door;
        private UserInterface _userInterface;

        private IButton _fakeTimeBtn;
        private IButton _fakePowerBtn;
        private IPowerTube _fakePowerTube;
        private IDisplay _fakeDisplay;
        private ILight _fakeLight;
        private ITimer _fakeTimer;

        [SetUp]
        public void Setup()
        {
            _fakeTimeBtn = Substitute.For<IButton>();
            _fakePowerBtn = Substitute.For<IButton>();
            _fakePowerTube = Substitute.For<IPowerTube>();
            _fakeDisplay = Substitute.For<IDisplay>();
            _fakeLight = Substitute.For<ILight>();
            _fakeTimer = Substitute.For<ITimer>();
            
            _startCancelBtn = new Button();
            _door = new Door();
            _cookController = new CookController(_fakeTimer, _fakeDisplay, _fakePowerTube);

            _userInterface = new UserInterface(_fakePowerBtn,
                _fakeTimeBtn,
                _startCancelBtn,
                _door,
                _fakeDisplay,
                _fakeLight,
                _cookController);

            // Completing double association
            _cookController.UI = _userInterface;
        }

        [Test]
        public void UserInterfaceOnStartCancelPressed_TimerStart_RecievedCall()
        {

        }
    }
}