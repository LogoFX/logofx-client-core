using FluentAssertions;
using Xunit;

namespace LogoFX.Client.Core.Tests
{
    public class ReentranceGuardTests
    {
        [Fact]
        public void AccessingSemaphore_SemaphoreIsRaisedOnce_IsLockedIsFalse()
        {
            var guard = new ReentranceGuard();
            guard.Raise();

            var isLocked = guard.IsLocked;

            isLocked.Should().BeFalse();
        }

        [Fact]
        public void AccessingSemaphore_SemaphoreIsRaisedTwice_IsLockedIsTrue()
        {
            var guard = new ReentranceGuard();
            guard.Raise();
            guard.Raise();

            var isLocked = guard.IsLocked;

            isLocked.Should().BeTrue();
        }

        [Fact]
        public void AccessingSemaphore_SemaphoreIsRaisedTrice_IsLockedIsTrue()
        {
            var guard = new ReentranceGuard();
            guard.Raise();
            guard.Raise();
            guard.Raise();

            var isLocked = guard.IsLocked;

            isLocked.Should().BeTrue();
        }

        [Fact]
        public void AccessingSemaphore_SemaphoreIsRaisedTwiceThenDisposeCalled_IsLockedIsFalse()
        {
            var guard = new ReentranceGuard();
            guard.Raise();
            using (guard.Raise())
            {
                
            }

            var isLocked = guard.IsLocked;

            isLocked.Should().BeFalse();
        }
    }
}
