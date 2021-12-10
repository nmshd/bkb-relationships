using Enmeshed.BuildingBlocks.Application.Abstractions.Exceptions;
using FluentAssertions;
using FluentAssertions.Specialized;

namespace Relationships.Application.Tests.FluentAssertions.Extensions;

public static class ExceptionAssertionsExtensions
{
    public static void WithErrorCode(this ExceptionAssertions<OperationFailedException> exceptionAssertions, string errorCode)
    {
        exceptionAssertions.Which.Code.Should().Be(errorCode);
    }
}
