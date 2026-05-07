using Microsoft.AspNetCore.Http;
using Moq;
using sprintFlow.API.Middleware;

namespace SprintFlow.Tests.Middleware;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task Invoke_WhenNoExceptionThrown_ShouldCallNextDelegate()
    {
        // Arrange
        var nextDelegateMock = new Mock<RequestDelegate>();

        var context = new DefaultHttpContext();

        var middleware = new ExceptionMiddleware(nextDelegateMock.Object);

        nextDelegateMock
            .Setup(next => next.Invoke(context))
            .Returns(Task.CompletedTask);

        // Act
        await middleware.Invoke(context);

        // Assert
        nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
    }

}
