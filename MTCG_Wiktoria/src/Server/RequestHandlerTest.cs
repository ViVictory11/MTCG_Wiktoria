using System;
using MTCG_Wiktoria.Server;

namespace MTCG_Wiktoria
{
    public class RequestHandlerTests
    {
        public void RunAllTests()
        {
            try
            {
                TestGetCards();
                Console.WriteLine("TestGetCards: Passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TestGetCards: Failed - {ex.Message}");
            }

            try
            {
                TestSignupNewUser();
                Console.WriteLine("TestSignupNewUser: Passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TestSignupNewUser: Failed - {ex.Message}");
            }

            try
            {
                TestLoginWithCorrectCredentials();
                Console.WriteLine("TestLoginWithCorrectCredentials: Passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TestLoginWithCorrectCredentials: Failed - {ex.Message}");
            }
        }

        private void TestGetCards()
        {
            var (statusCode, responseBody) = RequestHandler.GetRequest("/cards", null);
            if (statusCode != 200 || !responseBody.Contains("Dragon") || !responseBody.Contains("Water Blast"))
            {
                throw new Exception("Expected 200 status and card details in response.");
            }
        }

        private void TestSignupNewUser()
        {
            var (statusCode, responseBody) = RequestHandler.PostRequest("/signup?username=new_user&password=new_password");
            if (statusCode != 200 || responseBody != "User signed up successfully.")
            {
                throw new Exception("Expected 200 status and success message for signup.");
            }
        }

        private void TestLoginWithCorrectCredentials()
        {
            // Ensure the user is signed up first
            RequestHandler.PostRequest("/signup?username=login_user&password=login_password");

            var (statusCode, responseBody) = RequestHandler.PostRequest("/login?username=login_user&password=login_password");
            if (statusCode != 200 || !responseBody.Contains("User logged in successfully"))
            {
                throw new Exception("Expected 200 status and success message for login.");
            }
        }
    }
}
