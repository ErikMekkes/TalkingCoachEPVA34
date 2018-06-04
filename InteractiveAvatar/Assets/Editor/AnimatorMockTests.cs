using NUnit.Framework;
using Moq;

/// <summary>
/// Class with test cases with mocks of the AnimatorManager.
/// </summary>
[TestFixture]
public class AnimatorMockTests {
    private ApplicationManager _applicationManager;
    private Mock<AnimatorManager> _animatorManager;

    /// <summary>
    /// Runs before each tests, and is a setup for the application- and animatormanager object.
    /// </summary>
    [SetUp]
    public void BeforeEach() {
        _applicationManager = new ApplicationManager();
        _animatorManager = new Mock<AnimatorManager>();
        _applicationManager.Set_AnimatorManager(_animatorManager.Object);
    }

    /// <summary>
    /// Test SetTalk of the ApplicationManager, which interacts with the animator.
    /// </summary>
    [Test]
    public void SetTalkParameterTest() {
        _applicationManager.SetTalk(true);
        _animatorManager.Verify(x => x.SetBoolAnimator("talk", true), Times.Once());
    }

    /// <summary>
    /// Test StopAnimation of the ApplicationManager, which interacts with the animator.
    /// </summary>
    [Test]
    public void StopAnimationTest() {
        _applicationManager.StopAnimation();
        _animatorManager.Verify(x => x.SetBoolAnimator("talk", false), Times.Once());
        _animatorManager.Verify(x => x.CrossfadeAnimator("idle", It.IsAny<float>(), It.IsAny<int>()), Times.Once());
    }
}