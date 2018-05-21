using System;
using NetGrpcSentryTest.Helpers;
using NetGrpcSentryTest.Service;
using NUnit.Framework;

namespace NetGrpcSentryTest
{
    [TestFixture]
    public class InterceptorTest
    {
        private TestServer _server;
        private TestService.TestServiceClient _client;
        private bool _actionMade;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _server = new TestServer(ActionServer);
            _client = TestClient.Create(_server.Port, ActionClient);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _server.Dispose();
        }
        
        private void ActionServer(Exception exception)
        {
            _actionMade = true;
        }

        private void ActionClient(Exception exception)
        {
            _actionMade = true;
        }

        [Test]
        public void ServerInterceptor_True()
        {
            // Arrange
            _actionMade = false;

            // Act
            try
            {
                _client.ThrowsExcpetion(new Request());
            }
            catch (Exception)
            {
                // ignored
            }

            // Assert
            Assert.IsTrue(_actionMade);
        }

        [Test]
        public void ServerInterceptor_False()
        {
            // Arrange
            _actionMade = false;

            // Act
            try
            {
                _client.ThrowsRpc(new Request());
            }
            catch (Exception)
            {
                // ignored
            }

            // Assert
            Assert.IsFalse(_actionMade);
        }

        [Test]
        public void ClientInterceptor_True()
        {
            // Arrange
            _actionMade = false;

            // Act
            try
            {
                _client.ThrowsExcpetion(new Request());
            }
            catch (Exception)
            {
                // ignored
            }

            // Assert
            Assert.IsTrue(_actionMade);
        }

        [Test]
        public void ClientInterceptor_False()
        {
            // Arrange
            _actionMade = false;

            // Act
            try
            {
                _client.ThrowsRpc(new Request());
            }
            catch (Exception)
            {
                // ignored
            }

            // Assert
            Assert.IsFalse(_actionMade);
        }
    }
}
