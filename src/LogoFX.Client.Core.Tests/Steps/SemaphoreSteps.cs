using FluentAssertions;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Tests
{
    [Binding]
    internal sealed class SemaphoreSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public SemaphoreSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [When(@"The semaphore is created")]
        public void WhenTheSemaphoreIsCreated()
        {
            var semaphore = new ReentranceGuard();
            _scenarioContext.Add("semaphore", semaphore);
        }

        [When(@"The semaphore is raised")]
        public void WhenTheSemaphoreIsRaised()
        {
            var semaphore = _scenarioContext.Get<ReentranceGuard>("semaphore");
            semaphore.Raise();
        }

        [When(@"The semaphore is raised and disposed")]
        public void WhenTheSemaphoreIsRaisedAndDisposed()
        {
            var semaphore = _scenarioContext.Get<ReentranceGuard>("semaphore");
            using (semaphore.Raise())
            {

            }
        }

        [Then(@"The semaphore should not be locked")]
        public void ThenTheSemaphoreShouldNotBeLocked()
        {
            var semaphore = _scenarioContext.Get<ReentranceGuard>("semaphore");
            semaphore.IsLocked.Should().BeFalse();
        }

        [Then(@"The semaphore should be locked")]
        public void ThenTheSemaphoreShouldBeLocked()
        {
            var semaphore = _scenarioContext.Get<ReentranceGuard>("semaphore");
            semaphore.IsLocked.Should().BeTrue();
        }
    }
}
