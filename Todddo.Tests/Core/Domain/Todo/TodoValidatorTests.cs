using System;
using Todddo.Core.Domain;
using Todddo.Core.Domain.Todo;
using Xunit;

namespace Todddo.Tests.Core.Domain.Todo
{
    public class TodoValidatorTests
    {
        [Theory]
        [InlineData("12")]
        [InlineData("valid task")]
        [InlineData(thousandChars)]
        public void Can_Validate_A_Valid_Todo(string task)
        {
            var subject = new TodoValidator();
            var todo = new TodoEntity(new TodoId("123"), task);
            var result = subject.Validate(todo).Result;

            Assert.True(result.IsRight);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData(thousandOneChars)]
        public void Can_Validate_An_Invalid_Todo(string task)
        {
            var subject = new TodoValidator();
            var todo = new TodoEntity(new TodoId("123"), task);
            var result = subject.Validate(todo).Result;

            Assert.True(result.IsLeft);
            Assert.Equal(DomainErrorCode.FailedValidation, result.LeftAsEnumerable().Head.ErrorCode);
        }

        private const string thousandChars =
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890" +
            "1234567890" + "1234567890" + "1234567890" + "1234567890" + "1234567890";

        private const string thousandOneChars = thousandChars + "1";
    }
}
