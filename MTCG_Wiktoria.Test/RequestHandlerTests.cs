using NUnit.Framework;
using MTCG_Wiktoria.Server;
using System;
using System.Collections.Generic;

namespace MTCG_Wiktoria.Tests
{
    [TestFixture]
    public class RequestHandlerTests
    {
        

        [Test]
        public void GetRequest_ValidCardsEndpoint_Returns200WithCards()
        {
            var result = RequestHandler.GetRequest("/cards", null);
            Assert.AreEqual(200, result.Item1);
            Assert.IsTrue(result.Item2.Contains("Dragon"));
        }

        [Test]
        public void GetRequest_InvalidEndpoint_Returns404()
        {
            var result = RequestHandler.GetRequest("/invalid-endpoint", null);
            Assert.AreEqual(404, result.Item1);
        }
        

        [Test]
        public void PostRequest_SignupExistingUser_Returns409()
        {
            RequestHandler.PostRequest("/signup?username=existing_user&password=test_pass");
            var result = RequestHandler.PostRequest("/signup?username=existing_user&password=test_pass");
            Assert.AreEqual(409, result.Item1);
        }

        [Test]
        public void PostRequest_LoginValidUser_Returns200WithToken()
        {
            RequestHandler.PostRequest("/signup?username=test_user&password=test_pass");
            var result = RequestHandler.PostRequest("/login?username=test_user&password=test_pass");
            Assert.AreEqual(200, result.Item1);
            Assert.IsTrue(result.Item2.Contains("Token"));
        }

        [Test]
        public void PostRequest_LoginInvalidUser_Returns404()
        {
            var result = RequestHandler.PostRequest("/login?username=nonexistent_user&password=test_pass");
            Assert.AreEqual(404, result.Item1);
        }

        [Test]
        public void PostRequest_DeleteUser_ValidToken_Returns200()
        {
            RequestHandler.PostRequest("/signup?username=test_user&password=test_pass");
            var loginResult = RequestHandler.PostRequest("/login?username=test_user&password=test_pass");
            var token = ExtractToken(loginResult.Item2);
            var result = RequestHandler.PostRequest($"/delete?username=test_user&token={token}");
            Assert.AreEqual(200, result.Item1);
        }

        [Test]
        public void PostRequest_DeleteUser_InvalidToken_Returns401()
        {
            var result = RequestHandler.PostRequest("/delete?username=test_user&token=invalid_token");
            Assert.AreEqual(401, result.Item1);
        }

        [Test]
        public void PostRequest_UpdatePassword_ValidToken_Returns200()
        {
            RequestHandler.PostRequest("/signup?username=test_user&password=test_pass");
            var loginResult = RequestHandler.PostRequest("/login?username=test_user&password=test_pass");
            var token = ExtractToken(loginResult.Item2);
            var result = RequestHandler.PostRequest($"/update-password?username=test_user&newPassword=new_pass&token={token}");
            Assert.AreEqual(200, result.Item1);
        }

        [Test]
        public void PostRequest_UpdatePassword_InvalidToken_Returns401()
        {
            var result = RequestHandler.PostRequest("/update-password?username=test_user&newPassword=new_pass&token=invalid_token");
            Assert.AreEqual(401, result.Item1);
        }

        [Test]
        public void PostRequest_Logout_ValidToken_Returns200()
        {
            RequestHandler.PostRequest("/signup?username=test_user&password=test_pass");
            var loginResult = RequestHandler.PostRequest("/login?username=test_user&password=test_pass");
            var token = ExtractToken(loginResult.Item2);
            var result = RequestHandler.PostRequest($"/logout?token={token}");
            Assert.AreEqual(200, result.Item1);
        }

        private string ExtractToken(string response)
        {
            var tokenIndex = response.IndexOf("Token: ");
            return response.Substring(tokenIndex + 7).Trim();
        }
    }
}
